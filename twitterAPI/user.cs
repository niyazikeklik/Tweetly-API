using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace twitterAPI
{
    public class user
    {

        public int count { get; set; }
        public string name { get; set; }

        public string username { get; set; }
        public string photoURL { get; set; }

        public int followers { get; set; }
        public int following { get; set; }

        public double date { get; set; }
        public string location { get; set; }

        public int tweetCount { get; set; }
        public int likeCount { get; set; }
        public double lastTweetsDate { get; set; }
        public double lastLikesDate { get; set; }

        public string followStatus { get; set; }
        public bool followersStatus { get; set; }
        public string tweetSikligi { get; set; }
        public string begeniSikligi { get; set; }
        public string bio { get; set; }

    }
}
