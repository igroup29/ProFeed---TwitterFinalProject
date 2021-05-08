using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwitterAPI.Models
{
    public class ProfileStackTrace
    {
        //{ "Number of tweets in timeline:", "Calculated Tweets:", "Number of Tweets contaning query:", "Number of ReTweeted tweets:" };
        public int Number_of_tweets_in_timeline { get; set; }
        public int Calculated_Tweets { get; set; }
        public int Number_of_Tweets_contaning_query { get; set; }
        public int Number_of_ReTweeted_tweets { get; set; }

       /* override public string ToString()
        {
            return "Number of tweets in timeline:"+ Number_of_tweets_in_timeline +""+"Calculated Tweets:", "Number of Tweets contaning query:", "Number of ReTweeted tweets:" };

        }*/

    }
}