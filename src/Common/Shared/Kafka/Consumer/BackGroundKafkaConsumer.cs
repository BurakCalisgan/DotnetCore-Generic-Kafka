using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Shared.Kafka.Conversion.Deserialization;

namespace Shared.Kafka.Consumer;

public class BackGroundKafkaConsumer<TKey, TValue> : BackgroundService
{
    private readonly KafkaConsumerConfig<TKey, TValue> _config;
    private IKafkaHandler<TKey, TValue> _handler = null!;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public BackGroundKafkaConsumer(IOptions<KafkaConsumerConfig<TKey, TValue>> config,
        IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _config = config.Value;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        _handler = scope.ServiceProvider.GetRequiredService<IKafkaHandler<TKey, TValue>>();

        var builder = new ConsumerBuilder<TKey, TValue>(_config).SetValueDeserializer(new KafkaDeserializer<TValue>());

        using var consumer = builder.Build();
        consumer.Subscribe(_config.Topic);
        await Task.Yield();

        while (!stoppingToken.IsCancellationRequested)
        {
            var result = consumer.Consume(TimeSpan.FromMilliseconds(1000));

            if (result == null) continue;
            await _handler.HandleAsync(result.Message.Key, result.Message.Value);

            consumer.Commit(result);

            consumer.StoreOffset(result);
        }
    }
}