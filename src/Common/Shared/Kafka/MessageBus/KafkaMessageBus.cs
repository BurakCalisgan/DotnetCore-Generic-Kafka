using Shared.Kafka.Producer;

namespace Shared.Kafka.MessageBus;

public class KafkaMessageBus<TKey, TValue> : IKafkaMessageBus<TKey, TValue>
{
    private readonly KafkaProducer<TKey, TValue> _producer;

    public KafkaMessageBus(KafkaProducer<TKey, TValue> producer)
    {
        _producer = producer;
    }

    public async Task PublishAsync(TKey key, TValue message)
    {
        await _producer.ProduceAsync(key, message);
    }
}