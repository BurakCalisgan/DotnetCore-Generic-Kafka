using Confluent.Kafka;

namespace Shared.Kafka.Consumer;

public class KafkaConsumerConfig<TKey, TValue> : ConsumerConfig
{
    public string Topic { get; set; } = null!;

    public KafkaConsumerConfig()
    {
        AutoOffsetReset = Confluent.Kafka.AutoOffsetReset.Latest;
        EnableAutoOffsetStore = false;
    }
}