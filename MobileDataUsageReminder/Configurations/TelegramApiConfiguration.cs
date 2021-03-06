﻿using System.Collections.Generic;
using MobileDataUsageReminder.Configurations.Contracts;

namespace MobileDataUsageReminder.Configurations
{
    public class TelegramApiConfiguration : ITelegramApiConfiguration
    {
        public List<TelegramUser> TelegramUsers { get; set; }
        public string ApiEndPoint { get; set; }
        public string AccessToken { get; set; }
    }

    public class TelegramUser
    {
        public string PhoneNumber { get; set; }
        public string ChatId { get; set; }
    }
}