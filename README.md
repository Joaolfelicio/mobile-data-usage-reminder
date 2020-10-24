# MobileDataUsageReminder

[![MIT License][license-shield]][license-url]

## Table of Contents

- [MobileDataUsageReminder](#mobiledatausagereminder)
  - [Table of Contents](#table-of-contents)
  - [About The Project](#about-the-project)
    - [Built With](#built-with)
  - [Getting Started](#getting-started)
    - [Prerequisites](#prerequisites)
      - [.NET Core 3.1](#net-core-31)
      - [PostgreSQL](#postgresql)
    - [Installation](#installation)
      - [EntityFrameworkCore Migrations](#entityframeworkcore-migrations)
      - [Telegram Bot](#telegram-bot)
      - [AppSettings.json](#appsettingsjson)
        - [ApplicationConfiguration](#applicationconfiguration)
        - [TelegramApiConfiguration](#telegramapiconfiguration)
  - [Usage](#usage)
    - [Run PostgreSQL](#run-postgresql)
    - [Run .NET Core](#run-net-core)
  - [Logical steps of the application](#logical-steps-of-the-application)
  - [ISP Providers Supported](#isp-providers-supported)
  - [Contributing](#contributing)
    - [Adding a new ISP Provider support](#adding-a-new-isp-provider-support)
    - [How to contribute](#how-to-contribute)
  - [License](#license)

## About The Project

We are used to receive an sms (sent by our ISP provider) when our mobile data is getting close to our monthly limit, so we avoid going over it and paying an exorbitant amount for an extra GB.

Unfortunately, seems like the European law that was enforcing this, got drawn back.

That is why MobileDataUsageReminder *was born*.

In it's Core, it's a Job Scheduler, that will start at `6AM` and run each `30min` and will send you a reminder if your data usage percentage has reached a new level (each 10%, from 10% to 100%).

### Built With
* [.NET Core](https://dotnet.microsoft.com/)
* [PostgreSQL](https://www.postgresql.org/)
* [Quartz.NET](https://www.quartz-scheduler.net/)

## Getting Started

### Prerequisites

#### .NET Core 3.1

You need to have .NET Core 3.1 SDK version installed.

You can check if you already have it on your machine by running the following command.

```sh
dotnet --version --list-sdks
```

In the output, you should have something like "`3.1.xxx`", if not, you will need to install it.

You can find the download page [here](https://dotnet.microsoft.com/download/dotnet-core/3.1).

#### PostgreSQL

You need to have PostgreSQL installed.

You can check if you already have it on your machine by running the following command.

```sh
psql --version
```

In the output, you should be able to see your pgSQL version, if not, you will need to install it.

You can find the download page [here](https://www.postgresql.org/download/).

### Installation

#### EntityFrameworkCore Migrations

Don't forget that for this step, you need to run your database, you can check how to do it [here](#run-postgresql).

Go to the solution level and run the following command to migrate the database.

```sh
dotnet ef database update -s .\MobileDataUsageReminder\MobileDataUsageReminder.csproj -p .\MobileDataUsageReminder.Dal\MobileDataUsageReminder.DAL.csproj
```

Entity Framework Core should have created now the database and the required objects.

#### Telegram Bot

The application is using the telegram API to deliver the reminders.

You can read this [medium article](https://medium.com/@wk0/send-and-receive-messages-with-the-telegram-api-17de9102ab78) on how to:
- **Create your telegram bot** (It's the bot that is going to send the reminders).
- Get your **API Key** (You will need when calling the API to send the reminders).
- The **Chat Id(s)** (The Id(s) of the user(s) to send the reminder to).

You need to store the API Key and Chat Id(s), as you will need them to reference it in the [appsettings.json](#appsettingsjson).

#### AppSettings.json

##### ApplicationConfiguration

In the `ApplicationConfiguration` section of the appSettings, you will need to add the values for the provider email and provider password.

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

## Usage

You will need to run postgreSQL and dotnet.

### Run PostgreSQL

You will need to start your PostgreSQL server, you can follow this [guide](https://tableplus.com/blog/2018/10/how-to-start-stop-restart-postgresql-server.html) on how to do it.

### Run .NET Core

Run the following command inside the console app folder (`MobileDataUsageReminder`), to start the application.

```sh
dotnet watch run
```

## Logical steps of the application

The logical steps of the application are the following:

1. Get the mobile data usage by calling the provider's API.
2. Filter the received mobile data usage by comparing with the reminders sent stored in the database for this month, keep the mobile data usage that hasn't sent in the reminder.
4. **If** after filtering we have any record, means that the data usage percentage has increased since we sent the last reminder and we will need to send a new reminder, **if not**, the job will exit.
5. Send the notification via the notification's API.
6. Update the database by adding a new entry for the reminder sent.

## ISP Providers Supported

- Orange Luxembourg

You can contribute to this list, check the [contribution section](#adding-new-isp-providers-support).

## Contributing

<!-- TODO Add providers and notifications gateway -->

Contributions are what make the open source community such an amazing place to be learn, inspire, and create. Any contributions you make are **greatly appreciated**.

### Adding a new ISP Provider support

You can increase the list of the supported ISP Providers by implementing the code for the said providers.

You can do this by:

1. Go to `MobileDataUsageReminder/Infrastructure` and implement the  class for the new provider that you wish to add, it should derivate from the `IProviderGateway` contract.
2. The class for the new provider, because it derive from the `IProviderGateway`, needs to implement four methods:
   1. Login method
   2. Get client information method.
   3. Get mobile data products method.
   4. Get data usage method.
3. After creating the new class provider and implementing the methods, you will need to swap the implementation type of the `IProviderGateway` when using the dependency injection in the `Program.cs`.
    ```csharp
    .AddScoped<IProviderGateway, TheNewClassProviderGateway>()
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
