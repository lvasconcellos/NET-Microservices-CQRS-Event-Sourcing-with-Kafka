﻿using Confluent.Kafka;
using CQRS.Core.Events;
using CQRS.Core.Producers;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Post.Cmd.Infrastructure.Producers
{
    public class EventProducer : IEventProducer
    {
        public readonly ProducerConfig _producerConfig;
        public EventProducer(IOptions<ProducerConfig> producerConfig)
        {
            _producerConfig = producerConfig.Value;
        }

        public async Task ProduceAsync<T>(string topic, T @event) where T : BaseEvent
        {
            using var producer = new ProducerBuilder<string, string>(_producerConfig)
                 .SetKeySerializer(Serializers.Utf8)
                 .SetValueSerializer(Serializers.Utf8)
                 .Build();

            var eventMessage = new Message<string, string>
            {
                Key = Guid.NewGuid().ToString(),
                Value = JsonSerializer.Serialize(@event, @event.GetType())
            };

            var deliveryResult = await producer.ProduceAsync(topic, eventMessage);

            if (deliveryResult.Status == PersistenceStatus.NotPersisted)
            {
                throw new Exception($"Could not produce {@event.GetType().Name} message to topic - {topic} due to the following reasion: {deliveryResult.Message}");
            }
        }
    }
}
