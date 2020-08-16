using System;
using System.Collections.Generic;
using System.Text;
using MobileDataUsageReminder.Models;
using MobileDataUsageReminder.Services.Contracts;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace MobileDataUsageReminder.Services
{
    class OrangeDataUsage : IProviderDataUsage
    {
        public IWebDriver WebDriver;

        public OrangeDataUsage()
        {
            WebDriver = new ChromeDriver();

            WebDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        public List<DataUsage> GetMobileDataUsage()
        {
            var authDriver = LoginWebDriver();

            return GetDataUsages(authDriver);
        }

        private IWebDriver LoginWebDriver()
        {
            WebDriver.Navigate().GoToUrl("https://client.orange.lu/selfcare/login");

            WebDriver.FindElement(By.Id("userName")).SendKeys("");
            WebDriver.FindElement(By.Id("password")).SendKeys("");
            WebDriver.FindElement(By.CssSelector(".login-container form")).Submit();

            return WebDriver;
        }

        private List<DataUsage> GetDataUsages(IWebDriver authDriver)
        {
            var dataUsages = new List<DataUsage>();

            //Get to bottom of page so it's loads everything
            IJavaScriptExecutor js = (IJavaScriptExecutor)authDriver;
            js.ExecuteScript("window.scrollTo(0,100)");

            var plans = authDriver.FindElements(By.CssSelector(".box-subscription > .container"));

            foreach (var plan in plans)
            {
                //If the subscription box is for the Device Advantage
                if (plan.FindElement(By.CssSelector("h2.pageSubTitle")).Text != "Device Advantage") continue;

                var phoneNumber = plan.FindElement(By.CssSelector("h2.pageSubTitle span")).Text.Substring(2);
                var package = plan.FindElement(By.CssSelector("h2.pageSubTitle div")).Text;
                var dataUsedPercentage = plan.FindElement(By.XPath("//*[@id=\"c\"]/csc-dashboard/div/div[6]/csc-dashboard-postpaid/div/div/div[1]/csc-dashboard-postpaid-mobile/div[2]/dashboard-card/usage-consumption/div[1]/div/div[1]")).GetCssValue("width").ToString();
                var dataUserPercentageStr = double.Parse(dataUsedPercentage.Substring(0, dataUsedPercentage.Length - 2));
                var montlyDataGb = int.Parse(plan
                    .FindElement(By.XPath("//*[@id=\"c\"]/csc-dashboard/div/div[6]/csc-dashboard-postpaid/div/div/div[1]/csc-dashboard-postpaid-mobile/div[2]/dashboard-card/usage-consumption/div[1]/div/div[5]"))
                    .Text
                    .Split(" ")[0]);

                dataUsages.Add(new DataUsage
                {
                    FullDate = DateTime.Now,
                    Day = DateTime.Now.Day,
                    Month = DateTime.Now.ToString("MMMM"),
                    Year = DateTime.Now.Year,
                    PhoneNumber = phoneNumber,
                    Package = package,
                    DataUsedPercentage = Convert.ToInt32(Math.Round(dataUserPercentageStr / 10.0) * 10),
                    MonthlyDataGb = montlyDataGb
                });
            }

            return dataUsages;
        }
    }

}
