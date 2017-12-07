# Exercise 3: Backend access for Admin web

In this exercise we are going to configure so that our Admin Web can access our Admin api backend service. To do this we are going to "upgrade" our client for the Admin web to the Hybrid flow. This means that we will include an authorization code in our response along with the id_token.

## Exercise 3.1: Modify the client in Identity Server

Now we going to modify the `adminweb` client in our identity server configuration, so that it uses Hybrid grant type instead of Implicit.

### Step 1

Change the client configuration to this:

```C#
new Client
{
    ClientId = "adminweb",
    ClientName = "Admin web",
    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,

    ClientSecrets =
    {
        new Secret("secret".Sha256())
    },
    
    // where to redirect to after login
    RedirectUris = {"http://localhost:5004/signin-oidc"},

    // where to redirect to after logout
    PostLogoutRedirectUris = {"http://localhost:5004/signout-callback-oidc"},

    AllowedScopes = new List<string>
    {
        IdentityServerConstants.StandardScopes.OpenId,
        IdentityServerConstants.StandardScopes.Profile,
        "bankidentity",
        "adminapi"
    }
}
```

There are 3 important differences compared to Implicit:
* AllowedGrantType is changed to support HybridAndClientCredentials.
* We added a client secret to the client. This is used by the Admin web when it requests access tokens.
* We added the scope `adminapi` to our allowed scopes, which is the ApiResource for our Admin api. 

## Exercise 3.2: Update the configuration in Admin web

Now we need to modify the OpenId Authentication Handler in our Admin web.

### Step 1

Update the configuration to the following:
```C#
 services.AddAuthentication(options =>
    {
        options.DefaultScheme = "Cookies";
        options.DefaultChallengeScheme = "oidc";
    })
    .AddCookie("Cookies")
    .AddOpenIdConnect("oidc", options =>
    {
        options.SignInScheme = "Cookies";

        options.Authority = "http://localhost:5000";
        options.RequireHttpsMetadata = false;
        options.ResponseType = "code id_token";
        options.GetClaimsFromUserInfoEndpoint = true;

        options.ClientId = "adminweb";
        options.ClientSecret = "secret";
        options.SaveTokens = true;

        options.Scope.Add("adminapi");
        options.Scope.Add("bankidentity");
    });
```

The big difference here is that we configured `options.ResponseType = "code id_token";`, which means that our application wants both an Authorization Code and an id_token back when the user sign in and accept consent. We also added the client secret and the scope `adminapi`. 

We also added `options.GetClaimsFromUserInfoEndpoint = true;`, which is needed if we want to get the identity information for the User. When using Hybrid flow, identity server is not adding any user claims to the id_token. Instead we fetch them using the access token from the UserInfo endpoint. The access token is requested by the OpenId Handler from our identity server using the authorization code, which it saves in the Auth Cookie (`options.SaveTokens = true;`).

### Step 2

Now we need to use our access token in our call to the admin backend. In our Admin web project there is an `AdminHttpService` that handles all calls to the backend. It is prepared with an `IHttpContextAccessor` service, so that we can access our token from our HttpContext. Modify the `PostAsync` method:

```C#
var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");

var requestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri);

requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
requestMessage.Content = new StringContent(content, Encoding.UTF8, "application/json");

var response = await _httpClient.SendAsync(requestMessage);

return response;
```

Here we fetch the access_token from our `HttpContext` and add it to the Authorization header in the request to our Admin api.

### Step 3

Now need going to verify that we can access our backend Admin api. Start the identity server, Admin web and the Admin api. Go to http://localhost:5004/Account/Create and try to create an account. If everything goes well we should land back at http://localhost:5004/Account/ and we have an new account in the EventStore.

## Recap

In this exercise we have configured our Admin web so that it can communication with our Admin api.