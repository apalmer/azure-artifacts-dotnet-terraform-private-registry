# terraforrm-module-registry

## Development Set Up

From the root of the git repository...

In the directory src/GlintStream.TerraformModuleRegistry/GlintStream.TerraformModuleRegistry...

Update appsettings.json's AzureDevOps node's BaseUrl setting

```
"AzureDevOps": {
    "BaseUrl": "https://your-base-url-here.com",
    "PersonalAccessToken": ""
}
```

You can add the personal token as a dotnet secret with the following:

```
dotnet user-secrets set "AzureDevOps:PersonalAccessToken" "yoursecretpersonalaccesstokenhere" --project ".\GlintStream.TerraformModuleRegistry.csproj"
```

Launch the debug web app:

```
dotnet watch run --project .\GlintStream.TerraformModuleRegistry.csproj
```

You can launch a publically accessible proxy:

```
ngrok http https://localhost:5058
```

From the root of the git repository...

In the directory examples/mock-modules...

You can push test mock modules to your Azure DevOps Artifacts feed

```
.\publish-orville.ps1
.\publish-wilbur.ps1
```

From the root of the git repository...

In the directory examples/terraform_remote_service_discovery_protocol...

Update main.tf terraform file source to reference your domain:

```
module "orville" {
    source = "eeb8-216-66-61-133.ngrok-free.app/wright/orville/flight"
    version = "0.0.1"
}

module "wilbur" {
    source = "eeb8-216-66-61-133.ngrok-free.app/wright/wilbur/flight"
    version = "0.0.1"
}
```

Initialize terraform 

```
terraform init
```

You should be getting a response from your local terraform private repository