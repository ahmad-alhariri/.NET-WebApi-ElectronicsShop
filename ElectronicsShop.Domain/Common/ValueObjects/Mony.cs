namespace ElectronicsShop.Domain.Common.ValueObjects;

public record Money
{
    public decimal Amount { get; }
    public string Currency { get; } = "USD";

    public Money(decimal amount, string currency = "USD")
    {
        Amount = amount;
        Currency = currency.ToUpper().Trim();
    }

    // Operator overloads and helper methods
    public static Money operator +(Money a, Money b) => new(a.Amount + b.Amount, a.Currency);
    public static Money operator -(Money a, Money b) => new(a.Amount - b.Amount, a.Currency);
    
    public override string ToString() => $"{Amount:C} ({Currency})";
}