# MobileDataUsageReminder

<!-- PROJECT SHIELDS -->
<!--
*** I'm using markdown "reference style" links for readability.
*** Reference links are enclosed in brackets [ ] instead of parentheses ( ).
*** See the bottom of this document for the declaration of the reference variables
*** for contributors-url, forks-url, etc. This is an optional, concise syntax you may use.
*** https://www.markdownguide.org/basic-syntax/#reference-style-links
-->
[![MIT License][license-shield]][license-url]

<!-- TABLE OF CONTENTS -->
## Table of Contents

- [MobileDataUsageReminder](#mobiledatausagereminder)
  - [Table of Contents](#table-of-contents)
  - [About The Project](#about-the-project)
    - [Logical Steps](#logical-steps)
    - [ISP Providers Supported:](#isp-providers-supported)
    - [Built With](#built-with)
- [TODO BELOW HERE](#todo-below-here)
  - [Getting Started](#getting-started)
    - [Prerequisites](#prerequisites)
    - [Installation](#installation)
  - [Usage](#usage)
  - [Contributing](#contributing)
  - [License](#license)
  - [Contact](#contact)
  - [Acknowledgements](#acknowledgements)



<!-- ABOUT THE PROJECT -->
## About The Project

We are used to receive an sms (sent by our ISP provider) when our mobile data is getting close to our monthly limit, so we avoid going over it and paying an exorbitant amount for an extra GB.

Unfortunately, seems like the European law that was enforcing this, got drawn back.

That is why I created MobileDataUsageReminder.

In it's Core, it's a Job Scheduler, that will start at `6AM` and run each `30min` and will send you a reminder if your data usage percentage has reached a new level (each 10% from 0% to 100%).

### Logical Steps

The logical steps of the application are the following:

1. Get the mobile data usage by calling the provider's API.
2. Filter the received mobile data usage by comparing with what we have in the database, we only keep the data usage that for the current month and for the mobile phone number, has a different data used percentage from what we found on the DB (so we know that we haven't sent a reminder for that %). 
3. **If** after filtering we have any record, means that the data usage percentage has increased since we sent the last reminder and we will need to send the new reminder, **if not**, the job will exit.
4. Update the database by adding a new entry for the corresponding month, phone number and the current data used percentage.
5. Sill send the notification via telegram's API.

### ISP Providers Supported:
<!-- TODO -->

### Built With
This section should list any major frameworks that you built your project using. Leave any add-ons/plugins for the acknowledgements section. Here are a few examples.
* [.NET Core](https://dotnet.microsoft.com/)
* [PostgreSQL](https://www.postgresql.org/)
* [Quartz.NET](https://www.quartz-scheduler.net/)

<!-- TODO BELOW HERE-->
# TODO BELOW HERE

<!-- GETTING STARTED -->
## Getting Started

This is an example of how you may give instructions on setting up your project locally.
To get a local copy up and running follow these simple example steps.

### Prerequisites


1. .NET Core
2. Pgsql

This is an example of how to list things you need to use the software and how to install them.
* npm
```sh
npm install npm@latest -g
```

### Installation

1. Migrations
2. Telegram bot
3. Appsettings.json

qwerty

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



<!-- USAGE EXAMPLES -->
## Usage

Use this space to show useful examples of how a project can be used. Additional screenshots, code examples and demos work well in this space. You may also link to more resources.

_For more examples, please refer to the [Documentation](https://example.com)_


<!-- CONTRIBUTING -->
## Contributing

<!-- TODO Add providers -->

Contributions are what make the open source community such an amazing place to be learn, inspire, and create. Any contributions you make are **greatly appreciated**.

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request



<!-- LICENSE -->
## License

Distributed under the MIT License. See `LICENSE` for more information.



<!-- CONTACT -->
## Contact

Your Name - [@your_twitter](https://twitter.com/your_username) - email@example.com

Project Link: [https://github.com/your_username/repo_name](https://github.com/your_username/repo_name)



<!-- ACKNOWLEDGEMENTS -->
## Acknowledgements
* [GitHub Emoji Cheat Sheet](https://www.webpagefx.com/tools/emoji-cheat-sheet)
* [Img Shields](https://shields.io)
* [Choose an Open Source License](https://choosealicense.com)
* [GitHub Pages](https://pages.github.com)
* [Animate.css](https://daneden.github.io/animate.css)
* [Loaders.css](https://connoratherton.com/loaders)
* [Slick Carousel](https://kenwheeler.github.io/slick)
* [Smooth Scroll](https://github.com/cferdinandi/smooth-scroll)
* [Sticky Kit](http://leafo.net/sticky-kit)
* [JVectorMap](http://jvectormap.com)
* [Font Awesome](https://fontawesome.com)





<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[license-shield]: https://img.shields.io/github/license/othneildrew/Best-README-Template.svg?style=flat-square
[license-url]: https://github.com/Joaolfelicio/mobile-data-usage-reminder/blob/master/LICENSE
