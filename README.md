# Kafka-Eventsourcing




`
docker run --name sql-container --network mydockernetwork --restart always -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD={your sa pwd}" -e "MSSQL_PID=Express" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2019-latest
`


Docker related url
https://www.nocentino.com/posts/2019-09-01-persisting-sql-server-data-in-docker-containers-part-1/



## What is an Event?
- Events are objects that describe something that has occured in the application. A typical source of events is the aggregate. When something important happens in the aggregate, it will raise an event.
- Events are named with a past-partial verb.
(e.g. PostCreatedEvent, PostLikedEvent


## Mediator Pattern
- Behavioral Design Pattern
- Promotes loose coupling by preventing objects from referring to each other explicitly.
- Simplifies communication between objects by introducing a single object known as the mediator that manages the distribution of messages among other objects.
- 
