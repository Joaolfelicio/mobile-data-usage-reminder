# MobileDataUsageReminder
[![Build and Deploy Prod](https://github.com/Joaolfelicio/mobile-data-usage-reminder/actions/workflows/build-deploy.yaml/badge.svg?branch=main)](https://github.com/Joaolfelicio/mobile-data-usage-reminder/actions/workflows/build-deploy.yaml)
[![MIT License][license-shield]][license-url]
[![CodeFactor](https://www.codefactor.io/repository/github/joaolfelicio/mobile-data-usage-reminder/badge)](https://www.codefactor.io/repository/github/joaolfelicio/mobile-data-usage-reminder)

## About The Project

We are used to receive a mobile message (sent by our ISP provider) when our mobile data is getting close to our monthly limit, so we avoid going over it and paying an exorbitant amount for an extra GB.

Unfortunately, seems like the European law that was enforcing this, got drawn back.

That is why MobileDataUsageReminder *was born*.

### Built With
* [.NET 6](https://dotnet.microsoft.com/)

## Getting Started

### Prerequisites

#### .NET 6

You need to have .NET 6 SDK version installed.

You can check if you already have it on your machine by running the following command.

```sh
dotnet --version --list-sdks
```

In the output, you should have something like "`6.xxx`", if not, you will need to install it.


### Installation

#### Telegram Bot

The application is using the telegram API to deliver the reminders.

You can read this [medium article](https://medium.com/@wk0/send-and-receive-messages-with-the-telegram-api-17de9102ab78) on how to:
- **Create your telegram bot** (It's the bot that is going to send the reminders).
- Get your **API Key** (You will need when calling the API to send the reminders).
- The **Chat Id(s)** (The Id(s) of the user(s) to send the reminder to).

You need to store the API Key and Chat Id(s), as you will need them to reference it in the [local.settings.json](#appsettingsjson).

#### Local.Settings.json

##### ApplicationConfiguration

In the `ApplicationConfiguration` section, you will need to add the values for the provider email and provider password. **The password will ONLY be used to login into the provider, it won't be saved or used anywhere else.**

```json
{
  "ApplicationConfiguration": {
    "ProviderEmail": "testEmail@test.com",
    "ProviderPassword": "testPassword"
  }
}
```

##### TelegramApiConfiguration

In the `TelegramApiConfiguration`, you need to configure multiple fields for the Telegram Api.

`TelegramUsers` should contain a *array* of users (or a single user) that we should get the reminder for.

- PhoneNumber: The phone number that is going to be used to get the current data usage (it should include the indicative, for example, 352 for Luxembourg).
- ChatId: The telegram chat id of the user that should get the reminder for this phone number (If you don't have this id, check the [previous section](#telegram-bot)).

`AccessToken` is the Api Key of your telegram bot (If you don't have this api key, check the [previous section](#telegram-bot)).

```json
{
  "TelegramApiConfiguration": {
    "TelegramUsers": [
      {
        "PhoneNumber": "352123456789",
        "ChatId": "1231231230"
      },
      {
        "PhoneNumber": "352987654321",
        "ChatId": "9879879871"
      }
    ],
    "ApiEndPoint": "https://api.telegram.org/bot",
    "AccessToken": "ApiKey"
  }
}
```

## Logical steps of the application

The logical steps of the application are the following:

1. Get the mobile data usage by calling the provider's API.
2. Filter the received mobile data usage by comparing with the reminders sent stored in the database for this month, keep the mobile data usage that hasn't sent in the reminder.
3. **If** after filtering we have any record, means that the data usage percentage has increased since we sent the last reminder and we will need to send a new reminder, **if not**, the app will exit.
4. Send the notification via the notification's API.
5. Update the database by adding a new entry for the reminder sent.

## ISP Providers Supported

- Orange Luxembourg

You can contribute to this list, check the [contribution section](#adding-new-isp-providers-support).

## Contributing

<!-- TODO Add providers and notifications gateway -->

Contributions are what make the open source community such an amazing place to be learn, inspire, and create. Any contributions you make are **greatly appreciated**.

### Adding a new ISP Provider support

You can increase the list of the supported ISP Providers by implementing the code for the said providers.

You can do this by:

1. Go to `src/MobileDataUsageReminder.Core/Infrastructure` and implement the  class for the new provider that you wish to add, it should derivate from the `IDataProviderGateway` contract.
2. The class for the new provider, because it derive from the `IDataProviderGateway`, needs to implement the `GetDataUsages` method.
3. After creating the new class provider and implementing the methods, you will need to swap the implementation type of the `IDataProviderGateway` when using the dependency injection in the `FunctionStartup.cs`.
```csharp
.AddScoped<IDataProviderGateway, TheNewClassProviderGateway>()
```
4. Now when you run the application, it will fetch all the provider data from the provider gateway that you just added.

### How to contribute

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/feature-name`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/feature-name`)
5. Open a Pull Request

## License

Distributed under the MIT License. See `LICENSE` for more information.


<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[license-shield]: https://img.shields.io/github/license/othneildrew/Best-README-Template.svg?style=flat-square
[license-url]: https://github.com/Joaolfelicio/mobile-data-usage-reminder/blob/master/LICENSE
