# msreactor-pulumi-azure
Hands on for session with #MSReactor Abu Dhabi on Pulumi with Azure

- Slides: [MSReactor Pulumi & Azure](https://1drv.ms/p/s!AuHw0yZ3EZrTgok59Tekerc2CqXgwA?e=tMOxR8)
- Azure DevOps Pipeline: [Build pipelines](https://dev.azure.com/mistryhardik05/MSReactor/_build)
  - Edit pipeline variable `pulumi.access.token` with access_token value from your `Pulumi` account
  
Import the [sample azure devops pipeline](https://github.com/mistryhardik/msreactor-pulumi-azure/tree/development/azure-devops-pipeline)
- Download the json file from here [https://github.com/mistryhardik/msreactor-pulumi-azure/tree/development/azure-devops-pipeline](https://github.com/mistryhardik/msreactor-pulumi-azure/tree/development/azure-devops-pipeline)
- Login to you Azure DevOps account
  - Create a free account [here](https://azure.microsoft.com/en-us/services/devops/?nav=min)
- Navigate to Pipelines menu (blue rocket on the left hand side menu bar ;)
  - On the top right corner, click the 3 dot menu button and look for `Import a pipeline` option, click the `Import a pipeline` option.
- Browse the downloaded .json file
- Click and wait to import
- You will be required to replace the `github repository` (if you have cloned it under your account because of change in the url)
- You will be required to edit other parameters of the pipeline tasks as well, such as `Azure subscription` connection etc.
- Please remember to edit the `pulumi.access.token` pipeline variable in the build pipeline.
- Save changes and run the pipeline

Should you have issues, tweet or DM them to me [@mistryhardik05](https://twitter.com/mistryhardik05)
