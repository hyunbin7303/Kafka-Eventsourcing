using CQRS.Core.Domain;
using CQRS.Core.Events;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Post.Cmd.Infrastructure.Config;

namespace Post.Cmd.Infrastructure.Repositories
{
    public class EventStoreRepository : IEventStoreRepository
    {
        private readonly IMongoCollection<EventModel> _eventStoreCollection;
        public EventStoreRepository(IOptions<MongoDbConfig> config)
        {

        }

        public Task<List<EventModel>> FindByAggregateId(Guid aggregateId)
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync(EventModel @event)
        {
            throw new NotImplementedException();
        }
    }
}