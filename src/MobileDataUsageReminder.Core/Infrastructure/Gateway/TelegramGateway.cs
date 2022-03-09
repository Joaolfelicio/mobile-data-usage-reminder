using System.Net.Http.Json;
using Microsoft.Extensions.Logging;

public class TelegramGateway : INotificationGateway
{
    private readonly ITelegramApiConfiguration _telegramApiConfiguration;
    private readonly ILogger<ITelegramApiConfiguration> _logger;
    private readonly HttpClient _httpClient;

    public TelegramGateway(
        ITelegramApiConfiguration telegramApiConfiguration,
        ILogger<ITelegramApiConfiguration> logger,
        HttpClient httpClient)
    {
        _telegramApiConfiguration = telegramApiConfiguration;
        _logger = logger;
        _httpClient = httpClient;
    }

    /// <summary>
    /// Sends the post to API reminder.
    /// </summary>
    /// <param name="mobileData">The mobile data package.</param>
    public async Task SendNotification(MobileData mobileData)
    {
        var reminder = new TelegramReminder
        {
            ChatId = mobileData.ChatId,
            ParseMode = "HTML",
            Text = "Mobile Data Usage Reminder: Your mobile data plan has reached " +
                   $"<strong>{mobileData.UsedPercentage}%</strong> of the total of <em>{mobileData.InitialAmount}{mobileData.Unit}</em> " +
                   $"that you have for the month of {mobileData.Month}."
        };

        var urlTelegramMessage = $"{_telegramApiConfiguration.ApiEndPoint}+{_telegramApiConfiguration.AccessToken}/sendMessage";

        var result = await _httpClient.PostAsJsonAsync(urlTelegramMessage, reminder);
        result.EnsureSuccessStatusCode();

        _logger.LogInformation("Successfully sent reminder to {phone number}", mobileData.PhoneNumber);
    }
}