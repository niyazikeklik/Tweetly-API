using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.Threading;
using Newtonsoft.Json;

namespace twitterAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {

        static void Control(IWebDriver calismaDriver, string link, string kullaniciid)
        {
            try
            {
                if (calismaDriver.FindElements(By.CssSelector("[data-testid=login]")).Count > 0)
                    calismaDriver.Navigate().Refresh();

                while (calismaDriver.FindElements(By.XPath("//a[@href='/" + kullaniciid + "/followers']")).Count == 0)
                {
                    if (calismaDriver.FindElements(By.CssSelector("[data-testid=emptyState]")).Count > 0) break;
                    if (calismaDriver.Url.Contains("limit"))
                    {
                        Thread.Sleep(300000);
                        calismaDriver.Navigate().GoToUrl(link);
                        Control(calismaDriver, link, kullaniciid);
                    }
                    Thread.Sleep(5);
                }
            }
            catch (Exception)
            {
                Thread.Sleep(300000);
                calismaDriver.Navigate().GoToUrl(link);
                Control(calismaDriver, link, kullaniciid);
            }

        }

        user getUser(IWebDriver calismaDriver, string kullaniciid, bool fast)
        {
            List<Task> tasks = new List<Task>();
            getUser newgetUser = new getUser();
            string link = "";
        yeniden:
            try
            {
                link = "https://mobile.twitter.com/" + kullaniciid;
                calismaDriver.Navigate().GoToUrl(link);
            }
            catch { goto yeniden; }
            Control(calismaDriver, link, kullaniciid);

            newgetUser.profil.username = kullaniciid;

            Task g5 = Task.Run(() => newgetUser.getDate(calismaDriver));
            tasks.Add(g5);

            Task g6 = Task.Run(() => newgetUser.getLocation(calismaDriver));
            tasks.Add(g6);

            Task g13 = Task.Run(() => newgetUser.getPPhotoURL(calismaDriver, kullaniciid));
            tasks.Add(g13);

            Task g7 = Task.Run(() => newgetUser.getFollowing(calismaDriver, kullaniciid));
            tasks.Add(g7);

            Task g8 = Task.Run(() => newgetUser.getFollowers(calismaDriver, kullaniciid));
            tasks.Add(g8);

            Task g1 = Task.Run(() => newgetUser.getName(calismaDriver));
            tasks.Add(g1);

            Task g2 = Task.Run(() => newgetUser.IsFollowers(calismaDriver));
            tasks.Add(g2);

            Task g3 = Task.Run(() => newgetUser.getfollowStatus(calismaDriver));
            tasks.Add(g3);

            Task g4 = Task.Run(() => newgetUser.getBio(calismaDriver));
            tasks.Add(g4);

            Task.WaitAny(g5);
            Task g11 = Task.Run(() => newgetUser.getTweetCount(calismaDriver));
            tasks.Add(g11);


            if (!fast)
            {

                Task.WaitAny(g11);
                Task g12 = Task.Run(() => newgetUser.getLastTweetsDateAVC(calismaDriver, 1));
                tasks.Add(g12);

                Task.WaitAny(g1);
                Task.WaitAny(g12);

                try
                {
                    calismaDriver.FindElement(By.XPath("//a[@href='/" + kullaniciid + "/likes']")).Click();

                    Task g9 = Task.Run(() => newgetUser.getLikeCount(calismaDriver));
                    tasks.Add(g9);

                    Task.WaitAny(g9);
                    Task g10 = Task.Run(() => newgetUser.getLastTweetsDateAVC(calismaDriver, 2));
                    tasks.Add(g10);
                }
                catch {; }
            }

            Task.WaitAll(tasks.ToArray());
            driver.kullanıyorum.Remove(calismaDriver);
            return newgetUser.profil;
        }

        [HttpGet("{kullaniciid}/{fast}")]
        public user Get(string kullaniciid, bool fast = false)
        {
            IWebDriver driverr = driver.Driver;
            var result = getUser(driverr, kullaniciid, fast);
            return result;
        }




        [HttpGet("GetFollowingUsers/{kullaniciid}/{fast}/{count}")]
        public string GetFollowingUsers(string kullaniciid, bool fast = false, int count = 9999999)
        {
            IWebDriver driverr = driver.Driver;

            string link = "https://mobile.twitter.com/" + kullaniciid + "/following";
            driverr.Navigate().GoToUrl(link);
            Thread.Sleep(1500);
            List<user> takipciler = new List<user>();
            List<IWebElement> kontrolEdildi = new List<IWebElement>();


            while (!isSayfaSonu(driverr))
            {
                var listelenenKullanicilar = driverr.FindElements(By.CssSelector("[data-testid=UserCell]"));
                var kontrolEdilecekler = listelenenKullanicilar.Except(kontrolEdildi);


                foreach (var item in kontrolEdilecekler)
                {
                    if (takipciler.Count >= count) break;
                    string followingUserName = "";
                    try
                    {
                        followingUserName = item.FindElements(By.TagName("a"))[1].GetAttribute("href");
                        followingUserName = followingUserName.Substring(followingUserName.LastIndexOf('/') + 1);

                        IWebDriver calismadriver = driver.MusaitOlanDriver();
                        Thread th = new Thread(new ThreadStart(() =>
                        {
                            var result = getUser(calismadriver, followingUserName, fast);
                            takipciler.Add(result);

                        }));
                        th.Priority = ThreadPriority.Highest;
                        th.Start();

                    }
                    catch (Exception) { continue; }
                }

                if (takipciler.Count >= count) break;
                kontrolEdildi.AddRange(kontrolEdilecekler);
            }

            int countt = 1;
            takipciler.All(x => { x.count = countt++; return true; });
            var json = JsonConvert.SerializeObject(takipciler);
            return json;
        }
        bool isSayfaSonu(IWebDriver driverr)
        {
            try
            {
                IJavaScriptExecutor jse = (IJavaScriptExecutor)driverr;
                double oncekiY = Convert.ToDouble(jse.ExecuteScript("return window.scrollY;"));
                double sonrakiY = Convert.ToDouble(jse.ExecuteScript("window.scrollBy(0,1500); return window.scrollY;"));

                if (oncekiY == sonrakiY) return true;
                else return false;
            }
            catch (Exception)
            {
                return isSayfaSonu(driverr);
            }

        }
    }
}
