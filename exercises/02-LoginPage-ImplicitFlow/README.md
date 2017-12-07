# Exercise 2: Login page for Statistics Web and Admin web

In this exercise we are going to add an login page to our identity server. This is used to authenticate users for our two web MVC application. To solve this we are going to use OpenID Connect Implicit flow. 

## Exercise 2.1: Add an UI to identity server

First we need to add a working UI for our identity server. Fortunately for us, the creators of IdentityServer4 have a very good MVC quickstart setup we can use to get started. This is not an production ready UI, it is primarly for demo purposes and to act as an template when building your real solution.

The repo for the quickstart is located here: https://github.com/IdentityServer/IdentityServer4.Quickstart.UI#adding-the-quickstart-ui

Follow the instructions in the link or follow the steps provided here (very similar):

### Step 1

Open an powershell terminal and navigate to the root of the Bank.Cards.IdentityServer project. Use the following command:

```Powershell
iex ((New-Object System.Net.WebClient).DownloadString('https://raw.githubusercontent.com/IdentityServer/IdentityServer4.Quickstart.UI/release/get.ps1'))
```

### Step 2 

Add MVC to your identity server and configure the pipeline:
```C#
public void ConfigureServices(IServiceCollection services)
{
    services.AddMvc();

    // Identity server configuration
}

public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
{
    loggerFactory.AddConsole();

    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }

    app.UseStaticFiles();

    app.UseIdentityServer();

    app.UseMvcWithDefaultRoute();
}
```

### Step 3

To verify that we have an UI, compile and start your identity server and navigate to: http://localhost:5000

## Exercise 2.2: Add Implict flow login for Statistics web  

With the UI in place we can now add an OpenId Implict flow client configuration in our identity server for the Statistics web.

### Step 1

Now we need to add a new client in the identity server. This client will contain more configuration, most important how to navigate between the identity server and the Statistics web.

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
}
```

### Step 2

In exercise 1 we used the ApiResources concept that uses scopes to define an application. Now when have an user with an identity instead of a defined Api. OpenID Connect uses scopes to defined identity data. We are going to add the two standard OpenId Connect scopes, which are defined in the OpenId specification.

```C#
services.AddIdentityServer()
        .AddDeveloperSigningCredential()
        ....
        .AddInMemoryIdentityResources(new List<IdentityResource>
                            {
                                new IdentityResources.OpenId(),
                                new IdentityResources.Profile(),
                            }) 
```

### Step 3

Next we need to introduce the concept of users. For demo and development purposes IdentityServer has the concept of TestUser. We are going to add two users: 

```C#
services.AddIdentityServer()
        .AddDeveloperSigningCredential()
        ....
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

TestUsers should not be used in production. If you want user control you should replace the TestUserStore in the AccountController with your own implementation. There is an tutorial in the IdentityServer4 docs if you want to use ASP.NET Identity: https://identityserver4.readthedocs.io/en/release/quickstarts/6_aspnet_identity.html

### Step 4

Now we are going to configure the Statistics web to use OpenID Connect. First we going to "fix" Microsoft's OpenIdConnect handler. More info here:
https://leastprivilege.com/2017/11/15/missing-claims-in-the-asp-net-core-2-openid-connect-handler/

```C#
public void ConfigureServices(IServiceCollection services)
{
    JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
    
    ....
```

Then we add the configuration for OpenId Connect:

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

        options.ClientId = "statisticsweb";
        options.SaveTokens = true;
    });
```

We also need to add authentication in the pipeline:
```C#
app.UseAuthentication();

app.UseMvc(routes =>
        {
            routes.MapRoute(
                name: "default",
                template: "{controller=Home}/{action=Index}/{id?}");
        });
```

What happens here is that we set the DefaultScheme to be `"Cookies"`, which is configured with the standard ASP.NET Auth Cookies scheme `.AddCookie("Cookies")`. Then we set the ChallengeScheme to `"oidc"`. 
* If an user request an MVC route with an `[Authorize]` attribute and the user has an valid cookie for scheme `"Cookies"`, then the user is authenticated and is allowed access
* If an user request an MVC route with an `[Authorize]` attribute and the user unauthenticated then we challenge the request with the scheme `"oidc"`. This results in that the user is redirected to the standard OpenId connect endpoint on the Authority site (our IdentityServer).

### Step 5

No we can test the login page by adding a `[Authorize]` attribute to the `StatisticsController` and start both the Statistics web and the identity server.
If we go to http://localhost:5003/statistics, we should now be redirect to the login page. 

### Step 6

If you tried to click the logout button in the statistics, you might have noticed that it does not work. We have to add an logout action in the `HomeController` to handle logout for both the statistics web and the identity server.

```C#
public async Task Logout()
{
    await HttpContext.SignOutAsync("Cookies");
    await HttpContext.SignOutAsync("oidc");
}
```

Here we sigout of Auth Cookie scheme and the triggers the signout on the OpenId Connect Handler. This will redirect the browser so that the user signs out of the identity server aswell.

## Exercise 2.3: Add Implict flow to Admin web

Now we should have enough to information to add the same implementation to the Admin web. Make sure that you set the correct urls in the identity server configuration. Admin web is at http://localhost:5004/.

It is also possible to turn off consent with on the client configuration:
```C#
RequireConsent = false,
```

If you done everything correct it should now be possible to go between the applications and it will autologin to the other application if you are logged in. Single sign-on implemented :)

## Exercise 2.4: Extend your identity with more information

Now we are going to add more information to our user. Each user in this system is connected to an issuer, so when a user signin we want the OpenId Connect id token to contain an issuerid claim. It is not enough to just add the claim to the testuser, we must configure it so that it is included in the token.

### Step 1

First we add the issuerId claims to the TestUsers.  

For `squeeder1`:
```C#
new Claim("issuerid", "1"),
```
For `squeeder2`:
```C#
new Claim("issuerid", "2"),
```

### Step 2

With claims registered on the TestUsers, we can now add an new IdentityResource scope `bankidentity`, that contains that `issuerid` claim.

```C#
public class BankIdentityResource : IdentityResource
{
    public BankIdentityResource()
    {
        Name = "bankidentity";
        Description = "Bank information";
        DisplayName = "Bank information";
        Required = true;
        Emphasize = true;
        UserClaims = new List<string> {"issuerid"};
    }
}
```

There are 2 important properties here, the Name is the scope name and UserClaims is the list of claims that will be included in the id_token if the scope `bankidentity` is requested. The other is used in the constent flow:
* Required: The scope is required and can´t be left out in the consent page.
* Emphasize: The scope is set as important in the consent page.

Then we add it as an allowed scope with the statistics web client
```C#
...
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
        "bankidentity"
    }
}
...
.AddInMemoryIdentityResources(new List<IdentityResource>
                {
                    new IdentityResources.OpenId(),
                    new IdentityResources.Profile(),
                    new BankIdentityResource()
                })
...
```

### Step 4

In the Statistics web, we need to add the scope to the OpenId Connect handler. This is done by adding `bankidentity` to the Scopes collection. When the handler redirects to the authorize endpoint on the identity server, it will also request the scope `bankidentity`, along with the 2 default OpenId scopes.

```C#
options.SignInScheme = "Cookies";
                       
options.Authority = "http://localhost:5000";
options.RequireHttpsMetadata = false;

options.ClientId = "statisticsweb";
options.SaveTokens = true;

options.Scope.Add("bankidentity");
```

### Step 5

To verify that this is working, we can add some code that depends on issuerId. Replace the code in the `Index()` method in the `StatisticsController` with this:

```C#
var issuerId = User.Claims.First(claim => claim.Type == "issuerid").Value;

var accounts = await _accountStatisticsService.GetAccountSummaryForIssuer(long.Parse(issuerId));

return View(new StatisticsViewModel
            {
                Accounts = accounts.Select(summary => new AccountViewModel(summary))
            });
```

Now we can run and test that we get the issuerid along with the claims. 

## Recap

We have now implemented single sign-on using IdentityServer4 and the OpenId Connect Implict flow. 

## Extra information

If you want to configure your own routes in identity server instead of the default (`"account/login"`) that exist in the quickstart, it is possible to configure IdentityServer4 during service registration. Example: 

```C#
services.AddIdentityServer(options =>
                {
                    options.UserInteraction.LoginUrl = "myurl/login";
                })
``` 