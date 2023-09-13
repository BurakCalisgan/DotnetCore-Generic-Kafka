namespace Shared.Kafka.Consumer;

public interface IKafkaHandler<in TKey, in TValue>
{
    Task HandleAsync(TKey key, TValue value);
}
