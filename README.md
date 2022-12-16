<a href="https://softserve.academy/"><img src="https://raw.githubusercontent.com/ita-social-projects/StreetCode/master/StreerCodeLogo.jpg" 
  title="SoftServe IT Academy" 
  alt="SoftServe IT Academy"></a>

# Streetcode
This is a Back-end part of our Streetcode project.
Front-end part: https://github.com/ita-social-projects/StreetCode_Client
>### **Vision**
>The largest platform about the history of Ukraine, built in the space of cities.

>### **Mission**
>To fill the gaps in the historical memory of Ukrainians.

[![Build Status](https://img.shields.io/travis/ita-social-projects/StreetCode/master?style=flat-square)](https://travis-ci.org/github/ita-social-projects/StreetCode)
[![Coverage Status](https://img.shields.io/gitlab/coverage/ita-social-projects/StreetCode/master?style=flat-square)](https://coveralls.io)
[![Github Issues](https://img.shields.io/github/issues/ita-social-projects/StreetCode?style=flat-square)](https://github.com/ita-social-projects/StreetCode/issues)
[![Pending Pull-Requests](https://img.shields.io/github/issues-pr/ita-social-projects/StreetCode?style=flat-square)](https://github.com/ita-social-projects/StreetCode/pulls)


---

## Table of Contents 

- [Streetcode](#streetcode)
  - [Table of Contents](#table-of-contents)
  - [Installation](#installation)
    - [Required to install](#required-to-install)
    - [Environment](#environment)
    - [Clone](#clone)
    - [Setup](#setup)
    - [How to run local](#how-to-run-local)
    - [How to run Docker](#how-to-run-docker)
  - [Usage](#usage)
    - [How to work with swagger UI](#how-to-work-with-swagger-ui)
    - [How to run tests](#how-to-run-tests)
    - [How to Checkstyle](#how-to-checkstyle)
  - [Documentation](#documentation)
  - [Contributing](#contributing)
    - [Git flow](#git-flow)
      - [Step 1](#step-1)
      - [Step 2](#step-2)
      - [Step 3](#step-3)
    - [Issue flow](#issue-flow)
  - [Team](#team)
  - [FAQ](#faq)
  - [Support](#support)
  - [License](#license)

---

## Installation

### Required to install
* <a href="https://dotnet.microsoft.com/en-us/download/dotnet/6.0" target="_blank">ASP.NET Core Runtime 6.0.12</a>
* <a href="https://www.microsoft.com/en-us/sql-server/sql-server-downloads" target="_blank"> Microsoft SQL Server 2017</a>+

### Environment
environmental variables
```properties
spring.datasource.url=${DATASOURCE_URL}
spring.datasource.username=${DATASOURCE_USER}
spring.datasource.password=${DATASOURCE_PASSWORD}
spring.mail.username=${EMAIL_ADDRESS}
spring.mail.password=${EMAIL_PASSWORD}
cloud.name=${CLOUD_NAME}
api.key=${API_KEY}
api.secret=${API_SECRET}
```

### Clone
  Clone this repo to your local machine using:
  ```
https://github.com/ita-social-projects/StreetCode
  ```

### Setup
  1. Change connection string  
   (Go to **appsettings.json** and write your local database connection string)
  2. Create local database  
   (Run project and make sure that database was created filled with data)


### How to run local 
 Run the Streetcode project than open your browser and enter https://localhost:7051/swagger/index.html url. If you had this page already opened, just reload it.

### How to run Docker
 
---

## Usage
### How to work with swagger UI
Run the Streetcode project than open your browser and enter https://localhost:7051/swagger/index.html url. If you had this page already opened, just reload it.

### How to run tests
### How to Checkstyle

---

## Documentation
Learn more about our documentation <a href="https://github.com/ita-social-projects/StreetCode/wiki" target="_blank">*here*</a>.

---

## Contributing

### Git flow
> To get started...
#### Step 1

- **Option 1**
    - 🍴 Fork this repo!

- **Option 2**
    - 👯 Clone this repo to your local machine using `https://github.com/ita-social-projects/StreetCode.git`

#### Step 2

- **HACK AWAY!** 🔨🔨🔨

#### Step 3

- 🔃 Create a new pull request using <a href="https://github.com/ita-social-projects/StreetCode/compare/" target="_blank">github.com/ita-social-projects/StreetCode</a>.

### Issue flow

---

## Team

<div align="center">

***Project manager***

[![@IrynaZavushchak](https://avatars.githubusercontent.com/u/45690640?s=100&v=4)](https://github.com/IrynaZavushchak) 

***Tech expert***

[![@LanchevychMaxym](https://avatars.githubusercontent.com/u/47561209?s=100&v=4)](https://github.com/LanchevychMaxym) 

***Dev team***

[![@PingvinAustr](https://avatars.githubusercontent.com/u/94307620?size=100&v=4)](https://github.com/PingvinAustr) [![@EyR1oN](https://avatars.githubusercontent.com/u/91558615?s=100&v=4)](https://github.com/EyR1oN) [![@Tatiana2424](https://avatars.githubusercontent.com/u/92846322?s=100&v=4)](https://github.com/Tatiana2424) [![@AleXLaeR](https://avatars.githubusercontent.com/u/99609396?s=100&v=4)](https://github.com/AleXLaeR) [![@dimasster](https://avatars.githubusercontent.com/u/65833018?s=100&v=4)](https://github.com/dimasster) [![@grygorenkod](https://avatars.githubusercontent.com/u/113851742?s=100&v=4)](https://github.com/grygorenkod) [![@valllentine](https://avatars.githubusercontent.com/u/90246019?s=100&v=4)](https://github.com/valllentine)

</div>

---

## FAQ

- **Сan't  install .NET Core 6.0.0+ in Visual Studio?**
    - Try to install Visual Studio 2022

---

## Support

Reach out to us at one of the following places!

- Telegram at <a href="https://t.me/ira_zavushchak" target="_blank">`Iryna Zavushchak`</a>

---

## License
- **[MIT license](http://opensource.org/licenses/mit-license.php)**
- Copyright 2022 © <a href="https://softserve.academy/" target="_blank"> SoftServe IT Academy</a>.
