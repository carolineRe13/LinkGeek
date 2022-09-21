@description('Location for the SQL server account.')
param location string = resourceGroup().location
param databaseOptions object

@description('The administrator username of the SQL logical server.')
param administratorLogin string

@description('The administrator password of the SQL logical server.')
@secure()
param administratorLoginPassword string

var accountName = databaseOptions.databaseNameSQLServer

resource sqlServer 'Microsoft.Sql/servers@2022-02-01-preview' = {
  name: toLower('${accountName}-${location}')
  location: location
  properties: {
    administratorLogin: administratorLogin
    administratorLoginPassword: administratorLoginPassword
  }
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
