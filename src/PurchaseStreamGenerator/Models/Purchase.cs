using System;

public class Purchase
{
    public Guid Id { get; set; }
    public DateTime Timestamp { get; set; }
    public string Currency { get; set; }
    public decimal Amount { get; set; }
}
