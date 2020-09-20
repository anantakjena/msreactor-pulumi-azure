using msreactor.common;
using Newtonsoft.Json;
using Pulumi;
using Pulumi.Azure.AppService;
using Pulumi.Azure.AppService.Inputs;
using Pulumi.Azure.Core;
using Pulumi.Azure.Sql;
using System;
using System.Linq;
using Azure = Pulumi.Azure;
using AzureStorage = Pulumi.Azure.Storage;

class MyStack : Stack
{
    public MyStack()
    {
        InputMap<string> tags = new InputMap<string>();

        // we want to read from config.json file
        Configuration.Init();

        tags.Add("Environment", Configuration.Instance.Environment);

        // we read resource group properties
        var rg = Configuration.Instance.Services.FirstOrDefault(s => s.Type == Configuration.ResourceGroup);

        Console.WriteLine($"Resource group: {JsonConvert.SerializeObject(rg.Name)}");

        // we define resource group type
        // NOTE we explictly define Name proprety, if we do not do it, pulumi will auto append a unique guid like string to the resource name
        var resourceGroup = new ResourceGroup($"{Configuration.Instance.Environment}{rg.Name}", new ResourceGroupArgs
        {
            Name = $"{Configuration.Instance.Environment}{rg.Name}",
            Location = rg.Location,
            Tags = tags
        });

        // we read app service plan properties
        var asp = Configuration.Instance.Services.FirstOrDefault(s => s.Type == Configuration.AppServicePlan);

        Console.WriteLine($"App service plan: {JsonConvert.SerializeObject(asp.Name)}");

        // we define app service resource type
        // NOTE we explictly define Name proprety, if we do not do it, pulumi will auto append a unique guid like string to the resource name
        var appServicePlan = new Plan($"{Configuration.Instance.Environment}{asp.Name}", new PlanArgs
        {
            Name = $"{Configuration.Instance.Environment}{asp.Name}",
            Location = resourceGroup.Location,
            ResourceGroupName = resourceGroup.Name,
            Kind = asp.Kind,
            Reserved = true,
            Sku = new PlanSkuArgs
            {
                Tier = asp.Sku.Tier,
                Size = asp.Sku.Size,
            },
            Tags = tags
        });

        // we read application insights properties
        var appInsights = Configuration.Instance.Services.FirstOrDefault(s => s.Type == Configuration.ApplicationInsights);

        // we define application insights resource type
        // NOTE we explictly define Name proprety, if we do not do it, pulumi will auto append a unique guid like string to the resource name
        var appServiceInsights = new Pulumi.Azure.AppInsights.Insights($"{Configuration.Instance.Environment}{rg.Name}", new Pulumi.Azure.AppInsights.InsightsArgs
        {
            Name = $"{Configuration.Instance.Environment}{rg.Name}",
            Location = resourceGroup.Location,
            ResourceGroupName = resourceGroup.Name,
            ApplicationType = appInsights.Settings[Configuration.ApplicationType]
        }, new CustomResourceOptions
        {
            DependsOn = resourceGroup
        });

        // we optionally define app settings inputmap type
        var appServiceSettings = new InputMap<string>();

        appServiceSettings.Add(Configuration.APPINSIGHTS_INSTRUMENTATIONKEY, appServiceInsights.InstrumentationKey);
        appServiceSettings.Add(Configuration.APPLICATIONINSIGHTS_CONNECTION_STRING, appServiceInsights.InstrumentationKey.Apply(key => $"InstrumentationKey={key}"));

        // we read web portal properties
        var webPortal = Configuration.Instance.Services.FirstOrDefault(s => s.Type == Configuration.Web);

        Console.WriteLine($"Web portal: {webPortal.Name}");

        // we define the web app resource type
        // NOTE we explictly define Name proprety, if we do not do it, pulumi will auto append a unique guid like string to the resource name
        var webAppService = new AppService($"{Configuration.Instance.Environment}{webPortal.Name}", new AppServiceArgs
        {
            Name = $"{Configuration.Instance.Environment}{webPortal.Name}",
            Location = resourceGroup.Location,
            ResourceGroupName = resourceGroup.Name,
            AppServicePlanId = appServicePlan.Id,
            AppSettings = appServiceSettings,
            SiteConfig = new AppServiceSiteConfigArgs
            {
                Cors = new AppServiceSiteConfigCorsArgs
                {
                    AllowedOrigins = "*"
                }

            },
            Tags = tags
        });

        // we read api portal properties
        var api = Configuration.Instance.Services.FirstOrDefault(s => s.Type == Configuration.Api);

        Console.WriteLine($"Api: {api.Name}");

        // we define the api app resource type
        // NOTE we explictly define Name proprety, if we do not do it, pulumi will auto append a unique guid like string to the resource name
        var apiAppService = new AppService($"{Configuration.Instance.Environment}{api.Name}", new AppServiceArgs
        {
            Name = $"{Configuration.Instance.Environment}{api.Name}",
            Location = resourceGroup.Location,
            ResourceGroupName = resourceGroup.Name,
            AppServicePlanId = appServicePlan.Id,
            AppSettings = appServiceSettings,
            SiteConfig = new AppServiceSiteConfigArgs
            {
                Cors = new AppServiceSiteConfigCorsArgs
                {
                    AllowedOrigins = "*"
                },
            },
            Tags = tags
        });
    }
}
