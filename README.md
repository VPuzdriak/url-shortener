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

### Apply deployment
```bash
# For the checking what is going to be deployed
az deployment group what-if --resource-group urlshortener-dev --template-file infrastructure/main.bicep

# For deployment
az deployment group create --resource-group urlshortener-dev --template-file infrastructure/main.bicep
```