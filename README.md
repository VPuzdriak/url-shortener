# Url Shortener

## Infrastructure as Code

### Download Azure CLI
https://learn.microsoft.com/en-us/cli/azure

### Log in into Azure
```bash
az login
```

### Create Resource Group
```bash
az group create --name urlshortener-dev --location westeurope
```

### What-if infrastructure
```bash
az deployment group what-if --resource-group urlshortener-dev --template-file infrastructure/main.bicep
```

### Create infrastructure
```bash
az deployment group create --resource-group urlshortener-dev --template-file infrastructure/main.bicep
```

### Create User for GH Actions
```bash
az ad sp create-for-rbac \
    --name "GitHub-Actions-SP" \
    --role contributor \
    --scopes /subscriptions/dd627ee9-2a7b-4650-9ab6-6b4436401b7f \
    --sdk-auth
```

### Apply to Custom contributor role
```bash
az ad sp create-for-rbac \
    --name "GitHub-Actions-SP" \
    --role 'infra_deploy' \
    --scopes /subscriptions/dd627ee9-2a7b-4650-9ab6-6b4436401b7f \
    --sdk-auth
```

#### Configure a federated identity credential on an app
Navigate to Microsoft Entra admin center at https://entra.microsoft.com/
Find App registration for GitHub-Actions-SP and add Federated Credential "GitHub Actions deploying Azure resources"