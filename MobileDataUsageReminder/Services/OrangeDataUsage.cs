using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using MobileDataUsageReminder.Configurations.Contracts;
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

        public OrangeDataUsage(IApplicationConfiguration applicationConfiguration,
                               ILogger<OrangeDataUsage> logger)
        {
            _applicationConfiguration = applicationConfiguration;
            _logger = logger;
        }

        public List<DataUsage> GetMobileDataUsage()
        {
            var authDriver = LoginWebDriver();

            var dataUsages = GetDataUsages(authDriver);

            authDriver.Close();

            return dataUsages;
        }

        private IWebDriver LoginWebDriver()
        {
            var webDriver = new ChromeDriver();
            webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            webDriver.Navigate().GoToUrl("https://client.orange.lu/selfcare/login");

            webDriver.FindElement(By.Id("userName")).SendKeys(_applicationConfiguration.ProviderEmail);
            webDriver.FindElement(By.Id("password")).SendKeys(_applicationConfiguration.ProviderPassword);
            webDriver.FindElement(By.CssSelector(".login-container form")).Submit();

            return webDriver;
        }

        private List<DataUsage> GetDataUsages(IWebDriver authDriver)
        {
            var dataUsages = new List<DataUsage>();

            //Smoothly scroll to bottom to load everything
            for (int i = 0; i < 6000; i++)
            {
                ((IJavaScriptExecutor)authDriver).ExecuteScript("window.scrollBy(0,2)", "");
            }

            var plans = authDriver.FindElements(By.CssSelector(".box-subscription > .container"));

            foreach (var plan in plans)
            {
                //If the subscription box is for the Device Advantage
                if (!plan.FindElement(By.CssSelector("h2.pageSubTitle")).Text.Contains("Device Advantage")) continue;

                //Get the phone number of the subscription
                var phoneNumber = plan.FindElement(By.CssSelector("h2.pageSubTitle span")).Text.Substring(3);

                //Get the second chart bar display (the second one is the one referring to the used data), so we can extract the width % (as it is the data used percentage)
                var charBarStyle = plan.FindElement(By.CssSelector(".chart-bar:nth-child(2)")).GetAttribute("style");

                //From the style, get the width percentage formatted to int, but as string
                var dataUsageString = Regex.Match(charBarStyle, @"\d+\.*\d*").Value;
                
                //Parse it to int so we have the used data percentage
                var dataUsedPercentage = int.Parse(dataUsageString);

                //If the dataUsedPercentage is 0, assign it 1, so we can make the needed calculus
                dataUsedPercentage = dataUsedPercentage == 0 ? 1 : dataUsedPercentage;

                //Get the Monthly Data Gigabytes
                var monthlyDataGb = int.Parse(plan
                    .FindElement(By.CssSelector("usage-consumption > div > div > div:nth-child(5)"))
                    .Text
                    .Split(" ")[0]);

                var currentDataUsage = new DataUsage
                {
                    FullDate = DateTime.Now,
                    Day = DateTime.Now.Day,
                    Month = DateTime.Now.ToString("MMMM"),
                    Year = DateTime.Now.Year,
                    PhoneNumber = phoneNumber,
                    DataUsedPercentage = Convert.ToInt32(Math.Round(dataUsedPercentage / 10.0) * 10),
                    MonthlyDataGb = monthlyDataGb
                };

                dataUsages.Add(currentDataUsage);

                _logger.LogInformation($"Current data usage is at {currentDataUsage.DataUsedPercentage}% of {currentDataUsage.MonthlyDataGb} GB " +
                                        $"for number {currentDataUsage.PhoneNumber}.");
            }
            return dataUsages;
        }
    }
}
