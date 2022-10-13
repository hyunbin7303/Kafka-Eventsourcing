using CQRS.Core.Events;

namespace CQRS.Core.Infrastructure
{
    public class IEventStore
    {
        Task SaveEventsAsync(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVersion);
        Task<List<BaseEvent>> GetEventsAsync(Guid aggregateId);
    }
}