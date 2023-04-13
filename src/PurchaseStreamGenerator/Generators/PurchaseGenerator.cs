using System;

public class PurchaseGenerator
{
    public static readonly string[] Currencies = { "USD", "EUR", "GBP", "JPY", "CAD" };
    private static readonly Random Random = new();

    public virtual Purchase GenerateRandomPurchase()
    {
        var randomCurrency = Currencies[Random.Next(Currencies.Length)];
        var randomAmount = GenerateRandomAmount(randomCurrency);

        return new Purchase
        {
            Id = Guid.NewGuid(),
            Timestamp = DateTime.UtcNow,
            Currency = randomCurrency,
            Amount = randomAmount
        };
    }

    private decimal GenerateRandomAmount(string currency)
    {
        int multiplier = currency == "USD" && Random.Next(1, 51) == 50 ? 10001 : 100;
        return Math.Round((decimal)Random.NextDouble() * multiplier, 2);
    }
}
