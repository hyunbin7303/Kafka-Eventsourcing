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


## What is an Aggregate?
- The aggregate can be viewed as the domain entity on the write or command side of a CQRS and event sourcing based application or service, similar to the domain entity that you find on the read or query side.
- When you create the aggregate class, you will see that it is diffcult at first glance to view the aggregate as a domain entity, because unlike the domain entirty on the read side, it is not a simple plain old c# object that contains only state and no behavior.
- the fundamental differences in its structure is due to the fundamental differences in how the data is stored in the write database or event store vs how it is stored in the read database. 

* the design of the aggregate should therefore allow you to be able to use these events to recreate or replay the latest state of the aggregate, so that you don't have to query the read database for the latest state, else the hard separation of commands or queries would be in vain. 
