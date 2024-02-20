// See https://aka.ms/new-console-template for more information

using HashiCorp.Cdktf;
using Softville.Prospecting.Infra;

App app = new();
new AzureInfrastructure(app, "MyAzureInfrastructure",
    "rg-prspct-test-plc-001",
    "cosmos-prspct-test-plc-001",
    "test");
app.Synth();
