using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Threading;

namespace twitterAPI
{
   static public class driver
    {
        public static IWebDriver Driver { get; set;}
        public static IWebDriver Driver2 { get; set; }
        public static IWebDriver Driver3 { get; set; }
        public static IWebDriver Driver4 { get; set; }
        public static IWebDriver Driver5 { get; set; }
        public static string loginUserName { get; set; }

        public static List<IWebDriver> kullan�yorum = new List<IWebDriver>();

      
        public static IWebDriver MusaitOlanDriver()
        {
             IWebDriver[] drivler = { Driver2, Driver3, Driver4, Driver5 };
            foreach (IWebDriver item in drivler)
            {
                if (!kullan�yorum.Contains(item))
                {
                    kullan�yorum.Add(item);
                    return item;
                }
            }
            Thread.Sleep(50);
            return MusaitOlanDriver();
        }
    }
}
