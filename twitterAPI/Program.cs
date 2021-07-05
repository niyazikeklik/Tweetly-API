using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace twitterAPI
{
    public class Program
    {
        static IWebDriver optionDriver(string un)
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--headless");
            chromeOptions.EnableMobileEmulation("Pixel 2 XL");
            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;
          
            IWebDriver driver = new ChromeDriver(service, chromeOptions);
            driver.Manage().Window.Size = new Size(400, 820);
            
            driver.Navigate().GoToUrl("https://mobile.twitter.com/login");
            Thread.Sleep(2500);
            
            driver.FindElement(By.Name("session[username_or_email]")).SendKeys(un);
            driver.FindElement(By.Name("session[password]")).SendKeys("Galatasaray1453");
            driver.FindElement(By.CssSelector("[data-testid=LoginForm_Login_Button]")).Click();
            return driver;
        }
        static void createDriver(string un)
        {
            IWebDriver driver = optionDriver(un);
            twitterAPI.driver.Driver = driver;
        }
        static void createDriver2(string un)
        {
            IWebDriver driver = optionDriver(un);
            twitterAPI.driver.Driver2 =driver ;
        }
        static void createDriver3(string un)
        {
            IWebDriver driver = optionDriver(un);
            twitterAPI.driver.Driver3 = driver;
        }
        static void createDriver4(string un)
        {
            IWebDriver driver = optionDriver(un);
            twitterAPI.driver.Driver4 = driver;
        }
        static void createDriver5(string un)
        {
            IWebDriver driver = optionDriver(un);
            twitterAPI.driver.Driver5 = driver;
        }
        public static void Main(string[] args)
        {
            string userName = "alienationxs";
            Process[] runingProcess = Process.GetProcesses();
            for (int i = 0; i < runingProcess.Length; i++)
            {
                if (runingProcess[i].ProcessName == "chrome" || runingProcess[i].ProcessName == "chromedriver" || runingProcess[i].ProcessName == "conhost")
                {
                    double s = (DateTime.Now - runingProcess[i].StartTime).TotalSeconds;
                    if (s >= 30) { runingProcess[i].Kill(); }
                }

            }

            twitterAPI.driver.loginUserName = userName;
            Task.Run(() => createDriver(userName));
            Task.Run(() => createDriver2(userName));
            Task.Run(() => createDriver3(userName));
            Task.Run(() => createDriver4(userName));
            Task.Run(() => createDriver5(userName));

            
            CreateHostBuilder(args).Build().Run();
        
            
        }
        static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            ;
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
       
    }
}
