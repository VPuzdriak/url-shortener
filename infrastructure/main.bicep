@secure()
param pgSqlPassword string
param location string = resourceGroup().location

var uniqueId = uniqueString(resourceGroup().id)
var keyVaultName = 'kv-${uniqueId}'

module keyVault 'modules/secrets/keyvault.bicep' = {
  name: 'keyVaultDeployment'
  params: {
    vaultName: 'kv-${uniqueId}'
    location: location
  }
}

module entraApp 'modules/identity/entra-app.bicep' = {
  name: 'entraAppWeb'
  params: {
    applicationName: 'web-${uniqueId}'
  }
}


module apiService 'modules/compute/appservice.bicep' = {
  name: 'apiDeployment'
  params: {
    appName: 'api-${uniqueId}'
    appServicePlanName: 'plan-api-${uniqueId}'
    keyVaultName: keyVaultName
    appSettings: [
      {
        name: 'DatabaseName'
        value: 'urls'
      }
      {
        name: 'ContainerName'
        value: 'items'
      }
      {
        name: 'TokenRangeService__Endpoint'
        value: tokenRangesService.outputs.url
      }
      {
        name: 'AzureAd__Instance'
        value: environment().authentication.loginEndpoint
      }
      {
        name: 'AzureAd__TenantId'
        value: tenant().tenantId
      }
      {
        name: 'AzureAd__ClientId'
        value: entraApp.outputs.applicationId
      }
      {
        name: 'AzureAd__Scopes'
        value: 'URLs.Read'
      }
    ]
    location: location
  }
}

module tokenRangesService 'modules/compute/appservice.bicep' = {
  name: 'tokenRangesServiceDeployment'
  params: {
    appName: 'token-ranges-service-${uniqueId}'
    appServicePlanName: 'plan-token-ranges-${uniqueId}'
    keyVaultName: keyVaultName
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
    keyVaultName: keyVaultName
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
    keyVaultName: keyVaultName
  }
}

module keyVaultRoleAssingment 'modules/secrets/keyvault-role-assignment.bicep' = {
  name: 'keyVaultRoleAssignmentDeployment'
  params: {
    keyVaultName: keyVault.outputs.name
    pricipalIds: [
      apiService.outputs.principalId
      tokenRangesService.outputs.principalId
    ]
  }
}
