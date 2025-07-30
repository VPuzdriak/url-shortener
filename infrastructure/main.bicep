param location string = resourceGroup().location
param alternativeLocation string = 'North Europe'

@secure()
param pgSqlPassword string

var uniqueId = uniqueString(resourceGroup().id)

module keyVault 'modules/secrets/keyvault.bicep' = {
  name: 'keyVaultDeployment'
  params: {
    location: location
    vaultName: 'kv-${uniqueId}'
  }
}

module apiService 'modules/compute/appservice.bicep' = {
  name: 'apiDeployment'
  params: {
    location: location
    appServicePlanName: 'plan-api-${uniqueId}'
    appName: 'api-${uniqueId}'
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
  }
}

module tokenRangeService  'modules/compute/appservice.bicep' = {
  name: 'tokenRangeServiceDeployment'
  params: {
    location: location
    appServicePlanName: 'plan-token-range-${uniqueId}'
    appName: 'token-range-service-${uniqueId}'
    keyVaultName: keyVault.outputs.name
  }
}

module pgsql 'modules/storage/postgres.bicep' = {
  name: 'postgresDeployment'
  params: {
    name: 'pgsql-${uniqueId}'
    location: alternativeLocation
    administratorLogin: 'adminUser'
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
    locationName: alternativeLocation
    keyVaultName: keyVault.outputs.name
  }
}

module keyVaultRoleAssignment 'modules/secrets/keyvault-role-assignment.bicep' = {
  name: 'keyVaultRoleAssignmentDeployment'
  params: {
    keyVaultName: keyVault.outputs.name
    principalIds: [apiService.outputs.principalId, tokenRangeService.outputs.principalId]
  }
}
