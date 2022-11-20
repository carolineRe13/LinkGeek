param databaseOptions object = {}

param location string = resourceGroup().location
param appServicePlanName string = toLower('AppServicePlan-Linkgeek-${location}')
param webSiteName string = toLower('wapp-Linkgeek-${location}')
param sku string = 'F1'

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

module webApp 'modules/appService.bicep' = {
  name: 'LinkGeek-app'
  params: {
    location: location
    appServicePlanName: appServicePlanName
    webSiteName: webSiteName
    sku: sku
  }
}
