using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace msreactor.common
{
    public static class Configuration
    {
        public const string ResourceGroup = "Resource Group";
        public const string AppServicePlan = "App Service Plan";
        public const string ApplicationInsights = "Application Insights";

        public const string Web = "Web";
        public const string Api = "Api";

        public const string ApplicationType = "ApplicationType";

        public const string APPINSIGHTS_INSTRUMENTATIONKEY = "APPINSIGHTS_INSTRUMENTATIONKEY";
        public const string APPLICATIONINSIGHTS_CONNECTION_STRING = "APPLICATIONINSIGHTS_CONNECTION_STRING";

        public static Config Instance { get; set; }

        public static void Init()
        {
            using (StreamReader r = new StreamReader("config.json"))
            {
               string json = r.ReadToEnd();
               Instance = JsonConvert.DeserializeObject<Config>(json);
            }
        }
    }
}
