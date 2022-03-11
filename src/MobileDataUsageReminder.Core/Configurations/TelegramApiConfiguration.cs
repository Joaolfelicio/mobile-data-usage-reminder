public class TelegramApiConfiguration : ITelegramApiConfiguration
{
    public List<TelegramUser> TelegramUsers { get; set; } = new List<TelegramUser>();
    public string ApiEndPoint { get; set; } = string.Empty;
    public string AccessToken { get; set; } = string.Empty;
}
