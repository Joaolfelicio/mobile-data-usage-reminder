using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MobileDataUsageReminder.Configurations.Contracts;
using MobileDataUsageReminder.DAL.Models;
using MobileDataUsageReminder.Infrastructure.Contracts;
using MobileDataUsageReminder.Models;
using MobileDataUsageReminder.Services.Contracts;


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

        /// <summary>
        /// Gets the mobile data packages.
        /// </summary>
        /// <returns></returns>
        public async Task<List<MobileData>> GetMobileData()
        {
            await _providerGateway.Login(_applicationConfiguration.ProviderEmail, _applicationConfiguration.ProviderPassword);

            await _providerGateway.GetClient();

            // Get the phone numbers to fetch
            var phoneNumbers = _telegramApiConfiguration.TelegramUsers
                .Select(x => x.PhoneNumber)
                .ToList();

            var dataProducts = await _providerGateway.GetMobileDataProducts(phoneNumbers);

            var mobileDataPackages = new List<MobileData>();

            foreach (var dataProduct in dataProducts)
            {
                var dataUsage = await _providerGateway.GetDataUsage(dataProduct);

                var chatId = _telegramApiConfiguration.TelegramUsers
                    .First(x => x.PhoneNumber == dataProduct.PhoneNumber).ChatId;

                var truncatedUsedAmount =
                    dataUsage.UsedAmount.Substring(0, dataUsage.UsedAmount.IndexOf(".", StringComparison.Ordinal));

                var usedPercentage = int.Parse(truncatedUsedAmount) * 100 / int.Parse(dataUsage.InitialAmount);

                var roundedUsedPercentage = Convert.ToInt32(Math.Round(usedPercentage / 10.0) * 10);

                mobileDataPackages.Add(new MobileData()
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
                });
            }

            return mobileDataPackages;
        }
    }
}
