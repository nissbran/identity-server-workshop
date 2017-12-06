# Exercise 2: Login page for Statistics Web and Admin web

In this exercise we are going to add an login page to our identity server. This is used to authenticate users for our two web MVC application. To solve this we are going to use OpenID Connect Implicit flow. 


## Exercise 2.1: Add an login page frontend to your identity server

Quickstart

### Step 1



## Exercise 2.2: Add Implict flow login for Statistics web  

The 


### Step 1

Add a new client in the identity server

```C#
new Client
{
    ClientId = "statisticsweb",
    ClientName = "Statistics web",
    AllowedGrantTypes = GrantTypes.Implicit,

    // where to redirect to after login
    RedirectUris = {"http://localhost:5003/signin-oidc"},

    // where to redirect to after logout
    PostLogoutRedirectUris = {"http://localhost:5003/signout-callback-oidc"},

    AllowedScopes = new List<string>
    {
        IdentityServerConstants.StandardScopes.OpenId,
        IdentityServerConstants.StandardScopes.Profile,
    }
},
```

### Step 2

Add test users

```C#
.AddTestUsers(new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "squeeder1",
                    Password = "password",

                    Claims = new[]
                    {
                        new Claim("name", "Squeed1"),
                        new Claim("website", "http://www.squeed.com"),
                    }
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "squeeder2",
                    Password = "password",

                    Claims = new[]
                    {
                        new Claim("name", "Squeed2"),
                        new Claim("website", "http://www.squeed.com"),
                    }
                }
            });
```

### Step 3

Add open id connect authentication in 

```C#
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

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

        options.ClientId = "statisticsweb";
        options.SaveTokens = true;
    });
```

## Recap

