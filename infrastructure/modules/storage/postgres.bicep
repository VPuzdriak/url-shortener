param name string
param location string
param adminstratorLogin string
@secure()
param administratorLoginPassword string
param keyVaultName string

resource postgresSqlServer 'Microsoft.DBforPostgreSQL/flexibleServers@2025-08-01' = {
  name: name
  location: location
  sku: {
    name: 'Standard_B1ms'
    tier: 'Burstable'
  }
  properties: {
    version: '18'
    storage: {
      storageSizeGB: 32
    }
    backup: {
      backupRetentionDays: 7
      geoRedundantBackup: 'Disabled'
    }
    administratorLogin: adminstratorLogin
    administratorLoginPassword: administratorLoginPassword
  }
  resource database 'databases' = {
    name: 'ranges'
  }
  resource firewallRule 'firewallRules' = {
    name: 'allow-all-azure-internal-IPs'
    properties: {
      startIpAddress: '0.0.0.0'
      endIpAddress: '0.0.0.0'
    }
  }
}

resource keyVault 'Microsoft.KeyVault/vaults@2025-05-01' existing = {
  name: keyVaultName
}

resource cosmosDbConnectionString 'Microsoft.KeyVault/vaults/secrets@2025-05-01' = {
  parent: keyVault
  name: 'Postgres--ConnectionString'
  properties: {
    value: 'Server=${postgresSqlServer.name}.postgres.database.azure.com;Database=ranges;Port=5432;User Id=${adminstratorLogin};Password=${administratorLoginPassword};'
  }
}

output serverId string = postgresSqlServer.id
