public class TestablePurchaseGenerator : PurchaseGenerator
{
    public Func<Purchase> GenerateRandomPurchaseFunc { get; set; }
    public int GenerateRandomPurchaseCallCount { get; private set; }

    public override Purchase GenerateRandomPurchase()
    {
        GenerateRandomPurchaseCallCount++;
        return GenerateRandomPurchaseFunc();
    }
}