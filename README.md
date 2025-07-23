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