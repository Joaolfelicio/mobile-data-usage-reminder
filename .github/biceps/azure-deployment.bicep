@description('Specifies region of all resources.')
param location string = resourceGroup().location

@description('Suffix for function app, storage account, and app insights.')
param appNameSuffix string

@description('The email used to connect to the provider.')
param providerEmail string

@description('The password used to connect to the provider.')
param providerPassword string

param telegramUsers array = [
  {
    phoneNumber: '123'
    chatId: '123'
  }
  {
    phoneNumber: '123'
    chatId: '123'
  }
]

@description('The access token to manipulate the telegram api.')
param telegramAccessToken string

@description('The crono expression of the function\'s timer.')
param cronoTimerSchedule string

var functionAppName = 'fn-${appNameSuffix}'
var appInsightsName = 'ai-${appNameSuffix}'
var appServicePlanName = 'pn-${appNameSuffix}'
var storageAccountName = 'sa-${replace(appNameSuffix, '-', '')}'
var cosmosDbName = 'cdb-${appNameSuffix}'

resource cosmosDb 'Microsoft.DocumentDB/databaseAccounts@2021-04-15' = {
  name: cosmosDbName
  location: location
  kind: 'GlobalDocumentDB'
  properties: {
    enableFreeTier: true
    databaseAccountOfferType: 'Standard'
    consistencyPolicy: {
      defaultConsistencyLevel: 'Session'
    }
    locations: [
      {
        locationName: location
        failoverPriority: 0
        isZoneRedundant: false
      }
    ]
    capabilities: [
      {
        name: 'EnableServerless'
      }
    ]
  }
}

resource storageAccount 'Microsoft.Storage/storageAccounts@2021-04-01' = {
  name: storageAccountName
  location: location
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'StorageV2'
}

resource appInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: appInsightsName
  location: location
  kind: 'web'
  properties: {
    Application_Type: 'web'
    publicNetworkAccessForIngestion: 'Enabled'
    publicNetworkAccessForQuery: 'Enabled'
  }
}

resource plan 'Microsoft.Web/serverfarms@2020-12-01' = {
  name: appServicePlanName
  location: location
  kind: 'functionapp'
  sku: {
    name: 'Y1'
  }
  properties: {}
}

resource functionApp 'Microsoft.Web/sites@2020-12-01' = {
  name: functionAppName
  location: location
  kind: 'functionapp'
  properties: {
    serverFarmId: plan.id
    siteConfig: {
      appSettings: [
        {
          name: 'AzureWebJobsStorage'
          value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccount.name};EndpointSuffix=${environment().suffixes.storage};AccountKey=${listKeys(storageAccount.id, storageAccount.apiVersion).keys[0].value}'
        }
        {
          name: 'WEBSITE_CONTENTAZUREFILECONNECTIONSTRING'
          value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccount.name};EndpointSuffix=${environment().suffixes.storage};AccountKey=${listKeys(storageAccount.id, storageAccount.apiVersion).keys[0].value}'
        }
        {
          name: 'MongoConfiguration:ConnectionString'
          value: 'AccountEndpoint=https://${cosmosDb.name}.documents.azure.com:443/‌​;AccountKey=${listKeys(cosmosDb.id, cosmosDb.apiVersion).keys[0].value}'
        }
        {
          name: 'MongoConfiguration:DatabaseName'
          value: 'MobileDataUsageReminder'
        }
        {
          name: 'MongoConfiguration:CollectionName'
          value: 'MobileDataReminders'
        }
        {
          name: 'ApplicationConfiguration:ProviderEmail'
          value: providerEmail
        }
        {
          name: 'ApplicationConfiguration:ProviderPassword'
          value: providerPassword
        }
//TODO: Make this dynamic
        {
          name: 'TelegramApiConfiguration:TelegramUsers:0:PhoneNumber'
          value: telegramUsers[0].phoneNumber
        }
        {
          name: 'TelegramApiConfiguration:TelegramUsers:0:ChatId'
          value: telegramUsers[0].chatId
        }
        {
          name: 'TelegramApiConfiguration:TelegramUsers:1:PhoneNumber'
          value: telegramUsers[1].phoneNumber
        }
        {
          name: 'TelegramApiConfiguration:TelegramUsers:1:ChatId'
          value: telegramUsers[1].chatId
        }
        {
          name: 'TelegramApiConfiguration:AccessToken'
          value: telegramAccessToken
        }
        {
          name: 'TimerSchedule'
          value: cronoTimerSchedule
        }
        {
          name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
          value: appInsights.properties.InstrumentationKey
        }
        {
          name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
          value: 'InstrumentationKey=${appInsights.properties.InstrumentationKey}'
        }
        {
          name: 'FUNCTIONS_WORKER_RUNTIME'
          value: 'dotnet'
        }
        {
          name: 'FUNCTIONS_EXTENSION_VERSION'
          value: '~4'
        }
      ]
    }
    httpsOnly: true
  }
}

output functionAppName string = functionAppName
