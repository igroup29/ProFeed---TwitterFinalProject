using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Parameters;
using Tweetinvi.Models;
using Tweetinvi.Parameters.Enum;
using Tweetinvi.Parameters.V2;

namespace TwitterAPI.Models
{
    public class ProFeedAlg
    {
        private ITweet[] tweets;

        public ProFeedAlg(ITweet[] tc)
        {
            System.Threading.Thread.Sleep(1000);
            tweets = tc;
        }

    
       



    }
}