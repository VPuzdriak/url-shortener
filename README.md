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