namespace Shared.Kafka.MessageBus;

public interface IKafkaMessageBus<in TKey, in TValue>
{
    Task PublishAsync(TKey key, TValue message);
}
