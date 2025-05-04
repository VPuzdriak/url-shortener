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

### Create app service
```bash
az deployment group create --resource-group urlshortener-dev --template-file infrastructure/main.bicep
```