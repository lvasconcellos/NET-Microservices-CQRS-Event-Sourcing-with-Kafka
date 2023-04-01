using Confluent.Kafka;
using CQRS.Core.Consumer;
using CQRS.Core.Events;
using Microsoft.Extensions.Options;
using Post.Query.Infrastructure.Converters;
using Post.Query.Infrastructure.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Post.Query.Infrastructure.Consumers
{
    public class EventConsumer : IEventConsumer
    {
        private readonly ConsumerConfig _consumerConfig;
        private readonly IEventHandler _eventHandler;
        public EventConsumer(IOptions<ConsumerConfig> consumerConfig, IEventHandler eventHandler)
        {
            _consumerConfig = consumerConfig.Value;
            _eventHandler = eventHandler;
        }

        public void Consume(string topic)
        {
            using var consumer = new ConsumerBuilder<string, string>(_consumerConfig)
                .SetKeyDeserializer(Deserializers.Utf8)
                .SetValueDeserializer(Deserializers.Utf8)
                .Build();

            consumer.Subscribe(topic);

            while (true)
            {
                var consumeResult = consumer.Consume();

                if (consumeResult?.Message == null) continue;

                var options = new JsonSerializerOptions { Converters = { new EventJsonConverter() } };
                var @event = JsonSerializer.Deserialize<BaseEvent>(consumeResult.Message.Value, options);

                if (@event == null)
                {
                    throw new ArgumentNullException($"{nameof(@event)} Could not convert the event");
                }

                var handlerMethod = _eventHandler.GetType().GetMethod("On", new Type[] { @event.GetType() });

                if (handlerMethod == null)
                {
                    throw new ArgumentNullException($"{nameof(handlerMethod)} Could not find the event handler method");
                }

                handlerMethod.Invoke(_eventHandler, new object[] { @event });
                consumer.Commit(consumeResult);
            }
        }
    }
}
