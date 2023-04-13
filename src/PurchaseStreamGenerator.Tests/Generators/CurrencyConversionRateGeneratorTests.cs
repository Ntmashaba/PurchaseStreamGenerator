public class CurrencyConversionRateGeneratorTests
{
    private readonly CurrencyConversionRateGenerator _generator;

    public CurrencyConversionRateGeneratorTests()
    {
        _generator = new CurrencyConversionRateGenerator();
    }

    [Fact]
    public void GenerateCurrencyConversionRates_ReturnsCorrectNumberOfRates()
    {
        // Arrange
        var timestamp = DateTimeOffset.UtcNow;

        // Act
        var rates = _generator.GenerateCurrencyConversionRates(timestamp);

        // Assert
        Assert.Equal(5, rates.Count);
    }

    [Fact]
    public void GenerateCurrencyConversionRates_ReturnsValidConversionRates()
    {
        // Arrange
        var timestamp = DateTimeOffset.UtcNow;

        // Act
        var rates = _generator.GenerateCurrencyConversionRates(timestamp);

        // Assert
        foreach (var rate in rates)
        {
            Assert.InRange(rate.ConversionRateToUSD, 0.5m, 1.5m);
            Assert.True(Math.Abs((rate.Timestamp - timestamp).TotalSeconds) < 1);
        }
    }
}
