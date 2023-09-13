using Confluent.Kafka;

namespace Shared.Kafka.Producer;

public class KafkaProducerConfig<TKey, TValue> : ProducerConfig
{
    public string Topic { get; set; } = null!;
}
