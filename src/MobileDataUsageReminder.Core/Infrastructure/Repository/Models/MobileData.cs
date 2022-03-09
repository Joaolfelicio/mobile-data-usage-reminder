using System;

public class MobileData
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string PhoneNumber { get; init; }
    public string ChatId { get; init; }
    public string Unit { get; init; }
    public string InitialAmount { get; init; }
    public string UsedAmount { get; init; }
    public string RemainingAmount { get; init; }
    public int UsedPercentage { get; init; }
    public DateTime FullDate { get; init; }
    public int Day { get; init; }
    public string Month { get; init; }
    public int Year { get; init; }
}