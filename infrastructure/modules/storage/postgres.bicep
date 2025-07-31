param name string
param location string
param administratorLogin string
@secure()
param administratorLoginPassword string
param keyVaultName string

resource postgresqlServer 'Microsoft.DBforPostgreSQL/flexibleServers@2024-08-01' = {
  name: name
  location: location
  sku: {
    name: 'Standard_B1ms'
    tier: 'Burstable'
  }
  properties: {
    version: '16'
    storage: {
      storageSizeGB: 32
    }
    backup: {
      backupRetentionDays: 7
      geoRedundantBackup: 'Disabled'
    }
    administratorLogin: administratorLogin
    administratorLoginPassword: administratorLoginPassword
  }

  resource database 'databases' = {
    name: 'ranges'
  }

  resource firewallAzure 'firewallRules' = {
    name: 'allow-all-azure-internal-IPs'
    properties: {
      startIpAddress: '0.0.0.0'
      endIpAddress: '0.0.0.0'
    }
  }
}

resource keyVault 'Microsoft.KeyVault/vaults@2024-11-01' existing = {
  name: keyVaultName
}

resource postgresDbConnectionString 'Microsoft.KeyVault/vaults/secrets@2024-11-01' = {
  parent: keyVault
  name: 'Postgres--ConnectionString'
properties: {
    value: 'Host=${postgresqlServer.properties.fullyQualifiedDomainName};Database=ranges;Port=5432;User Id=${administratorLogin};Password=${administratorLoginPassword}'
    contentType: 'application/x-sql'
  }
}

output serverId string = postgresqlServer.id
