using System;
using System.Collections;
using System.Collections.Generic;

namespace TwitterAPI.Models
{
    public class TProfile
    {
        public int ListID { get; set; }
        //from twitter user
        public long ProfileID { get; set; }
        public string Name { get; set; }
        public string ScreenName { get; set; }
        public string ProfileUrl { get; set; }
        public string Website { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public int Followers { get; set; }// also considered as reach
        public int TimelineCount { get; set; }
        public int CalculatedTweets { get; set; }
        public int QueryIncludedTweets { get; set; }
        public int RetweetedTweets { get; set; }

        public bool IsVerified { get; set; }
        public string Image { get; set; }
        //from algorithm
        public double Impact { get; set; }
        public bool Profetional { get; set; }       
        public int OriginalTweets { get; set; }
        public double TweetsEngagmentRate { get; set; }

        public double Engagment { get; set; }
        public double GeneralActivity { get; set; }
        public double Rank { get; set; }
        
       // public ProfileStackTrace StackTrace {get;set;}
        
        //category will obligate relation table in DB
        //public List<string> Categories { get; set; }

        public TProfile()
        {
            Rank = 0;
            Impact = 0;
            Engagment = 0;
            OriginalTweets = 0;
            GeneralActivity = 0;
            TweetsEngagmentRate = 0;
            Profetional = false;
            
        }

        public TProfile(long id,string name,string screenName,bool verified)
        {
            ProfileID = id;
            Name = name;
            ScreenName = screenName;
            Profetional = false;
            IsVerified = verified;

        }


        public bool ProfileEquals(long id)
        {
            if (id == ProfileID)
                return true;
            return false;
        }
    }
    

}