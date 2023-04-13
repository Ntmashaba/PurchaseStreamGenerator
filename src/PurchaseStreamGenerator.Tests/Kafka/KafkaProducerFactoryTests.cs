public class KafkaProducerFactoryTests
{
    private readonly KafkaProducerFactory _factory;

    public KafkaProducerFactoryTests()
    {
        _factory = new KafkaProducerFactory("localhost:9092");
    }

    [Fact]
    public void Create_ProducerIsNotNull()
    {
        // Arrange
        var topic = "test_topic";

        // Act
        var producer = _factory.Create<string, string>(topic);

        // Assert
        Assert.NotNull(producer);
        Assert.NotNull(producer.Handle);
    }
}
