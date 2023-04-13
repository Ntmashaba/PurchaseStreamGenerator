using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Startup))]

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        // Configure dependency injection here
        builder.Services.AddSingleton<PurchaseGenerator>();
        builder.Services.AddSingleton<CurrencyConversionRateGenerator>();
        builder.Services.AddSingleton(x => new KafkaProducerFactory("localhost:9092")); // Replace with your Kafka connection string
    }
}
