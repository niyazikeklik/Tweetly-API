using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace twitterAPI
{
    public class getUser
    {
        public user profil = new user();


        public Task getName(IWebDriver driverr)
        {
            try
            {
                profil.name = driverr.FindElement(By.XPath("//header[@role='banner']")).Text.Split("\r\n")[0];
            }
            catch (Exception)
            {
                ;
            }
            return Task.CompletedTask;
        }
        public Task getTweetCount(IWebDriver driverr)
        {
            try
            {
                string tweetcount = driverr.FindElement(By.XPath("//header[@role='banner']")).Text;

                tweetcount = tweetcount.Split("\r\n")[1];
                tweetcount = tweetcount.Replace("Tweets", "").Replace("Tweet", "").Replace("Tweetler", "");
                tweetcount = yardimci.Donustur(tweetcount);

                profil.tweetCount = Convert.ToInt32(tweetcount);
                profil.tweetCount = profil.tweetCount > 1 ? profil.tweetCount : profil.tweetCount + 1;
                double gunluktweet = (profil.tweetCount / profil.date);
                if (gunluktweet >= 1) profil.tweetSikligi = "Günde '" + Math.Round(gunluktweet, 1) + "' tweet atıyor";
                else profil.tweetSikligi = "-" + Math.Round((1 / gunluktweet), 0) + "- Günde bir tweet atıyor";
            }
            catch (Exception)
            {

                ;
            }
            return Task.CompletedTask;
        }
        public Task getDate(IWebDriver driverr)
        {
            try
            {
                var cocuklar = driverr.FindElement(By.CssSelector("[data-testid=UserProfileHeader_Items]")).FindElements(By.TagName("span"));
                profil.date = yardimci.KayıtTarihi(cocuklar[cocuklar.Count - 1].Text);
            }
            catch (Exception)
            {

                ;
            }
            return Task.CompletedTask;

        }
        public Task getLocation(IWebDriver driverr)
        {
            try
            {
                string res = driverr.FindElement(By.CssSelector("[data-testid=UserProfileHeader_Items]")).FindElements(By.TagName("span"))[0].Text;
                if (!res.Contains("Doğum") && !res.Contains("Birthday") && !res.Contains("Born") && !res.Contains("Joined") && !res.Contains("tarih")) profil.location = res;
            }
            catch (Exception)
            {
                ;
            }
            return Task.CompletedTask;
        }

        public Task getFollowing(IWebDriver driverr, string ka)
        {
            try
            {
                string countText = driverr.FindElement(By.XPath("//a[@href='/" + ka + "/following']")).Text;
                countText = countText.Replace(" Takip edilen", "").Replace(" Following", "");
                countText = yardimci.Donustur(countText);
                profil.following = Convert.ToInt32(countText);
            }
            catch (Exception)
            {

                ;
            }
            return Task.CompletedTask;
        }
        public Task getFollowers(IWebDriver driverr, string ka)
        {
            try
            {

                string countText = driverr.FindElement(By.XPath("//a[@href='/" + ka + "/followers']")).Text;
                countText = countText.Replace(" Takipçi", "").Replace(" Followers", "");
                countText = yardimci.Donustur(countText);
                profil.followers = Convert.ToInt32(countText);
            }
            catch (Exception)
            {

                ;
            }
            return Task.CompletedTask;
        }

        public Task getLastTweetsDateAVC(IWebDriver driverr, int secenek)
        {
            if(profil.tweetCount < 100 && secenek == 1)
            {
                profil.lastTweetsDate = Math.Round((profil.date / (profil.tweetCount)), 2);
            }
            else if(profil.likeCount < 1000 && secenek == 2)
            {
                profil.lastLikesDate = Math.Round((profil.date / (profil.likeCount+1)), 2);
            }
            else
            {
                double toplamGunSayisi = 0;
                List<IWebElement> sonTweetler = driverr.FindElements(By.CssSelector("[data-testid=tweet]")).ToList();
                int count = 0;
                while (sonTweetler.Count < 1)
                {
                    sonTweetler = driverr.FindElements(By.CssSelector("[data-testid=tweet]")).ToList();
                    if (count > 100) break;
                    else Thread.Sleep(10);
                    count++;
                }
                if (sonTweetler.Count > 0)
                {
                    try
                    {
                        if (driverr.FindElements(By.CssSelector("[data-testid=socialContext]")).Count > 0) sonTweetler.Remove(sonTweetler[0]);

                        foreach (var item in sonTweetler)
                        {
                            string date = item.FindElement(By.TagName("time")).GetAttribute("datetime");
                            double gunSayisi = (DateTime.Today - Convert.ToDateTime(date)).TotalDays;
                            toplamGunSayisi += gunSayisi;
                        }
                        if (secenek == 1)
                            profil.lastTweetsDate = Math.Round((toplamGunSayisi / sonTweetler.Count), 2);
                        else if (secenek == 2)
                            profil.lastLikesDate = Math.Round((toplamGunSayisi / sonTweetler.Count), 2);
                    }
                    catch {; }
                }
            }
            
            return Task.CompletedTask;
        }

        public Task getfollowStatus(IWebDriver driverr)
        {
            try
            {
                profil.followStatus = driverr.FindElement(By.CssSelector("[data-testid=placementTracking]")).Text;
            }
            catch (Exception) {; }
            return Task.CompletedTask;
        }
        public Task IsFollowers(IWebDriver driverr)
        {
            try
            {
                profil.followersStatus = driverr.FindElements(By.CssSelector("div.css-901oao.css-bfa6kz.r-1awozwy.r-1sw30gj.r-z2wwpe.r-14j79pv.r-6koalj.r-1q142lx.r-1qd0xha.r-1enofrn.r-16dba41.r-fxxt2n.r-13hce6t.r-bcqeeo.r-s1qlax.r-qvutc0")).Count > 0;
            }
            catch (Exception)
            {

                ;
            }
            return Task.CompletedTask;
        }

        public Task getBio(IWebDriver driverr)
        {
            try
            {
                profil.bio = driverr.FindElement(By.CssSelector("[data-testid=UserDescription]")).Text;
            }
            catch (Exception) {; }
            return Task.CompletedTask;
        }

        public Task getLikeCount(IWebDriver driverr)
        {
            try
            {
                string lc = driverr.FindElement(By.XPath("//header[@role='banner']")).Text;
                lc = lc.Split("\r\n")[1];
                lc = lc.Replace("Beğeniler", "").Replace("Beğeni", "").Replace("Likes", "").Replace("Like", "");
                profil.likeCount = Convert.ToInt32(yardimci.Donustur(lc));
                profil.likeCount = profil.likeCount > 1 ? profil.likeCount : profil.likeCount + 1;
                double gunluklike = profil.likeCount / profil.date;
                if (gunluklike >= 1) profil.begeniSikligi = "Günde '" + Math.Round(gunluklike, 0) + "' beğeni yapıyor";
                else profil.begeniSikligi = "-" + Math.Round((1 / gunluklike), 1) + "- Günde bir beğeni yapıyor";
            }
            catch (Exception)
            {

                ;
            }
            return Task.CompletedTask;

        }
        public Task getPPhotoURL(IWebDriver driverr, string ka)
        {
            try
            {
                profil.photoURL = driverr.FindElement(By.XPath("//a[@href='/" + ka + "/photo']")).FindElement(By.TagName("img")).GetAttribute("src");
            }
            catch (Exception)
            {

                ;
            }
            return Task.CompletedTask;

        }


    }
}