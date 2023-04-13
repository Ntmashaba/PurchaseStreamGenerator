using Confluent.Kafka;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

public class CurrencyConversionRateFunction
{
    private readonly CurrencyConversionRateGenerator _currencyConversionRateGenerator;
    private readonly KafkaProducerFactory _kafkaProducerFactory;

    public CurrencyConversionRateFunction(CurrencyConversionRateGenerator currencyConversionRateGenerator, KafkaProducerFactory kafkaProducerFactory)
    {
        _currencyConversionRateGenerator = currencyConversionRateGenerator;
        _kafkaProducerFactory = kafkaProducerFactory;
    }

    [FunctionName("GenerateCurrencyConversionRates")]
    public async Task Run(
        [TimerTrigger("0 0 0 * * *")] TimerInfo timerInfo,
        ILogger log)
    {
        log.LogInformation("Generating currency conversion rates.");

        var conversionRates = _currencyConversionRateGenerator.GenerateCurrencyConversionRates(DateTimeOffset.UtcNow);
        var producer = _kafkaProducerFactory.Create<string, CurrencyConversionRate>("currency_conversion_rate_topic");

        foreach (var rate in conversionRates)
        {
            await producer.ProduceAsync("currency_conversion_rate_topic", new Message<string, CurrencyConversionRate> { Key = rate.Currency, Value = rate });
        }

        log.LogInformation($"Produced currency conversion rates: {string.Join(", ", conversionRates.Select(rate => $"{rate.Currency}: {rate.ConversionRateToUSD}"))}");

    }
}
