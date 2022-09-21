param databaseOptions object = {}
param redundancyOptions object = {}

param location string = resourceGroup().location

module SQLServer 'modules/databaseSQLServer.bicep' = if (!empty(databaseOptions)) {
  name: 'LinkGeek-SQLServer-${location}'
  params: {
    location: location
    databaseOptions: databaseOptions
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
