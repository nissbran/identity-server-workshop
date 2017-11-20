# Exercise 1: Client credentials for the transaction api

In this exercise we are going to secure the transaction api. The requests are coming from a cardterminal client, which means that no user is involved. The client credentials flow is best suited for this purpose. (https://www.oauth.com/oauth2-servers/access-tokens/client-credentials/)

## Exercise 1.1: Configure IdentityServer4

The first we have to do is to setup IdentityServer4. In the workshop solution you will find a prepared ASP.NET Core 2.0 project (Bank.Cards.IdentityServer). This is the part of the application where most of the work will be done.

### Step 1: Add the IdentityServer4 nuget package

To use IdentityServer4 we have to add the IdentityServer4 nuget package to the project using your favorite nuget install method. Here we have the dotnet command to add the package: 

```
dotnet add package IdentityServer4
```

### Step 2: Add the services to run IdentityServer4

To get IdentityServer4 up and running we must register its services to the ServiceCollection in ASP.NET Core 2.0.
This is done by using the supplied ServiceCollection extension:
```C#
public void ConfigureServices(IServiceCollection services)
{
    services.AddIdentityServer();
}
```

To use IdentityServer in the pipeline, you have to add it in Configure:
```C#
public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
    app.UseIdentityServer();
}
```
### Step 3: Configure clients and api resources

If you try to run the application now, you will get an exception:
```
No storage mechanism for clients specified. Use the 'AddInMemoryClients' extension method to register a development version.
```

This means you haven't configured an store for clients. A client is application/service that you allow to request access tokens from the IdentityServer. It is very similar to a user, but without the identification information. In this exercise we will use the InMemory version to get started:
```C#
public void ConfigureServices(IServiceCollection services)
{
    services.AddIdentityServer()
            .AddInMemoryClients(Enumerable.Empty<Client>()))
            .AddInMemoryApiResources(Enumerable.Empty<ApiResource>());
}
```
We also added an InMemory store for your api resources. An api resource is a specification for one of your applications that you want the clients to access.

### Step 4: Test it!

Now we have configured an empty instance of IdentityServer4. To verify that it works, start the project and navigate to:
http://localhost:5000/.well-known/openid-configuration

This is the discovery endpoint for IdentityServer. The discovery endpoint can be used to retrieve metadata about your IdentityServer - it returns information like the issuer name, key material, supported scopes etc.

## Exercise 1.2: Configure IdentityServer4 to accept client credentials

The first we have to do is to secure the transaction api. 

```C#
services
    .AddIdentityServer()
    .AddDeveloperSigningCredential()
    .AddInMemoryClients(new List<Client>
    {
        new Client
        {
            ClientId = "cardterminal1",
            AllowedGrantTypes = GrantTypes.ClientCredentials,
            ClientSecrets = new List<Secret>
            {
                new Secret("secret1".Sha512())
            },
            AllowedScopes =
            {
                "cardtransactionapi"
            }
        }
    }).AddInMemoryApiResources(new List<ApiResource>
    {
        new ApiResource("cardtransactionapi", "Card transaction api")
    });
```

### 


