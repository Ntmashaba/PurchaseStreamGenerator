using System;
using System.Collections.Generic;

public class CurrencyConversionRateGenerator
{
    private static readonly Random Random = new();

    public virtual List<CurrencyConversionRate> GenerateCurrencyConversionRates(DateTimeOffset timestamp)
    {
        return new List<CurrencyConversionRate>
        {
            new CurrencyConversionRate { Currency = "USD", ConversionRateToUSD = 1.00m, Timestamp = timestamp },
            new CurrencyConversionRate { Currency = "EUR", ConversionRateToUSD = RandomRate(), Timestamp = timestamp },
            new CurrencyConversionRate { Currency = "GBP", ConversionRateToUSD = RandomRate(), Timestamp = timestamp },
            new CurrencyConversionRate { Currency = "JPY", ConversionRateToUSD = RandomRate(), Timestamp = timestamp },
            new CurrencyConversionRate { Currency = "CAD", ConversionRateToUSD = RandomRate(), Timestamp = timestamp }
        };
    }

    private decimal RandomRate()
    {
        return Math.Round((decimal)(Random.NextDouble() * (1.5 - 0.5) + 0.5), 4);
    }
}
