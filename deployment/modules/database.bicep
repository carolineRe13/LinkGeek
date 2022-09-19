@description('Location for the Cosmos DB account.')
param location string = resourceGroup().location
param databaseOptions object
param redundancyOptions object = {}

var accountName = databaseOptions.databaseName

var locations = [for (location, i) in redundancyOptions.locations:{
  locationName: location
  failoverPriority: i
  isZoneRedundant: false
}]

resource databaseAccount 'Microsoft.DocumentDB/databaseAccounts@2022-05-15' = {
  name: toLower('${accountName}-${location}')
  kind: 'GlobalDocumentDB'
  location: location
  properties: {
    consistencyPolicy: {
      defaultConsistencyLevel: 'Session'
    }
    locations: locations
    databaseAccountOfferType: 'Standard'
    enableAutomaticFailover: false
    enableMultipleWriteLocations: true
  }
}

resource database 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases@2022-05-15' = {
  name: '${accountName}'
  parent: databaseAccount
  properties: {
    resource: {
      id: accountName
    }
  }
}

resource hpContainer 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers@2022-05-15' = {
  name: 'LinkGeek-container'
  parent: database
  properties: {
    resource: {
      id: 'LinkGeek-container'
      partitionKey: {
        paths: [
          '/UserId'
        ]
        kind: 'Hash'
      }
    }
  }
}
