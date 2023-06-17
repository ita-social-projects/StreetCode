<a href="https://softserve.academy/"><div align="center"><img src="https://github.com/ita-social-projects/StreetCode/blob/master/StreetCodeLogo.jpg" title="SoftServe IT Academy" alt="SoftServe IT Academy"></div></a>

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

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=ita-social-projects_StreetCode&metric=alert_status)](https://sonarcloud.io/project/overview?id=ita-social-projects_StreetCode) [![Coverage](https://sonarcloud.io/api/project_badges/measure?project=ita-social-projects_StreetCode&metric=coverage)](https://sonarcloud.io/dashboard?id=ita-social-projects_StreetCode)[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=ita-social-projects_StreetCode&metric=bugs)](https://sonarcloud.io/dashboard?id=ita-social-projects_StreetCode)[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=ita-social-projects_StreetCode&metric=code_smells)](https://sonarcloud.io/dashboard?id=ita-social-projects_StreetCode)[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=ita-social-projects_StreetCode&metric=security_rating)](https://sonarcloud.io/dashboard?id=ita-social-projects_StreetCode)
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
    - [Git flow](#github-flow)
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
* <a href="https://nuke.build/" target="_blank"> Nuke Build 6.2.1+</a> ```dotnet tool install Nuke.GlobalTool --global --version <VERSION_NUMBER>```

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
  Or if your have an associated SSH key:
  ```
git@github.com:ita-social-projects/StreetCode.git
  ```

### Setup
  1. Change connection string  
   (Go to **appsettings.json** and write your local database connection string)
  2. Create local database  
   (Run project and make sure that database was created filled with data)


### How to run local 
 Run the Streetcode project than open your browser and enter https://localhost:5001/swagger/index.html url. If you had this page already opened, just reload it.

### How to connect to db locally
1. launch SQL Server management Studio
2. In the pop-up window:
    - enter **"localhost"** as the server name;
    - select **"windows authentication"** as authentication mechanism;
3. After the connection has been established, right-click on the server (the first line with the icon), on the left-hand side of the UI
4. In the the appeared window find and click on **"properties"**
5. In the properties section, select **"security"** page
6. Make sure that **"Server authentication"** radio-button is set to **"SQL Server and Windows Authentication mode"**
7. Click "Ok"
8. Then again, on the left-hand side of the UI find folder entitled **"Security"**, and expand it
9. In unrolled list of options find folder "Logins", and expand it
10. At this point, you should have **"sa"** as the last option.
    If for some reason you do not see it, please refer to https://stackoverflow.com/questions/35753254/why-login-without-rights-can-see-sa-login
11. Right-click on the "sa" item, select "properties"
12. Change password to the default system one - **"Admin@1234"**. Don't forget to confirm it afterwards
13. On the left-hand side select **"Status"** page, and set **"Login"** radio-button to **"Enabled"**
14. Click "Ok"

Now you can connect to your localhost instance with login (sa) and password (Admin@1234)!

**_NOTE:_** Here's the full walkthrough: https://www.youtube.com/watch?v=ANFnDqe4JBk&t=211s.


### How to run Docker

1. In the **"build"** project, find the **"Targets"** folder
2. In the appeared list of classes, find and click on **"SetupPublicBuild.cs"**
3. Open the command prompt/PowerShell/linux terminal, go inside of **"/Streetcode"** directory, and start the **"SetupDocker"** Target.
```
cd ./Streetcode
nuke SetupDocker
```
After waiting for target completion, you should find that the image and running containers for back-end, front-end, db and redis have been successfully created.

**_NOTE:_**  If order to delete newly created images/containers/volumes, you can utilize the **"CleanDocker"** Target.
```
cd ./Streetcode
nuke CleanDocker
```
That will delete all unnecessary docker-atoms for you.

---

## Usage
### How to work with swagger UI
Run the Streetcode project than open your browser and enter https://localhost:5001/swagger/index.html url. If you had this page already opened, just reload it.

### How to run API without swagger UI
Run the Streetcode project in any other profile but "Local" and enter http://localhost:5000. Now, you are free to test API-endpoints with <a href="https://www.postman.com/" target="_blank">Postman</a> or any other tool.

### How to run tests
### How to Checkstyle

---

## Documentation
Learn more about our documentation <a href="https://github.com/ita-social-projects/StreetCode/wiki" target="_blank">*here*</a>.

---

## Contributing

### Gitflow

Gitflow is a lightweight, branch-based workflow.

Gitflow is an alternative Git branching model that involves the use of feature branches and multiple primary branches.

#### Step 1

- First step is checkout to `developer` branch and pull the recent changes.

#### Step 2

- 🍴 Fork this repo from `developer` branch and name it! A short, descriptive branch name enables your collaborators to see ongoing work at a glance. For example, `increase-test-timeout` or `add-code-of-conduct`. 

#### Step 3

- 🔨 On your branch, make ANY reasonable & desired changes to the repository.

#### Step 4

- :chart_with_upwards_trend: Commit and push your changes to your branch. 
Give each commit a descriptive message to help you and future contributors understand what changes the commit contains. 
For example, `fix typo` or `increase rate limit`. Note: you don't need to commit every line of your code in separate commits.

#### Step 5

- Before creating pull request you need to check the `developer` branch state! To avoid conflicts, you should merge `developer` branch to your local branch! And resolve your local conflicts. Mini manual: checkout to your local branch and write in console `git merge developer`.

#### Step 6

- 🔃 Create a new pull request using <a href="https://github.com/ita-social-projects/StreetCode_Client/pulls" target="_blank">*this link*</a>.

#### Step 7

- :raising_hand: Assign reviewers! Reviewers should leave questions, comments, and suggestions. After receiving comments, improve the code. Get Approved status on the request and be satisfied with it! 

#### Step 8

- :tada: After 3 approved reviews, merge your pull request with `developer` branch! Also, it is important to wait for your scrum master to approve your changes. If there are some conflicts, resolve them, again.

#### Step 9

- :scissors: Delete redundant branch. Done!

### Hotfixes

Oops, some fixed needs to be done immediately? Use this guide for Hotfixes!

Some fixes will be needed due to the nature of Gitflow. You would have to do a 'hotfix' or something outside of the normal process, but it's simply part of our normal process. 

#### Step 1 

- :fire: To implement an urgent change, a Hotfix branch is created off the `developer` branch to test and implement the fix.

#### Step 2

- :dancer: Once it’s complete, the Hotfix is merged with the `developer` branch.

### Issue flow

---

## Team

<div align="center">

***Project manager***

[![@IrynaZavushchak](https://avatars.githubusercontent.com/u/45690640?s=100&v=4)](https://github.com/IrynaZavushchak) 

***Tech expert***

[![@LanchevychMaxym](https://avatars.githubusercontent.com/u/47561209?s=100&v=4)](https://github.com/LanchevychMaxym) 

***Business analyst***

[![@vladnvp](https://avatars.githubusercontent.com/u/112704799?s=100&v=4)](https://github.com/vladnvp)

***Dev team***

[![@Kotusyk](https://avatars.githubusercontent.com/u/72945528?s=100&v=4)](https://github.com/Kotusyk) 
[![@Kasterov](https://avatars.githubusercontent.com/u/96317477?s=100&v=4)](https://github.com/Kasterov)
[![@Katerix](https://avatars.githubusercontent.com/u/92515141?s=100&v=4)](https://github.com/Katerix)
[![@Tysyatsky](https://avatars.githubusercontent.com/u/77460353?s=100&v=4)](https://github.com/Tysyatsky)
[![@MementoMorj](https://avatars.githubusercontent.com/u/98163405?s=100&v=4)](https://github.com/MementoMorj)
[![@Chynchenko](https://i.ibb.co/LP9n7w3/Svetlana.jpg)](https://github.com/Chynchenko)
[![@NadiaKishchuk](https://i.ibb.co/s3kgMSM/Nadia.jpg)](https://github.com/NadiaKishchuk)

[![@Dobriyr](https://avatars.githubusercontent.com/u/67451349?s=100&v=4)](https://github.com/Dobriyr)
[![@DanyilTerentiev](https://avatars.githubusercontent.com/u/96494594?s=100&v=4)](https://github.com/DanyilTerentiev)
[![@ValDekh](https://avatars.githubusercontent.com/u/61435019?s=100&v=4)](https://github.com/ValDekh)
[![@ormykhalyshyn](https://avatars.githubusercontent.com/u/92263517?s=100&v=4)](https://github.com/ormykhalyshyn)
[![@MaksBrat](https://avatars.githubusercontent.com/u/113379463?s=100&v=4)](https://github.com/MaksBrat)
[![@Lolimkeri](https://avatars.githubusercontent.com/u/57957843?s=100&v=4)](https://github.com/Lolimkeri)

</div>

---

## FAQ

- **Сan't  install .NET Core 6.0.0+ in Visual Studio?**
    - Try to install <a href="https://visualstudio.microsoft.com/ru/free-developer-offers/" target="_blank">Visual Studio 2022</a>

---

## Support

Reach out to us at one of the following places!

- Telegram at <a href="https://t.me/ira_zavushchak" target="_blank">`Iryna Zavushchak`</a>

---

## License
- **[MIT license](http://opensource.org/licenses/mit-license.php)**
- Copyright 2022 © <a href="https://softserve.academy/" target="_blank"> SoftServe IT Academy</a>.
