// See https://aka.ms/new-console-template for more information

using HashiCorp.Cdktf;
using Softville.Prospecting.Infra;

App app = new();


var infrastructure = new InfrastructureInstance("prspct", "test", "001");
// Register the Stack to your App
ProspectingAzureStack stack = new(app, "MyAzureInfrastructure", infrastructure);

// Define the Remote State Configuration
new AzurermBackend(stack,
    new AzurermBackendConfig
    {
        ResourceGroupName = "rg-prspct_infra-test-plc-001",
        StorageAccountName = "stprspctinfratestplc",
        ContainerName = "terraformstate",
        Key = "prspct-test-plc.terraform.tfstate"
    });

app.Synth();
