@secure()
param pgSqlPassword string
param location string = resourceGroup().location

var uniqueId = uniqueString(resourceGroup().id)

module keyVault 'modules/secrets/keyvault.bicep' = {
  name: 'keyVaultDeployment'
  params: {
    vaultName: 'kv-${uniqueId}'
    location: location
  }
}

module apiService 'modules/compute/appservice.bicep' = {
  name: 'apiDeployment'
  params: {
    appName: 'api-${uniqueId}'
    appServicePlanName: 'plan-api-${uniqueId}'
    keyVaultName: keyVault.outputs.name
    appSettings: [
      {
        name: 'DatabaseName'
        value: 'urls'
      }
      {
        name: 'ContainerName'
        value: 'items'
      }
    ]
    location: location
  }
}

module tokenRangeService 'modules/compute/appservice.bicep' = {
  name: 'tokenRangeServiceDeployment'
  params: {
    appName: 'token-range-service-${uniqueId}'
    appServicePlanName: 'plan-token-range-${uniqueId}'
    keyVaultName: keyVault.outputs.name
    location: location
  }
}

module postgres 'modules/storage/postgres.bicep' = {
  name: 'postgresDeployment'
  params: {
    name: 'postgresql-${uniqueId}'
    location: location
    adminstratorLogin: 'adminuser'
    administratorLoginPassword: pgSqlPassword
    keyVaultName: keyVault.outputs.name
  }
}

module cosmosDb 'modules/storage/cosmos-db.bicep' = {
  name: 'cosmosDbDeployment'
  params: {
    name: 'cosmos-db-${uniqueId}'
    location: location
    kind: 'GlobalDocumentDB'
    databaseName: 'urls'
    locationName: 'West Europe'
    keyVaultName: keyVault.outputs.name
  }
}

module keyVaultRoleAssingment 'modules/secrets/keyvault-role-assignment.bicep' = {
  name: 'keyVaultRoleAssignmentDeployment'
  params: {
    keyVaultName: keyVault.outputs.name
    pricipalIds: [
      apiService.outputs.principalId
    ]
  }
}
