using Confluent.Kafka;
using System.Text.Json;

public class KafkaProducerFactory
{
    private readonly string _bootstrapServers;

    public KafkaProducerFactory(string bootstrapServers)
    {
        _bootstrapServers = bootstrapServers;
    }

    public virtual IProducer<TKey, TValue> Create<TKey, TValue>(string topic)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = _bootstrapServers
        };

        var producerBuilder = new ProducerBuilder<TKey, TValue>(config)
            .SetValueSerializer(new CustomJsonSerializer<TValue>());

        return producerBuilder.Build();
    }
}

public class CustomJsonSerializer<T> : ISerializer<T>
{
    public byte[] Serialize(T data, SerializationContext context)
    {
        if (data == null) return null;

        return JsonSerializer.SerializeToUtf8Bytes(data);
    }
}
