﻿using CQRS.Core.Domain;
using CQRS.Core.Events;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Post.Cmd.Infrastructure.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Cmd.Infrastructure.Repositories
{
    public class EventStoreRepository : IEventStoreRepository
    {
        private readonly IMongoCollection<EventModel> _eventStoreCollection;

        public EventStoreRepository(IOptions<MongoDbOptions> mongoDbOptions)
        {
            var mongoClient = new MongoClient(mongoDbOptions.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mongoDbOptions.Value.Database);

            _eventStoreCollection = mongoDatabase.GetCollection<EventModel>(mongoDbOptions.Value.Collection);
        }

        public async Task<List<EventModel>> FindByAggregateId(Guid aggregateId)
        {
            return await _eventStoreCollection
                .Find(x => x.AggregateIdentifier == aggregateId).ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task SaveAsync(EventModel @event)
        {
           await _eventStoreCollection.InsertOneAsync(@event).ConfigureAwait(false);
        }
    }
}
