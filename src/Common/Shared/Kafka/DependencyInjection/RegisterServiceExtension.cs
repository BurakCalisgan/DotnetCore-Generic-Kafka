using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shared.Kafka.Consumer;
using Shared.Kafka.Conversion.Serialization;
using Shared.Kafka.MessageBus;
using Shared.Kafka.Producer;

namespace Shared.Kafka.DependencyInjection;

public static class RegisterServiceExtension
{
    public static IServiceCollection AddKafkaMessageBus(this IServiceCollection serviceCollection)
        => serviceCollection.AddSingleton(typeof(IKafkaMessageBus<,>), typeof(KafkaMessageBus<,>));

    public static IServiceCollection AddKafkaConsumer<TKey, TValue, THandler>(this IServiceCollection services,
        Action<KafkaConsumerConfig<TKey, TValue>> configAction) where THandler : class, IKafkaHandler<TKey, TValue>
    {
        services.AddScoped<IKafkaHandler<TKey, TValue>, THandler>();

        services.AddHostedService<BackGroundKafkaConsumer<TKey, TValue>>();

        services.Configure(configAction);

        return services;
    }

    public static IServiceCollection AddKafkaProducer<TKey, TValue>(this IServiceCollection services,
        Action<KafkaProducerConfig<TKey, TValue>> configAction)
    {
        services.AddConfluentKafkaProducer<TKey, TValue>();

        services.AddSingleton<KafkaProducer<TKey, TValue>>();

        services.Configure(configAction);

        return services;
    }

    private static void AddConfluentKafkaProducer<TKey, TValue>(this IServiceCollection services)
    {
        services.AddSingleton(
            sp =>
            {
                var config = sp.GetRequiredService<IOptions<KafkaProducerConfig<TKey, TValue>>>();

                var builder =
                    new ProducerBuilder<TKey, TValue>(config.Value).SetValueSerializer(new KafkaSerializer<TValue>());

                return builder.Build();
            });
    }
}
