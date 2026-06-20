extension microsoftGraph

param applicationName string
@allowed([
  'AzureADMyOrg'
  'AzureADMultipleOrgs'
  'AzureADandPersonalMicrosoftAccount'
])
param signInAudience string = 'AzureADandPersonalMicrosoftAccount'

resource application 'Microsoft.Graph/applications@v1.0' = {
  displayName: applicationName
  uniqueName: applicationName
  signInAudience: signInAudience
}

resource updateApplicationWithSettings 'Microsoft.Graph/applications@v1.0' = {
  displayName: applicationName
  uniqueName: applicationName
  signInAudience: signInAudience
  api: {
    oauth2PermissionScopes: [
      {
        id: 'c3decca8-7dbd-42cd-ba5e-dacdf7705138'
        isEnabled: true
        value: 'Urls.Read'
        type: 'User'
        adminConsentDescription: 'URLs Read'
        adminConsentDisplayName: 'URLs Read'
        userConsentDescription: null
        userConsentDisplayName: 'Read Access to URLs'
      }
    ]
  }
  identifierUris: [
    'api://${application.appId}'
  ]
}

output applicationId string = application.appId
