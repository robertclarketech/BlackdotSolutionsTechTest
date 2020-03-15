# Building

- First ensure that all nuget packages have been restored. This can be done by going to the root of the project and running `dotnet restore`. This should also install `dotnet ef` if you  don't already have it.

- Then from the .NET solution directory (`/api`), run `dotnet ef database update --project .\BlackdotTechTest.WebApi\` in order to create the SQLite database we will be putting data into.

	- If you don't have sqlite installed, you may need to install it. If you're running a Linux distribution, this is very commonly included in most package managers. If you're running windows, the most pain free way to install this is to use a package manager such as [Chocolatey](https://chocolatey.org/packages/SQLite) or [Scoop](https://scoop.sh/).

- Using either the Firefox or Chrome Selenium drivers requires the full browser to be installed on your system. Install at least one of these and set `SearchEngineServiceSettings.DriverType` in `BlackdotTechTest.WebApi/appsettings.json` to either `Firefox` or `Chrome`.

	- Despite the fact I included Linux instructions for sqlite, the only included Selenium drivers are both for Windows. Theoretically these could be swapped out for Linux versions rather easily, but would probably require some configuration changes when creating the WebDrivers, so for now please consider this project to be Windows only.

	- Firefox's Selenium Driver also seems significantly slower than Chrome's. I don't know why.

- After that you can run `dotnet run  --project .\BlackdotTechTest.WebApi\` to start the project.


# Endpoints

This API includes a swagger endpoint at `/swagger/index.html` to allow for easy exploration of the API.

The API exposes 3 endpoints: `GET /SearchEngineInvestigateController`, `GET /SearchEngineInvestigateController/{query}` and `POST /SearchEngineInvestigateController/{query}`

- `GET /SearchEngineInvestigateController` will list all the queries that have been run against the api (but not their results)

- `GET /SearchEngineInvestigateController/{query}` will return the query this api has run, and its results.

- `POST /SearchEngineInvestigateController/{query}` will perform a new query for the given keyword.


# Architecture

For this project, I've chosen to go with a DDD style architecture with CQRS. The code is split into 3 libraries: WebApi, Infrastructure, and Domain.

- Domain: This holds all the base entites, commands, queries and configurations that is shared throughout all layers. Since we're going for a domain centric model, the domain entities themselves hold logic and validation that ensures that all entities are valid as soon as they are created, and more importantly the aggregate roots hold control over their children, ensuring that they are valid when created or updated.

- Infrastructure: This holds all the "plumbing" for the application, so to speak. Here is Query Handlers and Commands are executed, with Database access also being described here. In theory, this layer could be lifted out, or extra implementations added, and assuming all commands and queries were still appropriately covered by the new implementations, everything should continue to run.

- WebApi: This layer is essentially a shim for running commands and queries, exposing access to the underlying implementations. There should be little to no business logic here.


# Notes

- All searches are done with Selenium with JS disabled. Javascript disabled is to ensure that we get more consistent pages returned. This approach probably isn't tenable for more than basic tests, since it will be pretty obvious we're a bot to anyone looking.
