using System;
using MobileDataUsageReminder.Services;

namespace MobileDataUsageReminder
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var driver = new OrangeDataUsage();

            driver.GetMobileDataUsage();
        }
    }
}
