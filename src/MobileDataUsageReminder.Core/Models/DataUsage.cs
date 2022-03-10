public class DataUsage
{
    public string Unit { get; set; } = string.Empty;
    public int InitialAmount { get; set; }
    public float UsedAmount { get; set; }
    public float RemainingAmount { get; set; }
    public TelegramUser TelegramUser { get; set; }
}
