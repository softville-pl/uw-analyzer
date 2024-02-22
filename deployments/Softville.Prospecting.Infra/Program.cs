// See https://aka.ms/new-console-template for more information

using HashiCorp.Cdktf;
using Softville.Prospecting.Infra;

App app = new();

// Define the Remote State Configuration
new AzurermBackend(app,
    new AzurermBackendConfig
    {
        ResourceGroupName = "rg-prspct_infra-test-plc-001",
        StorageAccountName = "stprspctinfratestplc",
        ContainerName = "terraformstate",
        Key = "prspct-test-plc.terraform.tfstate"
    });

// Register the Stack to your App
new AzureInfrastructure(app, "MyAzureInfrastructure",
    "rg-prspct-test-plc-001",
    "cosmos-prspct-test-plc-001",
    "test");
app.Synth();
