public class MobileData
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string PhoneNumber { get; init; }
    public string ChatId { get; init; }
    public string Unit { get; init; }
    public int InitialAmount { get; init; }
    public int UsedAmount { get; init; }
    public int RemainingAmount { get; init; }
    public double UsedPercentage { get; init; }
    public DateTime FullDate { get; init; }
    public int Day { get; init; }
    public string Month { get; init; }
    public int Year { get; init; }
}
