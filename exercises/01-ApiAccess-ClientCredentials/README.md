# Exercise 1: Client credentials for the Apis

In this exercise we are going to secure the two apis. For the Transaction api, the requests are coming from a cardterminal client, which means that no user is involved. The client credentials flow is best suited for this purpose. (https://www.oauth.com/oauth2-servers/access-tokens/client-credentials/)

## Exercise 1.1: Setup IdentityServer4

The first we have to do is to setup IdentityServer4. In the workshop solution you will find a prepared ASP.NET Core 2.0 project (Bank.Cards.IdentityServer). This is the part of the application where most of the work will be done.

### Step 1

To use IdentityServer4 we have to add the IdentityServer4 nuget package to the project using your favorite nuget install method. Here we have the dotnet command to add the package: 

```
dotnet add package IdentityServer4
```

### Step 2

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
### Step 3

If you try to run the application now, you will get an exception:
```
No storage mechanism for clients specified. Use the 'AddInMemoryClients' extension method to register a development version.
```

This means you haven't configured an store for clients. A client is an access configuration for your applications. It is where you configure how an user or client can request access tokens from the IdentityServer. In this exercise we will use the InMemory version to get started:
```C#
public void ConfigureServices(IServiceCollection services)
{
    services.AddIdentityServer()
            .AddInMemoryClients(Enumerable.Empty<Client>())
            .AddInMemoryApiResources(Enumerable.Empty<ApiResource>());
}
```
We also added an InMemory store for your api resources. An api resource is a specification for one of your applications that you want the clients to access. It translates directly to scope. 

### Step 4

Now we have configured an empty instance of IdentityServer4. To verify that it works, start the project and navigate to:

http://localhost:5000/.well-known/openid-configuration

This is the discovery endpoint for IdentityServer. The discovery endpoint can be used to retrieve metadata about your IdentityServer - it returns information like the issuer name, key material, supported scopes etc.

## Exercise 1.2: Configure IdentityServer4 to accept client credentials

Now we are going to configure our identity server to accept clients with client credentials for our Transaction api.

### Step 1

To be able to connect to the transaction api with client credentials, we need to:
* Add an Api Resource for the transaction api.
* Add an "card terminal" client with a client secret that has access to the Api Resource. We also limit the client to only use client credentials grant type.
* Add an AddDeveloperSigningCredential() configuration. This is only used during development and generates an RSA key file in your project. This key is used to sign the access tokens that IdentityServer returns. For a production setup this must be replaced with a valid certificate.

Replace the service configuration for identity server with this:
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

### Step 2

Compile and start the identity server.

### Step 3

To test the configuration we are going to request an access token from the identity server. 

```HTTP
POST /connect/token HTTP/1.1
Host: localhost:5000
Content-Type: application/x-www-form-urlencoded

grant_type=client_credentials&client_id=cardterminal1&client_secret=secret1
```

You create the request yourself or use the "Get transaction api client credentials token" request in the [Postman collection](../../postman/IdentityServer.postman_collection.json) supplied with the workshop.

The response should look similar to this:

```JSON
{
    "access_token": "eyJhbGciOiJSUzI1NiIsImtpZCI6IjFjMWFmYzU2MmI1Y2M4NTc4MDdhYmUzMmI0MGNiMzczIiwidHlwIjoiSldUIn0.eyJuYmYiOjE1MTEyNTk2NzIsImV4cCI6MTUxMTI2MzI3MiwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo1MDAwIiwiYXVkIjpbImh0dHA6Ly9sb2NhbGhvc3Q6NTAwMC9yZXNvdXJjZXMiLCJjYXJkdHJhbnNhY3Rpb25hcGkiXSwiY2xpZW50X2lkIjoiY2FyZHRlcm1pbmFsMSIsInNjb3BlIjpbImNhcmR0cmFuc2FjdGlvbmFwaSJdfQ.XXXXXX_JwsSignature_XXXXXX",
    "expires_in": 3600,
    "token_type": "Bearer"
}
```

The access token is a JWT(Json Web Token) token, that consists of 3 parts seperated by an '.':
* The first part is the base64encoded header that contains the metadata for the signing algorithm and the token type.. Decoded it looks like this: 
```JSON
{
    "alg":"RS256",
    "kid":"1c1afc562b5cc857807abe32b40cb373",
    "typ":"JWT"
}
```
* The second part is the base64encoded payload that contains all the required information about the client or user. Decoded it looks like this: 
```JSON
{
  "nbf": 1511259672,
  "exp": 1511263272,
  "iss": "http://localhost:5000",
  "aud": [
    "http://localhost:5000/resources",
    "cardtransactionapi"
  ],
  "client_id": "cardterminal1",
  "scope": [
    "cardtransactionapi"
  ]
}
```
* The third and last part is the signature. It is used to verify that the payload of the token is correct and that it originates from the correct server.

If you want to decoded your own tokens, you can go to https://jwt.io/ to decode it.

## Exercise 1.3: Modify the Transaction Api use access token authentication

In this part we will add authentication to the transaction api. We are going to use the nuget package supplied by the IdentityServer4 team, `IdentityServer4.AccessTokenValidation`. It is also possible to use the regular ASP.NET Core 2.0 `Microsoft.AspNetCore.Authentication.JwtBearer` package. The difference between them is that `IdentityServer4.AccessTokenValidation` exposes more options and the terminology aligns with the tokens that IdentityServer4 generates. The `IdentityServer4.AccessTokenValidation` even builds on Microsofts package.

### Step 1

Install the IdentityServer4.AccessTokenValidation nuget package in the Bank.Cards.Transactional.Api project

```
dotnet add package IdentityServer4.AccessTokenValidation
```

### Step 2 

Configure the Transaction api to accept tokens from our identity server. Add the following code in ConfigureServices in Startup.cs: 

```C#
...
services.AddAuthentication(defaultScheme: "Bearer")
        .AddIdentityServerAuthentication(authenticationScheme: "Bearer", configureOptions: options =>
        {
            options.Authority = "http://localhost:5000";
            options.RequireHttpsMetadata = false;
            options.ApiName = "cardtransactionapi";
        });
...
``` 

The `AddAuthentication(defaultScheme: "Bearer")` part of this code add the services needed for authentication and set the default scheme to `"Bearer"`. This means that every time an request is authorized, it will verify that the request is authenticated with the scheme `"Bearer"`. 

The options specified here is:
* Authority: This is the authority that our application thrusts. This endpoint is used to fetch the RSA public key from our IdentityServer, which is used to verify the token. The `iss` claim in the access token must also be the same as Authority.
* RequireHttpsMetadata: Must be set to `false` during development to be able fetch the RSA public key information over http.
* ApiName: The name of the ApiResource specified in your identity server. It is the same as Scope. The access token must include the scope `cardtransactionapi` so that the Transaction api will accept the token.

### Step 3

Add authentication to the pipeline:
```C#
public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }

    app.UseAuthentication();

    app.UseMvc();
}
```

It is important that the `app.UseAuthentication();` comes before the `app.UseMvc();`. It´s in the `app.UseAuthentication();` part of the pipeline that the token is validated and transformed into an authenticated User in the HttpContext.

### Step 4

Now we need to implement authorization for the Api. To do that we add the `[Authorize]` attribute to our CardPurchaseController. This will authorize all requests that targets any of the routes in the controller. If a request is authenticated with the default scheme `"Bearer"` it will be accepted. 
```C#
...
    [Route("cards")]
    [Authorize]
    public class CardPurchaseController : Controller
    {
        private readonly CreditCardPurchaseService _creditCardPurchaseService;
...
```

It is possible to specify which schemes we want to authorize here using `[Authorize(AuthenticationSchemes = "Bearer,Scheme2")]` as an example. This opens up possiblities for finer access control based on how users and clients is authenticated. As an example, you show a part of your application if the user is authenticated with a `Facebook` scheme, and the prompt the user to register so they can login with a scheme to have access to all features.

### Step 5

Now the Purchase endpoint is secure and you now needs a valid access token to access it. To do that we need to include the access token in each call to the endpoint:
```HTTP
POST /cards/purchase HTTP/1.1
Host: localhost:5002
Content-Type: application/json
Authorization: Bearer ---access_token here---

{
	"pan": "your pan",
	"amount": 20
}
```

If you have run the setup part of the workshop and have created a card, you should now get a 200 OK with the card balance:
```JSON
{
    "balance": -80
}
```

## Exercise 1.4: Add authentication to the Admin api

Now we need to do the same thing to the admin api. Use the same procedure as in the previous exercises. In the [Postman collection](../../postman/IdentityServer.postman_collection.json) there is a predefined set of requests that uses the credentials: 

```HTTP
POST /connect/token HTTP/1.1
Host: localhost:5000
Content-Type: application/x-www-form-urlencoded

grant_type=client_credentials&client_id=adminclient&client_secret=secret
```

Since the admin api have several controllers that requires authorization we can add an filter with an authorization policy to our MVC config, so that all the routes require an valid access token.
```C#
public void ConfigureServices(IServiceCollection services)
{
    services.AddMvc(config =>
    {
        var policy = new AuthorizationPolicyBuilder()
                         .RequireAuthenticatedUser()
                         .Build();
                         
        config.Filters.Add(new AuthorizeFilter(policy));
    })
}
```

## Recap

We now have a working client credentials flow for our Apis using IdentityServer4. In the next exercise we going to add an signin flow, [here](../02-LoginPage-ImplicitFlow)


