using System.Collections.Generic;

public interface ITelegramApiConfiguration
{
    public List<TelegramUser> TelegramUsers { get; set; }
    string ApiEndPoint { get; }
    string AccessToken { get; }
}
