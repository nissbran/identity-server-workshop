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
* A Admin api -- Used by the admin portal and the customer portal. 
* A Admin portal -- Allow card admins to create cards.
* A Customer portal -- Allows card customers to see their balance and transactions

## Problem description

None of the applications in the business description contain any authentication or authorization. The goal of this workshop is to implement both with the help of IdentityServer4.

## Setup the enviroment

### Start an instance of EventStore. 

Go to the unzipped catalog of EventStore. Click the EventStore.ClusterNode.exe file to start an instance of EventStore.

### Test the solution

1. Open the solution file for the workshop. 
2. Compile and start the admin api. 
3. Import the workshop postman collection to postman
4. Try creating some accounts and cards using postman. 

### Configure the IdentityServer4



## Step 3: Setup client credentials for the transaction api

The first we have to do is to secure the transaction api. The requests are coming from a cardterminal client, which means that no user is involved. The client credentials flow is best suited for this purpose.

### 


