# MobileDataUsageReminder

[![MIT License][license-shield]][license-url]

## Table of Contents

- [MobileDataUsageReminder](#mobiledatausagereminder)
  - [Table of Contents](#table-of-contents)
  - [About The Project](#about-the-project)
    - [Logical Steps](#logical-steps)
    - [ISP Providers Supported:](#isp-providers-supported)
    - [Built With](#built-with)
  - [Getting Started](#getting-started)
    - [Prerequisites](#prerequisites)
      - [.NET Core 3.1](#net-core-31)
      - [PostgreSQL](#postgresql)
    - [Installation](#installation)
      - [EntityFrameworkCore Migrations](#entityframeworkcore-migrations)
      - [Telegram Bot](#telegram-bot)
      - [Appsettings.json](#appsettingsjson)
  - [Usage](#usage)
    - [Run dotnet core and pgsql](#run-dotnet-core-and-pgsql)
      - [Run PostgreSQL](#run-postgresql)
  - [Contributing](#contributing)
  - [License](#license)

## About The Project

We are used to receive an sms (sent by our ISP provider) when our mobile data is getting close to our monthly limit, so we avoid going over it and paying an exorbitant amount for an extra GB.

Unfortunately, seems like the European law that was enforcing this, got drawn back.

That is why MobileDataUsageReminder *was born*.

In it's Core, it's a Job Scheduler, that will start at `6AM` and run each `30min` and will send you a reminder if your data usage percentage has reached a new level (each 10%, from 10% to 100%).

### Logical Steps

The logical steps of the application are the following:

1. Get the mobile data usage by calling the provider's API.
2. Filter the received mobile data usage by comparing with the reminders sent stored in the database for this month, keep the mobile data usage that hasn't sent in the reminder.
4. **If** after filtering we have any record, means that the data usage percentage has increased since we sent the last reminder and we will need to send a new reminder, **if not**, the job will exit.
5. Send the notification via the notification's API.
6. Update the database by adding a new entry for the reminder sent.

### ISP Providers Supported:

- Orange Luxembourg

You can contribute to this list, check the [contribution section](#contributing).

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
- Create your telegram bot (It's the bot that is going to send the reminders)
- Get your API Key (You will need when calling the API to send the reminders)
- The Chat Id (The Id of the user to send the reminder to)

You need to store the API Key and Chat Id, as he will need to reference it in the [appsettings.json](#appsettingsjson).

#### Appsettings.json

1. Get a free API Key at [https://example.com](https://example.com)
2. Clone the repo
```sh
git clone https://github.com/your_username_/Project-Name.git
```
3. Install NPM packages
```sh
npm install
```
4. Enter your API in `config.js`
```JS
const API_KEY = 'ENTER YOUR API';
```

## Usage

Use this space to show useful examples of how a project can be used. Additional screenshots, code examples and demos work well in this space. You may also link to more resources.

_For more examples, please refer to the [Documentation](https://example.com)

### Run dotnet core and pgsql

#### Run PostgreSQL

https://tableplus.com/blog/2018/10/how-to-start-stop-restart-postgresql-server.html

## Contributing

<!-- TODO Add providers and notifications gateway -->

Contributions are what make the open source community such an amazing place to be learn, inspire, and create. Any contributions you make are **greatly appreciated**.

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License

Distributed under the MIT License. See `LICENSE` for more information.


<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[license-shield]: https://img.shields.io/github/license/othneildrew/Best-README-Template.svg?style=flat-square
[license-url]: https://github.com/Joaolfelicio/mobile-data-usage-reminder/blob/master/LICENSE
