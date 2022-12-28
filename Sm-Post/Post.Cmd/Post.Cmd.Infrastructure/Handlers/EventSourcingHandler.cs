using CQRS.Core.Domain;
using CQRS.Core.Handlers;
using CQRS.Core.Infrastructure;
using CQRS.Core.Producers;
using Post.Cmd.Domain.Aggregates;
using System.Net.WebSockets;

namespace Post.Cmd.Infrastructure.Handlers
{
    public class EventSourcingHandler : IEventSourcingHandler<PostAggregate>
    {
        private readonly IEventStore _eventStore;
        private readonly IEventProducer _eventProducer;
        public EventSourcingHandler(IEventStore eventStore, IEventProducer producer)
        {
            _eventStore = eventStore;
            _eventProducer = producer;
        }
        public async Task<PostAggregate> GetByIdAsync(Guid aggregateId)
        {
            var aggregate = new PostAggregate();
            var events = await _eventStore.GetEventsAsync(aggregateId);
            if(events == null || !events.Any()) return aggregate;

            aggregate.ReplayEvents(events);
            aggregate.Version = events.Select(x => x.Version).Max();
            return aggregate;
        }

        public async Task RepublishEventsAsync()
        {
            var aggregateIds = await _eventStore.GetAggregateIdAsync();
            if (aggregateIds == null || !aggregateIds.Any())
                return;

            foreach(var aggregateId in aggregateIds)
            {
                var aggregate = await GetByIdAsync(aggregateId);
                if (aggregate == null || !aggregate.Active) continue;

                var events = await _eventStore.GetEventsAsync(aggregateId);
                foreach(var @event in events)
                {
                    var topic = Environment.GetEnvironmentVariable("KAFKA_TOPIC");
                    await _eventProducer.ProducerAsync(topic, @event);
                }
            }
        }

        public async Task SaveAsync(AggregateRoot aggregate)
        {
            await _eventStore.SaveEventsAsync(aggregate.Id, aggregate.GetUncommittedChanges(), aggregate.Version);
            aggregate.MarkChangesAsCommitted();
        }
    }
}


/*
For the state of the aggregate to be replayed/recreated correctly, it is important that the ordering of events is enforced by implementing
event versioning. So that the events are stored in the Event store in the correct order.
Optimistic concurrency control is then used to ensure that only the expected event versions can be persisted to the event store.

*/