# URL Shortener

## IAC

### Download Azure CLI
https://learn.microsoft.com/en-us/cli/azure

### Log in into Azure
```bash
az login
```

### Create resource group
```bash
az group create --name urlshortener-dev --location westeurope
```

### Check deployment group
```bash
az deployment group what-if --resource-group urlshortener-dev --template-file infrastructure/main.bicep
```

### Create deployment group
```bash
az deployment group create --resource-group urlshortener-dev --template-file infrastructure/main.bicep
```

### Create user for GH Actions
```bash
az ad sp create-for-rbac `
  --name "GitHub-Actions-SP" `
  --role contributor `
  --scopes /subscriptions/b0b3b8f4-da3c-4057-b14b-ae9b208a344b `
  --sdk-auth
```

### Create custom infra_deploy role
* Create a role based on Contributor
* Remove Write and Delete from No Action in RoleAssignment policy
* Add Write and Delete from Action in RoleAssignment policy

### Apply to Custom Role
```bash
az ad sp create-for-rbac `
  --name "GitHub-Actions-SP" `
  --role infra_deploy `
  --scopes /subscriptions/b0b3b8f4-da3c-4057-b14b-ae9b208a344b `
  --sdk-auth
```

#### Configure a federated identity credential on an app

### Get publish profile
```bash
az webapp deployment list-publishing-profiles --name api-wscvzm7ks5zzm --resource-group urlshortener-dev --xml
```