using Confluent.Kafka;
using Microsoft.Azure.WebJobs.Extensions.Timers;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Moq;

namespace PurchaseStreamGenerator.Tests.Functions
{
    public class CurrencyConversionRateFunctionTests
    {
        private readonly CurrencyConversionRateFunction _function;
        private readonly Mock<CurrencyConversionRateGenerator> _mockGenerator;
        private readonly Mock<KafkaProducerFactory> _mockKafkaProducerFactory;

        public CurrencyConversionRateFunctionTests()
        {
            _mockGenerator = new Mock<CurrencyConversionRateGenerator>();
            _mockKafkaProducerFactory = new Mock<KafkaProducerFactory>(string.Empty);
            _function = new CurrencyConversionRateFunction(_mockGenerator.Object, _mockKafkaProducerFactory.Object);
        }

        [Fact]
        public async void Run_ProducesExpectedCurrencyConversionRates()
        {
            // Arrange
            var testTimestamp = DateTimeOffset.UtcNow;
            var testRates = new List<CurrencyConversionRate>
    {
        new CurrencyConversionRate { Currency = "USD", ConversionRateToUSD = 1.00m, Timestamp = testTimestamp }
    };

            _mockGenerator.Setup(x => x.GenerateCurrencyConversionRates(It.IsAny<DateTimeOffset>())).Returns(testRates);

            var mockProducer = new Mock<IProducer<string, CurrencyConversionRate>>();
            _mockKafkaProducerFactory.Setup(x => x.Create<string, CurrencyConversionRate>(It.IsAny<string>())).Returns(mockProducer.Object);


            var mockLog = new Mock<ILogger>();

            // Create TimerInfo object
            var timerSchedule = new ConstantTimerSchedule(new TimeSpan(0, 1, 0));
            var timerInfo = new TimerInfo(timerSchedule, new ScheduleStatus(), true);


            List<Message<string, CurrencyConversionRate>> producedMessages = new List<Message<string, CurrencyConversionRate>>();

            mockProducer.Setup(x => x.ProduceAsync(
                            It.Is<string>(s => s == "currency_conversion_rate_topic"),
                            It.IsAny<Message<string, CurrencyConversionRate>>(),
                            It.IsAny<CancellationToken>())
                        )
                        .Callback<string, Message<string, CurrencyConversionRate>, CancellationToken>((topic, message, token) =>
                        {
                            producedMessages.Add(message);
                        })
                        .Returns((string topic, Message<string, CurrencyConversionRate> message, CancellationToken token) => Task.FromResult(new DeliveryResult<string, CurrencyConversionRate> { Message = message }));

            // Act
            await _function.Run(timerInfo, mockLog.Object);

            // Assert
            _mockGenerator.Verify(x => x.GenerateCurrencyConversionRates(It.IsAny<DateTimeOffset>()), Times.Once);
            _mockKafkaProducerFactory.Verify(x => x.Create<string, CurrencyConversionRate>(It.IsAny<string>()), Times.Once);

            Assert.Single(producedMessages);
            Assert.Equal(testRates[0].Currency, producedMessages[0].Key);
            Assert.Equal(testRates[0], producedMessages[0].Value);
        }
    }
}
