# url-shortener
Let's build URL shortener

## Infrastructure as Code

### Log in into Azure
```bash
az login
```

### Create Resource Group
```bash
az group create --name urlshortener-dev --location westeurope
```

### Provision resources on Azure
```bash
az deployment group what-if --resource-group urlshortener-dev --template-file infrastructure/main.bicep
az deployment group create --resource-group urlshortener-dev --template-file infrastructure/main.bicep
```

### Create user for GH Actions
```bash
az ad sp create-for-rbac --name "GitHub-Actions-SP" --role contributor --scopes /subscriptions/b0b3b8f4-da3c-4057-b14b-ae9b208a344b --sdk-auth
```

#### Configure a federated identity credential on an app
