# RESO Web API data pull using .net core

## This project is meant to be a guide to help get started with the RESO Web API.

This repo is meant to be an example of how to pull data from the RESO API and
insert it into a local DB. After starting the full pull runs once every 24 hours
to get all the listings of a publisher using the API. The modified listings are
updated and the inactive listings are deleted from the database, each time full
pull is run.

- Portions of this repo should be configured the way you would
  like it configured, i.e. Program.cs connection string, and
  FullWorker/IncrementalWorker urls.
- This repo is meant to be executed with docker compose, docker build will fail on its own, you can run "docker-compose up" to see it in action.
- It takes roughly 1-2 hours for a full API sync and just a few minutes for the
  incremental updates with this implementation. There
  are several possible improvements depending on how you want to go about it,
  this is an example of one solution.

## NOTE:

- The database does not persist.

## General overview

- The API pull is based on
  [Worker Services](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-5.0&tabs=visual-studio).
- The `IncrementalWorker` runs once an hour and will get new listings / updates
  within the past hour.
- The `FullWorker` runs once per day and will add / update all listings from the
  API as well as delete listings that are no longer in the API.

Documentation on how to query and use the API can be found at
[api.listhub.com](https://api.listhub.com)

## Main files

- `FullWorker.cs` is the full API pull that runs once daily
- `PropertyParser.cs` parsed Property objects from the API data.
- `IncrementalWorker.cs` is the incremental pull that runs once per hour, except
  hour 0
- `ApiWorker.cs` controls when the IncrementalWorker and FullWorker run so they
  cannot run at e same time.
- `Handler.cs` takes care of database transactions, API calls, and begins photo
  transactions
- `Program.cs` contains logic to migrate the DB any time the docker container is
  started and the command needed to create new migrations
