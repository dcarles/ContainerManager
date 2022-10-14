# Containers Manager Tech Challenge

In this document, I give a brief explanation about how I tackled the test, tech stack and architecture, how you can test the system, and what I would have done if i had more time, among other minor things.

The name of the application is ContainerManager. Whenever ContainerManager is referred, it represents the system as a whole.

## Tech Stack

For this test I used following framework/libraries:

* **Development Platform** : .Net 6
* **Web API Projects** : ASP.NET 6
* **Data Access and ORM** : Entity Framework Core 6
* **Containerization**:  Docker
* **Unit Testing Framework/Libraries** : xUnit, Moq
* **Database** : MS-SQL Server
* **Other Libraries** : Automapper (for easy mapping between models), MediatR (for easy apply of mediator pattern), Fluent Validation (for validation rules)

## How to test

Before  we start testing, we need to build/start all containers.

In order to build containers and run them please use the following command in command line in project folder. Please note that you need to have Docker desktop running on your computer (and if you are using windows make sure Docker is set to use LINUX containers rather than Windows ones before you start).

`docker-compose -f docker-compose-db.yml -f docker-compose-api.yml up --build`

This will start all the necessary containers to test the application.

Use following command to take containers down if you would like to start a fresh environment. 

`docker-compose -f docker-compose-db.yml -f docker-compose-api.yml down -v`

When the containers are running the urls below are ready to be used

```
API	  : http://localhost:55100/api/
MS SQL	  : localhost,1400
```
`**NOTE!**`
```
* IF FOR SOME REASON THE PORTS USED ARE NOT AVAILABLE/USED IN YOUR MACHINE, YOU CAN CHANCE THEM IN docker-compose.services.yml AND ALSO IN THE API DOCKER FILE AND THEN TRY START THE CONTAINERS AGAIN

* ALSO IF FOR SOME REASON THE THE API THROW AN ERROR ABOUT PROBLEMS CONNECTING TO DB, THEN TRY DESTROYING THE CONTAINERS AND START THE CONTAINERS AGAIN AS PER INSTRUCTIONS ABOVE

* ALSO IF FOR SOME REASON THE API THROUGH DOCKER LOOKS UNRESPONSIVE (OR YOU JUST SIMPLY WANT TO DEBUG OR RUN 
FROM VS), RUN THE API VIA VISUAL STUDIO (REMEMBER YOU STILL NEED HAVE AT LEAST THE MS-SQL CONTAINER RUNNING FOR THIS) AND THEN USE THE FOLLOWING URL INSTEAD:
API	  : http://localhost:55101/api/
(YOU CAN ALSO CHANGE THESE PORTS IN THE launchSettings.json FILE)
```

#### Test Flow

To make any request (except for creating new user which is public access), api key must be passed in **X-Api-Key** request header as an ApiKey authorization header. Please use below user api keys (which are created first time db and app start)

| Id    | ApiKey        | EmailAddress  | Role |
| ------------- |:-------------:|:-----|:-----|
| 9A484F14-3234-440D-BF99-3FCF2ADEAF95     | testConsumerApiKey3264 | danielcarles-consumer@gmail.com | Consumer
| A6DE3AFD-5C74-407B-8A19-75C37027E610      | testOwnerApiKey3264      |  danielcarles@gmail.com | ApiOwner

- ##### As an API consumer

#####  I should be able to register as a [User]
POST http://localhost:55100/api/user
```json
{
  "email": "test@test.com",
  "firstName": "John",
  "lastName": "Doe"
}
```
######  HTTP Response Codes
203 - CREATED  : User created successfully

400 - BAD REQUEST: Validation errors

409 - CONFLICT : If there is already a user with the same email

##### As a [User]; I should be able to create a [Machine] definition which I own
POST http://localhost:55100/api/machine
HEADER 'X-Api-Key': 'testConsumerApiKey3264'
```json
{
    "name": "Machine1",
    "os": "Linux"  
}
```
######  HTTP Response Codes
203 - CREATED  : Machine created successfully

400 - BAD REQUEST: Validation errors

409 - CONFLICT : If there is already a machine with the same name

##### As a [User]; I should be able to create an [Application] definition which I own.
POST http://localhost:55100/api/application
HEADER 'X-Api-Key': 'testConsumerApiKey3264'
```json
{
    "name": "app23",
    "port": 8080,
    "image": "app23",
    "command": "bash",
    "args": "dotnet app23.dll ",
    "workingDirectory": "app",
    "cpuLimit": 1000,
    "memoryMBLimit": 8000
}
```
######  HTTP Response Codes
203 - CREATED  : Application created successfully

400 - BAD REQUEST: Validation errors

409 - CONFLICT : If there is already an application with the same name

##### As a [User]; I should be able to create an [Application] that can be ran on a [Machine]
PATCH http://localhost:55100/api/application/{applicationId}
HEADER 'X-Api-Key': 'testConsumerApiKey3264'
```json
{
    "machineId": "1207137b-ac58-468c-aaf7-0ec05e6df375"
}
```
######  HTTP Response Codes
200 - OK  : Application updated successfully

400 - BAD REQUEST: Validation errors

404 - NOT FOUND : If either machine or application not found

##### As a [User]; I should be able to query the API for information about my [Application(s)] and [Machine(s)]

GET http://localhost:55100/api/application
HEADER 'X-Api-Key': 'testConsumerApiKey3264'

######  HTTP Response Codes
200 - OK  : Application updated successfully

GET http://localhost:55100/api/machine
HEADER 'X-Api-Key': 'testConsumerApiKey3264'
######  HTTP Response Codes
200 - OK  : Application updated successfully

- ##### As an API owner

##### An API owner should be able to create [User(s)], [Machine(s)] and [Application(s)]
See above for endpoints to create entities. Just replace the api key with testOwnerApiKey3264 to use as an Api Owner

##### An API owner should be able to  remove [User(s)], [Machine(s)] and [Application(s)]
DELETE http://localhost:55100/api/application/{applicationId}
HEADER 'X-Api-Key': 'testOwnerApiKey3264'

######  HTTP Response Codes
200 - OK  : Application deleted successfully

404 - NOT FOUND : Application not found

DELETE http://localhost:55100/api/machine/{machineId}
HEADER 'X-Api-Key': 'testOwnerApiKey3264'

######  HTTP Response Codes
200 - OK  : Machine deleted successfully

404 - NOT FOUND : Machine not found

DELETE http://localhost:55100/api/user/{userId}
HEADER 'X-Api-Key': 'testOwnerApiKey3264'

######  HTTP Response Codes
200 - OK  : User deleted successfully

404 - NOT FOUND : User not found

I have added a Postman Collection export file in the main solution, which has set of requests for Consumer and API Owner for your convenience (Delete endpoints are using apiOwner default apiKey, Register user endpoint has no authentication, and all the other endpoints use a consumer default api key). You can import this file to your Postman environment and use the pre-defined requests to test.

## Architecture Overview

ContainerManager consists of 3 main components which are **Api**, **Domain**, and **Infrastructurer**. Each component of the system is explained below. 

### Clean architecture

The Project was structured using  a Clean Architecture, following Microsoft guidelines here:
[Microsoft Clean Architecture Guidelines](https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures)

![Clean Architecture simple diagram](https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/media/image5-9.png)

This has many advantages as it will produce a system that is:
1. Independent of Frameworks: The architecture does not depend on the existence of some library. This allows you to use such frameworks as tools, rather than having to adapt your system into their limited constraints.
2. Testable: The business rules can be tested without the UI, Database, Web Server, or any other external element.
3. Independent of UI: The UI can change easily, without changing the rest of the system. A Web UI could be replaced with a console UI, for example, without changing the business rules or any major changes.
4. Independent of Database: You can swap out SQL Server, for Mongo, BigTable, or something else. Your business rules are not bound to the database. You would need just to change your infrastructure layer, and if done properly only need to swap few configurations and a repo.
5. Independent of any external provider/api: Your domain don’t know anything at all about the outside world.

### ContainerManager.Api

Api is the entry point to the ContainerManager. This will be used by users to create application/machine definitions.
At the moment there are 3 controllers for each entity (each has GET/POST/DELETE endpoints). User must be authenticated to make requests to this API (except for the endpoint to create a new user which is public).

#### Authentication

Api uses a simple/naive key authentication mechanism to check users. Users are stored in database and each user is given an api key in order to be passed in the request header for Api methods to be consumed. Api key is checked by a handler called ApiKeyAutenticationHandler which checks the ApiKey is present and has valid api key and finds associated User to be used in later stages of the process. In this application, api keys are deemed to be unique in Users table. Please note that there is no mechanism to revoke or regenerate keys, or any more secure form of authentication. I excluded a more complex auth from the scope of this challenge.

#### CQRS/Mediator Pattern
CQRS is the acronym for Command Query Responsibility Segregation. The core idea of this pattern is to separate Read and Update operations by using different models to handle the treatment. We use commands to update data, and queries to read data.

According to Microsoft documentation, the CQRS pattern must follow the three principles below:
1. Commands should be task based, rather than data centric. (“Book hotel room”, not “set ReservationStatus to Reserved”).
2. Commands may be placed on a queue for asynchronous processing, rather than being processed synchronously.
3. Queries never modify the database. A query returns a DTO that does not encapsulate any domain knowledge.

The Mediator design pattern defines an object that encapsulates how a set of objects interact. Mediator promotes loose coupling by keeping objects from referring to each other explicitly, and it lets you vary their interaction independently.

The combination of Mediator pattern (done simply with Mediatr) and CQRS (done via commands and query objects) allow your system to be loosely coupled and also separate reads from writes.

### ContainerManager.Domain

This project is the domain layer of ContainerManager. It is responsible for handling commands passed from Api and process them via handlers. It interacts with Database via Repository interfaces.

This module has
- Models for each of the entities: User, Application, Machine.  
- Commands/Queries that are send from API
- Command handlers to deal with each action for each entity (Create, Get, Delete)
- Repository interfaces to deal with data save/retrieving without knowing where/how
- A simple service to generate api keys when new users are created

### ContainerManager.Infrastructure

Infrastructure is the infrastructure/data layer of ContainerManager that is responsible for interacting with database. This layer would also be responsible for any integration with 3rd party services and apis if needs be in the future.

Entity Framework Core is used as ORM in this case. There is also a generic repository which implements some basic repository methods. Then there is a specific repo for each entity created in top of that (with respective interfaces) which are used to save/retrieve. 

The data model is very simple and has 3 entities: User, Machine, and Application. Each application/machine has an OwnerId field which identifies the owner (user). 

#### Database

As explained briefly above, there are only 3 tables in the database which are Users, Applications, and Machines and there is One to Many relationships between Machine and Applications table, and between User and Applications/Machine tables.

I added 2 users when the application starts first time for test purposes. I gave the details of these users in test section above. 

If you would like to connect to SQL Server and see the data, please use following credentials. Please note that ms-sql docker container should be running in order to connect via below credentials:

```
Server: localhost, 1400
UserID : sa
Password : Password123
```

## Application Testing

The test project has different classes that consists unit tests for each component. For the unit tests I used xUnit testing framework, and Moq for Mocking purposes. No integration tests are written at this stage.

## Containerization

There are 2 containers which are api and ms-sql. In User Testing section, I give details about how these containers should be started. There are 2 compose files one is for only ms-sql and the other is for the api. There are also 2 Docker files in api project and in the docker-setup\ms-sql folder.

##  What I would have done with more time
I spent approx 32h in this project. As it was fairly complex/long to do all the requirements in a good way, there are many things that could have been done. Few of those are:

 * More tests, there are sufficient tests but not all cases/code is covered, and also there are no integration tests.
 * Better error handling and logging, at the moment there is not very much of this.
 * Better validations at the moment is very basic validations just to avoid db errors and show how validations would work.
 * Better way to retrieve applications/machines of a user as it wont be very efficient to return them all if many, so some sort of query/pagination is needed
 * Better authentication at the moment is very naive and simple just by creating api keys on user registration and check those when executing a request.
 * Better implementation of Entity framework, at the moment it was done in a very quick and simple way in a repo class, but in a real app it would need to be better implemented. Or maybe even spend more time to analyse if this would be better suited for a NoSQL db, among other things.
 * Simplify the models a bit, at the moment may be overengineered for CQRS and clean architecture and have too many probably.
 * Better management of the roles, at the moment is very basic authorization so delete endpoints can only be executed by Api Owners, and the others by any authenticated user. But in a real application you probably want for the same endpoint to do different things depending on the role.
 * Would like to have implemented the K8S deployment properly as per the challenge description, but I did not understand very well how to implement the requirement for the app to be able to deploy via K8s and start/stop each of the apps registered.


