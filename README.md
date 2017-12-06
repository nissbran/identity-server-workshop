# IdentityServer4 workshop

The purpose of this workshop is an introduction to IdentityServer4 and how to apply it to a made up business problem. 

## Prerequisites
* Visual Studio 2017 Update 15.4
* Postman (https://www.getpostman.com/)
* .NET Core SDK 2.0 (https://www.microsoft.com/net/download/windows)
* EventStore 4 (https://eventstore.org/downloads/EventStore-OSS-Win-v4.0.3.zip)

## Business description

We have an business application handles transactions for cards. This application consists of a number of services:
* A Transaction api -- When an customer uses his or her card in a store the transaction api is called. 
* A Admin api -- Used by the admin web. 
* A Admin web -- Allow card admins to create cards.
* A Statistics web -- Allows card admins to se statistics for card purchases

## Problem description

None of the applications in the business description contain any authentication or authorization. The goal of this workshop is to implement both with the help of IdentityServer4.

## Setup the enviroment

### Start an instance of EventStore. 

Open an cmd prompt and go to the unzipped catalog of EventStore. Run the following command to start an instance of EventStore:
```
EventStore.ClusterNode.exe -RunProjections All -StartStandardProjections
```

### Test the solution

1. Open the solution file for the workshop. 
2. Compile and start the admin api. 
3. Import the postman collection to postman
4. Try creating some accounts and cards using postman. 


