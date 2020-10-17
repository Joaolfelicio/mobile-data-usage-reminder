using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MobileDataUsageReminder.Configurations.Contracts;
using MobileDataUsageReminder.Infrastructure.Contracts;
using MobileDataUsageReminder.Models;
using MobileDataUsageReminder.Services.Contracts;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;

namespace MobileDataUsageReminder.Services
{
    class OrangeDataUsage : IProviderDataUsage
    {
        private readonly IApplicationConfiguration _applicationConfiguration;
        private readonly ILogger<OrangeDataUsage> _logger;
        private readonly IProviderGateway _providerGateway;
        private readonly ITelegramApiConfiguration _telegramApiConfiguration;

        public OrangeDataUsage(IApplicationConfiguration applicationConfiguration,
                               ILogger<OrangeDataUsage> logger,
                               IProviderGateway providerGateway,
                               ITelegramApiConfiguration telegramApiConfiguration)
        {
            _applicationConfiguration = applicationConfiguration;
            _logger = logger;
            _providerGateway = providerGateway;
            _telegramApiConfiguration = telegramApiConfiguration;
        }

        public async Task<List<MobileDataPackage>> GetMobileDataPackages()
        {
            await _providerGateway.Login(_applicationConfiguration.ProviderEmail, _applicationConfiguration.ProviderPassword);

            await _providerGateway.GetClient();

            // Get the phone numbers to fetch
            var phoneNumbers = _telegramApiConfiguration.TelegramUsers
                .Select(x => x.PhoneNumber)
                .ToList();

            var dataProducts = await _providerGateway.GetMobileDataProducts(phoneNumbers);

            var mobileDataPackages = new List<MobileDataPackage>();

            foreach (var dataProduct in dataProducts)
            {
                var dataUsage = await _providerGateway.GetDataUsage(dataProduct);

                var chatId = _telegramApiConfiguration.TelegramUsers
                    .First(x => x.PhoneNumber == dataProduct.PhoneNumber).ChatId;

                var usedAmount = Convert.ToInt32(dataUsage.UsedAmount);
                var initialAmount = Convert.ToInt32(dataUsage.InitialAmount);
                var usedPercentage = (usedAmount * 100) / initialAmount;

                var roundedUsedPercentage = Convert.ToInt32(Math.Round(usedPercentage / 10.0) * 10);

                var mobileDataPackage = new MobileDataPackage()
                {
                    PhoneNumber = dataProduct.PhoneNumber,
                    FullDate = DateTime.Now,
                    Day = DateTime.Now.Day,
                    Month = DateTime.Now.ToString("MMMM"),
                    Year = DateTime.Now.Year,
                    Unit = dataUsage.Unit,
                    InitialAmount = dataUsage.InitialAmount,
                    UsedAmount = dataUsage.UsedAmount,
                    RemainingAmount = dataUsage.RemainingAmount,
                    ChatId = chatId,
                    UsedPercentage = roundedUsedPercentage
                };
            }

            return mobileDataPackages;
        }
    }
}
