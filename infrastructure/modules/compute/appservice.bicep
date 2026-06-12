param appServicePlanName string
param appName string
param keyVaultName string
param appSettings array = []
param location string = resourceGroup().location

resource appServicePlan 'Microsoft.Web/serverfarms@2025-03-01' = {
  kind: 'linux'
  location: location
  name: appServicePlanName
  properties: {
    reserved: true
  }
  sku: {
    name: 'B1'
  }
}

resource webApp 'Microsoft.Web/sites@2025-03-01' = {
  name: appName
  location: location
  properties: {
    serverFarmId: appServicePlan.id
    httpsOnly: true
    siteConfig: {
      linuxFxVersion: 'DOTNETCORE|10.0'
      appSettings: concat(
        [
          {
            name: 'KeyVaultName'
            value: keyVaultName
          }
        ],
        appSettings
      ) 
    }
  }
  identity: {
    type: 'SystemAssigned'
  }
}

resource webAppConfig 'Microsoft.Web/sites/config@2025-03-01' = {
  parent: webApp
  name: 'web'
  properties: {
    scmType: 'GitHub'
  }
}

resource scmPublishingCredentials 'Microsoft.Web/sites/basicPublishingCredentialsPolicies@2025-03-01' = {
  name: 'scm'
  parent: webApp
  properties: {
    allow: true
  }
}

output appServiceId string = webApp.id
output principalId string = webApp.identity.principalId
output url string = 'https://${webApp.properties.defaultHostName}'
