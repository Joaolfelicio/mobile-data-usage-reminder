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
    public class ProviderDataUsageService : IProviderDataUsageService
    {
        private readonly IApplicationConfiguration _applicationConfiguration;
        private readonly IProviderGateway _providerGateway;
        private readonly ITelegramApiConfiguration _telegramApiConfiguration;
        private readonly IMapperService _mapperService;

        public ProviderDataUsageService(IApplicationConfiguration applicationConfiguration,
                               IProviderGateway providerGateway,
                               ITelegramApiConfiguration telegramApiConfiguration,
                               IMapperService mapperService)
        {
            _applicationConfiguration = applicationConfiguration;
            _providerGateway = providerGateway;
            _telegramApiConfiguration = telegramApiConfiguration;
            _mapperService = mapperService;
        }

        public async Task<List<MobileData>> GetMobileData()
        {
            var providerCred = new ProviderCredentials(_applicationConfiguration.ProviderEmail, _applicationConfiguration.ProviderPassword);

            var dataUsages = await _providerGateway.GetDataUsages(providerCred, _telegramApiConfiguration.TelegramUsers);

            var mobileDatas = new List<MobileData>();

            foreach (var dataUsage in dataUsages)
            {
                mobileDatas.Add(_mapperService.MapMobileData(dataUsage));
            }

            return mobileDatas;
        }

    }
}
