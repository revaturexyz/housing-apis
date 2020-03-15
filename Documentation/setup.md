# Getting Started

## Cloning the Repo

`git clone https://github.com/revaturexyz/housing-apis.git`

## Setting up Consumed Services

### Okta

As of right now, this is needed for all services besides the Address service.
To get Okta working from the codebase, an Okta account is needed.
Go to https://developer.okta.com, click on signup, and fill out the required information.
After confirming your email, go to your new account and click on Users -> Groups and click add group, creating "Coordinator". Create the groups "Provider" and "Tenant" as well.
Your groups should look something like this:

![something](./Images/groups.png "groups")

Next, go to API -> Authorization Servers and select the default server. (note the URI shown)
Click on Claims, then on Add Claim. Set the name "Roles", Include in token type to "ID token", "Always", Value type "Groups", Filter to matches regex `.*`, and include in any scope. It should look like this:

![something](./Images/editclaim.png "editclaims")

Then, make another using Access. Your settings should look something like this when done:

![something](./Images/finalclaims.png "finalclaims")

Next, go to the applications tab. Click add application. For the front end, select single page app and click next. Change the Base URI to localhost:4200 and choose a descriptive name. In the Angular app, change [`environment.ts`] to include:

- `domain: 'https://dev-######.okta.com'`
- `issuer: 'https://dev-######.okta.com/oauth2/default'`
- `clientID: '<clientid from the app you just created>'`
- `redirectUri: 'https://localhost:4200/implicit/callback'`.

App.config should be injected in `app.module.ts`.

Next, make a token. To do this, click on API -> Tokens, then on create token. Name the token whatever you like, e.g. `managementToken`. *Make sure not to lose the token value.*
In each API, add the following in `appsettings.Development.json`, using your Okta domain, client ID, and token. Only the Identity service needs the token. In addition to adding them to the various appsettings, domain and client ID need to be added to [`environment.ts`] and [`environment.prod.ts`] in the front-end.

![something](./Images/Okta.png "Okta")

#### User accounts in seed data

There are a number of accounts in the seed data in identity, but you will have to create new Okta accounts for them. Your trainer will have the email passwords for these accounts. Also note that some of these emails are `revtest{one-four}+{number}@gmail.com`. Okta treats emails with +{tag} in them as different emails; however, all accounts with the same one-four number are accessed through the same email account.

```
COORDINATOR
Name: Test_One
Email: revtestone2020@gmail.com, revtestone2020+1@gmail.com
Password: (hidden)
Birthday: January 1 2000
OKTA password: UTAokta2020
OKTA What is your favorite security question: None

PROVIDER - PENDING
Name: Test_Two
Email: revtesttwo2020@gmail.com
Password: (hidden)
Birthday: January 1 2000
OKTA password : UTAokta2020
OKTA What is your favorite security question: None

PROVIDER - APPROVED
Name: Test_Three
Email: revtestthree2020@gmail.com, revtestthree2020+1@gmail.com
Password: (hidden)
Birthday: January 1 2000
OKTA password : UTAokta2020
OKTA What is your favorite security question: None

TENANT
Name: Test_Four
Email: revtestfour2020@gmail.com, revtestfour2020+1@gmail.com, revtestfour2020+2@gmail.com
Password: (hidden)
Birthday: January 1 2000
OKTA password : UTAokta2020
OKTA What is your favorite security question: None
```

### Google APIs
#### This section might look filled in, but it does need work

This section is required only for the Address service, which consumes the google maps Distance Matrix API and Geocoding API.

https://cloud.google.com/maps-platform/

1. From the above link, click "Get Started".
2. You will be prompted to create a new project. Enter a name for your project.

    ![something](./Images/CreateProject.JPG "createproject")

3. Agree to the Terms of Service pictured above, and click Create.
4. You will be prompted to enable billing. Click Create Billing Account.

    ![something](./Images/CreateBillingAccount.JPG "createbillingaccount")

5. Step through the process to set up your billing.
6. You will be prompted to pick your products. Check the boxes next to the Routes and Places APIs to enable them. These will give you access to the Distance Matrix and Geocoding API as required above.

    ![something](./Images/PickYourProducts.JPG "chooseyourproducts")

7. Click Next. In the next window, fill out your industry and what you want to build as prompted.
8. Your API key will be generated for you! Store it somewhere safe and click Done.

## Running with Visual Studio
### Configuration
Because `appsettings.json` is not gitignored, you will have to copy the contents of it to `appsettings.Development.json`, which is gitignored, before making any changes.
The following uses identity as an example of what you would put in appsettings.

```json
{
  "ConnectionStrings": {
    "ServiceBus": "(Enter Service Bus Connection String)",
    "IdentityDb": "Server=localhost;Port=8000;Database=identity;Username=postgres;Password=Pass@word"
  },
  "Okta": {
    "OktaDomain": "https://dev-example.okta.com",
    "ClientId": "Example-ClientId-0oas0sNkOVRvCKy46",
    "Token": "Example-Token-000jeGaXFDDLByNZ_B58uwTb3zQ9kFIYl"
  }
}
```

### Running the database container
We use docker to spin up development databases with the following command:
```
docker run --rm -it -e POSTGRES_PASSWORD=Pass@word -p 8000:5432 postgres:alpine
```
This will spin up a docker container with the postgres image that can be accessed on port `8000` with password `Pass@word`. If you are using Docker Desktop, this will be `localhost:8000`. If you are using Docker Toolbox, this will be at `192.168.99.100:8000`. In order to access it from Visual Studio, you will need to set up the connection string in your `secrets.json` file. The following is an example for the Tenant service with Docker Desktop, on port `8000`, with password `Pass@word`. **You will have to change `localhost` to `192.168.99.100` if you are using Docker Toolbox.**

```json
{
  "ConnectionStrings": {
    "TenantDb": "Server=localhost;Port=8000;Database=tenant;Username=postgres;Password=Pass@word"
  },
  "Okta": {
    "OktaDomain": "Okta-Domain-Here"
  }
}
```

### Running the code
After you have the appsettings completely filled out and the database running in docker, you can start your program as you would any program in VS.

## Running with Docker Compose
### Configuration
You need to fill data in these 5 files to match what is in appsettings, and you need to copy `*.template` to the same name but without the `.template` extension. Ideally, there should be a script that splits config from a single file into the five files needed by docker compose.

`A__B=setting` (note there are no quotes) in these files is the same setting as `"A": { "B": "setting" }`

The five files that hold the configuration follow. Note that you don't need to set up database connection strings because that is handled by docker compose.

- [`.addressAPIKey.template`] => `.addressAPIKey`
- [`.commonConfig`] (You shouldn't have to touch this unless you are adding a service or a service bus queue)
- [`.commonSecret.template`] => `.commonSecret`
- [`.identitySecret.template`] => `.identitySecret`
- [`.oktaSecrets.template`] => `.oktaSecrets`

### Commands
```
docker-compose -f manifest.docker.yaml up identity_data address_data lodging_data tenant_data
```

You need to run the data containers first and wait for them to be in a state to accept connections.

![Docker Compose DB Ready](./Images/DockerComposeDataReady.png "Docker Compose Database Containers Ready")

```
docker-compose -f manifest.docker.yaml build identity_api address_api lodging_api tenant_api
```

If you have made changes to your application, you will need to run the build command to ensure you are using the latest version. This step can be run in parallel with the previous step without any issues.

```docker-compose -f manifest.docker.yaml up identity_api address_api lodging_api tenant_api```

After the data containers are ready, and the api has built (if needed), you are ready to run your services in docker containers. After this step, if everything went well, you should be able to access the services at the local address noted in each service.

![Docker Compose API Ready](./Images/DockerComposeApiReady.png "Docker Compose API Containers Ready")

```
docker-compose -f manifest.docker.yaml down
```

By default, after stopping the containers, data persists. If you want to return to seed data, you need to run the `down` command.

# Working with existing architecture
## Adding Okta to a new API

Middleware: Add to `Startup.cs` in `ConfigureServices`:

```c#
services.AddAuthentication(options =>
{
  	options.DefaultAuthenticateScheme = OktaDefaults.ApiAuthenticationScheme;
   	options.DefaultSignInScheme = OktaDefaults.ApiAuthenticationScheme;
   	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
  options.Authority = Configuration["Okta:Domain"] + "/oauth2/default";
  options.Audience = "api://default";
  options.RequireHttpsMetadata = true;
  options.SaveToken = true;
  options.TokenValidationParameters = new TokenValidationParameters
  {
    NameClaimType = "name",
    RoleClaimType = "groups",
    ValidateIssuer = true,
  };
});
```

In your controller, add the `Authorize` filter to add authentication and authorization to the controller and/or http methods. E.g.

```c#
// adds authentication, only authenticated users can access
[Authorize]

// authentication + role-based authorization, can only be accessed by an account that has
//'Coordinator' in its role
[Authorize(Roles="Coordinator")]

// authorization for Coordinator OR Provider
[Authorize(Roles="Coordinator,Provider")]

// authorization for Coordinator AND Provider
[Authorize(Roles="Coordinator")]
[Authorize(Roles="Provider")]
```

![something](./Images/oktasetup.png "oktasetup")

## Pipeline
### When it runs
The pipeline runs on every pull request into master as well as commits/accepted PRs on master. The pipeline only runs the build stage on PRs and all of the build, pack, and deploy stages on commits.

### Reasons for failed pipelines
The pipeline will fail on failed tests, so make sure your tests pass before you push.

### Artifacts
A successful build on a pull request into master will deploy the services on Azure App Service, the root endpoints are listed in the individual services.

In addition to deploying the services, it provides static analysis on [SonarCloud].

[SonarCloud]: https://sonarcloud.io/organizations/revaturexyz/projects?sort=-analysis_date

[`.addressAPIKey.template`]: ../.addressAPIKey.template
[`.commonConfig`]: ../.commonConfig
[`commonSecret.template`]: ../.commonSecret.template
[`identitySecret.template`]: ../.identitySecret.template
[`oktaSecrets.template`]: ../.oktaSecrets.template
[`environment.ts`]: https://github.com/revaturexyz/housing/blob/master/housing/src/environments/environment.ts
[`environment.prod.ts`]: https://github.com/revaturexyz/housing/blob/master/housing/src/environments/environment.prod.ts
