using Confluent.Kafka;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

public class PurchaseStreamFunction
{
    private readonly PurchaseGenerator _purchaseGenerator;
    private readonly KafkaProducerFactory _kafkaProducerFactory;

    public PurchaseStreamFunction(PurchaseGenerator purchaseGenerator, KafkaProducerFactory kafkaProducerFactory)
    {
        _purchaseGenerator = purchaseGenerator;
        _kafkaProducerFactory = kafkaProducerFactory;
    }

    [FunctionName("GeneratePurchaseStream")]
    public async Task Run(
        [TimerTrigger("0 */1 * * * *")] TimerInfo timerInfo,
        ILogger log)
    {
        log.LogInformation("Generating purchase stream data.");

        var producer = _kafkaProducerFactory.Create<string, Purchase>("purchase_stream_topic");

        for (int i = 0; i < 100; i++)
        {
            var purchase = _purchaseGenerator.GenerateRandomPurchase();
            await producer.ProduceAsync("purchase_stream_topic", new Message<string, Purchase> { Key = purchase.Id.ToString(), Value = purchase });
        }

        log.LogInformation("100 purchase records generated and sent to the Kafka topic.");
    }
}
