$location = 'polandcentral'
$rg_name = 'rg-prspct_infra-test-plc-001'
$app_tag = 'app=prospecting-app'
$env_tag = 'env=test'
$scenario_tag = 'scenario=terraform_state'
$st_name = 'st-prspct_infra-test-plc'

# Create a resource group if you don't already have one
# az group create `
#     --name $rg_name `
#     --location $location `
#     --tags $app_tag $env_tag $scenario_tag

# Create the storage account
az storage account create `
    --name $st_name `
    --resource-group $rg_name `
    --location $location `
    --sku Standard_LRS `
    --tags $app_tag $env_tag $scenario_tag

# Create the blob container
az storage container create `
     --name terraformstate `
     --account-name $st_name
