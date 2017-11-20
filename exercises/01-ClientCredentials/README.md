# Client credentials for the transaction api

In this exercise we are going to secure the transaction api. The requests are coming from a cardterminal client, which means that no user is involved. The client credentials flow is best suited for this purpose. (https://www.oauth.com/oauth2-servers/access-tokens/client-credentials/)


## Step 1: Configure IdentityServer4 to accept client credentials

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


