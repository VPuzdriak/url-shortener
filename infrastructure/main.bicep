param location string = resourceGroup().location

var uniqueId = uniqueString(resourceGroup().id)

module apiService 'modules/compute/appservice.bicep'= {
  name: 'apiDeployment'
  params: {
    appServicePlanName: 'plan-api-${uniqueId}'
    appName: 'api-${uniqueId}'
    location: location
  }
}
