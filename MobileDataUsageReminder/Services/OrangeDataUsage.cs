using System;
using System.Collections.Generic;
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

        public OrangeDataUsage(IApplicationConfiguration applicationConfiguration,
                               ILogger<OrangeDataUsage> logger,
                               IProviderGateway providerGateway)
        {
            _applicationConfiguration = applicationConfiguration;
            _logger = logger;
            _providerGateway = providerGateway;
        }

        public async Task<List<MobileDataPackage>> GetMobileDataPackages()
        {
            await _providerGateway.Login(_applicationConfiguration.ProviderEmail, _applicationConfiguration.ProviderPassword);

            await _providerGateway.GetClient();

            var dataProducts = await _providerGateway.GetMobileDataProducts();

            var mobileDataPackages = new List<MobileDataPackage>();

            foreach (var dataProduct in dataProducts)
            {
                var dataUsages = _providerGateway.GetDataUsage(dataProduct);
            }
        }
    }
}
