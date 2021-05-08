using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwitterAPI.Models
{
    public class ProfileStackTrace
    {
        //{ "Number of tweets in timeline:", "Calculated Tweets:", "Number of Tweets contaning query:", "Number of ReTweeted tweets:" };
        public int TimelineTweets { get; set; }
        public int CalculatedTweets { get; set; }
        public int QueryIncludedTweets { get; set; }
        public int RetweetedTweets { get; set; }

       /* override public string ToString()
        {
            return "Number of tweets in timeline:"+ Number_of_tweets_in_timeline +""+"Calculated Tweets:", "Number of Tweets contaning query:", "Number of ReTweeted tweets:" };

        }*/

    }
}