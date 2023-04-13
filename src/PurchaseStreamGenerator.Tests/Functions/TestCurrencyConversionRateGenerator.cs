public class TestCurrencyConversionRateGenerator : CurrencyConversionRateGenerator
{
    public List<CurrencyConversionRate> RatesToReturn { get; set; }

    public override List<CurrencyConversionRate> GenerateCurrencyConversionRates(DateTimeOffset timestamp)
    {
        return RatesToReturn;
    }
}
