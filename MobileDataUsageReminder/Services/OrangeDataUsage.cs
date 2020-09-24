using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using MobileDataUsageReminder.Configurations.Contracts;
using MobileDataUsageReminder.Models;
using MobileDataUsageReminder.Services.Contracts;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace MobileDataUsageReminder.Services
{
    class OrangeDataUsage : IProviderDataUsage
    {
        private readonly IApplicationConfiguration _applicationConfiguration;

        public OrangeDataUsage(IApplicationConfiguration applicationConfiguration)
        {
            _applicationConfiguration = applicationConfiguration;
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

                var phoneNumber = plan.FindElement(By.CssSelector("h2.pageSubTitle span")).Text.Substring(3);
                var dataUsedPercentage = 100 - int.Parse(Regex.Match(plan.FindElement(By.CssSelector(".chart-bar")).GetAttribute("style"), @"\d+\.*\d*").Value);
                dataUsedPercentage = dataUsedPercentage == 0 ? 1 : dataUsedPercentage;

                //Fix this to css selectors
                var monthlyDataGb = int.Parse(plan
                    .FindElement(By.CssSelector("usage-consumption > div > div > div:nth-child(5)"))
                    .Text
                    .Split(" ")[0]);

                dataUsages.Add(new DataUsage
                {
                    FullDate = DateTime.Now,
                    Day = DateTime.Now.Day,
                    Month = DateTime.Now.ToString("MMMM"),
                    Year = DateTime.Now.Year,
                    PhoneNumber = phoneNumber,
                    DataUsedPercentage = Convert.ToInt32(Math.Round(dataUsedPercentage / 10.0) * 10),
                    MonthlyDataGb = monthlyDataGb
                });
            }

            return dataUsages;
        }

    }

}
