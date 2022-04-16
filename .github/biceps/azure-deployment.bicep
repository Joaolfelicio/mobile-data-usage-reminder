param location string = resourceGroup().location
param appNameSuffix string

param providerEmail string
param providerPassword string

param telegramUserOnePhoneNumber string
param telegramUserOneChatId string
param telegramUserTwoPhoneNumber string
param telegramUserTwoChatId string
param telegramAccessToken string
param telegramApiEndpoint string
param cronoTimerSchedule string

var functionAppName = 'fn-${appNameSuffix}'
var appInsightsName = 'ai-${appNameSuffix}'
var appServicePlanName = 'pn-${appNameSuffix}'
var storageAccountName = 'sa${replace(appNameSuffix, '-', '')}'
var cosmosDbName = 'cdb-${appNameSuffix}'

resource cosmosDb 'Microsoft.DocumentDB/databaseAccounts@2021-04-15' = {
  name: cosmosDbName
  location: location
  kind: 'MongoDB'
  properties: {
    apiProperties: {
      serverVersion: '4.0'
    }
    enableFreeTier: true
    databaseAccountOfferType: 'Standard'
    consistencyPolicy: {
      defaultConsistencyLevel: 'Eventual'
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
          value: first(listConnectionStrings(cosmosDb.id, cosmosDb.apiVersion).connectionStrings).connectionString
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
        {
          name: 'TelegramApiConfiguration:TelegramUsers:0:PhoneNumber'
          value: telegramUserOnePhoneNumber
        }
        {
          name: 'TelegramApiConfiguration:TelegramUsers:0:ChatId'
          value: telegramUserOneChatId
        }
        {
          name: 'TelegramApiConfiguration:TelegramUsers:1:PhoneNumber'
          value: telegramUserTwoPhoneNumber
        }
        {
          name: 'TelegramApiConfiguration:TelegramUsers:1:ChatId'
          value: telegramUserTwoChatId
        }
        {
          name: 'TelegramApiConfiguration:ApiEndPoint'
          value: telegramApiEndpoint
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

output functionBaseUrl string = 'https://${functionAppName}.azurewebsites.net/api'
output hostKey string = listkeys('${functionApp.id}/host/default', '2016-08-01').functionKeys.default
