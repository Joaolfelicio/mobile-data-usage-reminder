public class TelegramApiConfiguration : ITelegramApiConfiguration
{
    public List<TelegramUser> TelegramUsers { get; set; }
    public string ApiEndPoint { get; set; }
    public string AccessToken { get; set; }
}
