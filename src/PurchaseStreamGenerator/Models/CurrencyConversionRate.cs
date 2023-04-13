using System;

public class CurrencyConversionRate
{
    public string Currency { get; set; }
    public decimal ConversionRateToUSD { get; set; }
    public DateTimeOffset Timestamp { get; set; }
}