using Confluent.Kafka;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Timers;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using Xunit;

public class PurchaseStreamFunctionTests
{
    private readonly PurchaseStreamFunction _function;
    private readonly TestablePurchaseGenerator _testablePurchaseGenerator;
    private readonly Mock<KafkaProducerFactory> _mockKafkaProducerFactory;

    public PurchaseStreamFunctionTests()
    {
        _testablePurchaseGenerator = new TestablePurchaseGenerator();
        _mockKafkaProducerFactory = new Mock<KafkaProducerFactory>(string.Empty);
        _function = new PurchaseStreamFunction(_testablePurchaseGenerator, _mockKafkaProducerFactory.Object);
    }

    [Fact]
    public async void Run_GeneratesAndProducesExpectedNumberOfPurchases()
    {
        // Arrange
        var testPurchases = new List<Purchase>();
        for (int i = 0; i < 100; i++)
        {
            testPurchases.Add(new Purchase { Id = Guid.NewGuid(), Timestamp = DateTime.UtcNow, Currency = "USD", Amount = 10.00m });
        }

        int purchaseIndex = 0;
        _testablePurchaseGenerator.GenerateRandomPurchaseFunc = () => testPurchases[purchaseIndex++];

        var mockProducer = new Mock<IProducer<string, Purchase>>();
        _mockKafkaProducerFactory.Setup(x => x.Create<string, Purchase>(It.IsAny<string>())).Returns(mockProducer.Object);

        var mockLog = new Mock<ILogger>();

        // Create TimerInfo object
        var timerSchedule = new ConstantTimerSchedule(new TimeSpan(0, 1, 0));
        var timerInfo = new TimerInfo(timerSchedule, new ScheduleStatus(), true);

        List<Message<string, Purchase>> producedMessages = new List<Message<string, Purchase>>();

        mockProducer.Setup(x => x.ProduceAsync(
                                It.Is<string>(s => s == "purchase_stream_topic"),
                                It.IsAny<Message<string, Purchase>>(),
                                It.IsAny<CancellationToken>())
                            )
                            .Callback<string, Message<string, Purchase>, CancellationToken>((topic, message, token) =>
                            {
                                producedMessages.Add(message);
                            })
                            .Returns((string topic, Message<string, Purchase> message, CancellationToken token) => Task.FromResult(new DeliveryResult<string, Purchase> { Message = message }));

        // Act
        await _function.Run(timerInfo, mockLog.Object);

        // Assert
        
        _mockKafkaProducerFactory.Verify(x => x.Create<string, Purchase>(It.IsAny<string>()), Times.Once);

        Assert.Equal(100, producedMessages.Count);
        Assert.Equal(100, _testablePurchaseGenerator.GenerateRandomPurchaseCallCount);
    }
}
