# Url Shortener

## IaC

### Log in into Azure
```bash
az login
```

### Create Resource Group
```bash
az group create --name urlshortener-dev --location westeurope
```

### Deploy
```bash
# For the checking what is going to be deployed
az deployment group what-if --resource-group urlshortener-dev --template-file infrastructure/main.bicep

# For deployment
az deployment group create --resource-group urlshortener-dev --template-file infrastructure/main.bicep
```

### Create user for GH Actions
```bash
az ad sp create-for-rbac --name "GitHub-Actions-SP" \
                         --role contributor \
                         --scopes /subscriptions/dd627ee9-2a7b-4650-9ab6-6b4436401b7f \
                         --sdk-auth
```

### Configure a federated identity credential on an app
Configure federated credentials using entra.microsoft.com
Select app registration -> Certificates & secrets -> Federated credential