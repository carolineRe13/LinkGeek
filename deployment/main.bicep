param databaseOptions object = {}
param redundancyOptions object = {}

param location string = resourceGroup().location

var projectName = 'LinkGeek'

resource kv 'Microsoft.KeyVault/vaults@2022-07-01' existing = {
  name: '${projectName}SqlServer'
  scope: resourceGroup()
}

module SQLServer 'modules/databaseSQLServer.bicep' = if (!empty(databaseOptions)) {
  name: 'LinkGeek-SQLServer-${location}'
  params: {
    location: location
    databaseOptions: databaseOptions
    administratorLogin: 'caroline.reinig'
    administratorLoginPassword: kv.getSecret('sqlServer-pwd')
  }
}

module cosmosDB 'modules/databaseCosmos.bicep' = if (!empty(databaseOptions)) {
  name: 'LinkGeek-CosmosDB-${location}'
  params: {
    location: location
    databaseOptions: databaseOptions
    redundancyOptions: redundancyOptions
  }
}

module webApp 'modules/appService.bicep' = {
  name: 'LinkGeek-app'
  params: {
    location: location
    webAppName: 'LinkGeek'
  }
}
