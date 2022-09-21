@description('Location for the SQL server account.')
param location string = resourceGroup().location
param databaseOptions object

var accountName = databaseOptions.databaseNameSQLServer

resource sqlServer 'Microsoft.Sql/servers@2022-02-01-preview' = {
  name: toLower('${accountName}-${location}')
  location: location
}

resource sqlDB 'Microsoft.Sql/servers/databases@2021-08-01-preview' = {
  parent: sqlServer
  name: 'LinkGeekSQLdatabase'
  location: location
  sku: {
    name: 'Basic'
    tier: 'Basic'
  }
}
