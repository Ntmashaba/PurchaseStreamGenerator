using System;
using System.Linq;
using Xunit;

public class PurchaseGeneratorTests
{
    private readonly PurchaseGenerator _purchaseGenerator;

    public PurchaseGeneratorTests()
    {
        _purchaseGenerator = new PurchaseGenerator();
    }

    [Fact]
    public void GenerateRandomPurchase_GeneratesValidPurchase()
    {
        // Act
        Purchase purchase = _purchaseGenerator.GenerateRandomPurchase();

        // Assert
        Assert.NotEqual(Guid.Empty, purchase.Id);
        Assert.NotEqual(default(DateTime), purchase.Timestamp);
        Assert.Contains(purchase.Currency, PurchaseGenerator.Currencies);
        Assert.True(purchase.Amount > 0);
    }

    [Fact]
    public void GenerateRandomPurchase_GeneratesUniquePurchases()
    {
        // Arrange
        int numberOfPurchases = 100;
        Purchase[] purchases = new Purchase[numberOfPurchases];

        // Act
        for (int i = 0; i < numberOfPurchases; i++)
        {
            purchases[i] = _purchaseGenerator.GenerateRandomPurchase();
        }

        // Assert
        int uniqueIds = purchases.Select(p => p.Id).Distinct().Count();
        int uniqueTimestamps = purchases.Select(p => p.Timestamp).Distinct().Count();

        Assert.Equal(numberOfPurchases, uniqueIds);
        Assert.Equal(numberOfPurchases, uniqueTimestamps);
    }
}
