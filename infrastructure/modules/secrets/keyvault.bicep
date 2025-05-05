param location string
param vaultName string

resource keyVault 'Microsoft.KeyVault/vaults@2024-11-01' = {
  name: vaultName
  location: location
  properties: {
    sku: {
      name: 'standard'
      family: 'A'
    }
    enableRbacAuthorization: true
    tenantId: subscription().tenantId
  }
}

output id string = keyVault.id
output name string = keyVault.name
