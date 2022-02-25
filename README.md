﻿# RESO Web API data pull using .net core

## This project is meant to be a guide to help get started with the RESO Web API.
>- You can run "docker-compose up" inside the src directory to see it in action, the first time it is ran will take a little time as it is downloading all fake photos for all listings.
>- It takes roughly 2-3 hours for a full API sync and just a few minutes for the incremental updates with this impelementation, photo downloads included. There are several possible improvements depending on how you want to go about it, this is just an example of one solution.

## NOTE:
> The photos persist locally in "../photos/" change the docker-compose.yml if you want to save them locally somewhere else.
>
> The database does not persist.

## General overview
> The API pull is based on <a href="https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-5.0&tabs=visual-studio">Worker Services</a>.
>
> The IncrementalWorker runs once an hour and will get new listings / updates within the past hour.
>
> The FullWorker runs once per day and will add / update all listings from the API as well as delete listings that are no longer in the API.


## Documentation on how to query and use the API can be found at <a href="https://api.listhub.com">api.listhub.com</a>

## Main files
> "FullWorker.cs" is the full API pull that runs once daily
>
> "PropertyParser.cs" parsed Property objects from the API data.
>
> "IncrementalWorker.cs" is the incremental pull that runs once per hour, except hour 0
>
> "ApiWorker.cs" controls when the IncrementalWorker and FullWorker run so they cannot run at the same time.
>
> "Handler.cs" takes care of database transactions, API calls, and begins photo transactions
>
> "PhotoHandler.cs" takes care of downloading and deleting photos with the help of "Handler"
>
> "Program.cs" contains logic to migrate the DB any time the docker container is started and the command needed to create new migrations