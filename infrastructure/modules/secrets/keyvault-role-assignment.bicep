param keyVaultName string
param principalIds array
param principalType string = 'ServicePrincipal'
param roleDefinitionId string = 'acdd72a7-3385-48ef-bd42-f606fba81ae7'

resource keyVault 'Microsoft.KeyVault/vaults@2023-07-01' existing = {
  name: keyVaultName
}

resource keyVaultRoleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = [for princinpalId in principalIds: {
  name: guid(keyVault.id, princinpalId, roleDefinitionId)
  scope: keyVault
  properties: {
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', roleDefinitionId)
    principalId: princinpalId
    principalType: principalType
  }
}]
