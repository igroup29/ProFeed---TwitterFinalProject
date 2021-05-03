﻿using System;
using System.Collections;


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
        public bool IsVerified { get; set; }
        public string Image { get; set; }
        //from algorithm
        public double Impact { get; set; }
        public bool Profetional { get; set; }       
        public int OriginalTweets { get; set; }
        public double TweetsEngagmentRate { get; set; }

        public Double Engagment { get; set; }
        public Double GeneralActivity { get; set; }
        public double Rank { get; set; }

        public ArrayList StackTrace {get;set;}
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
            StackTrace = new ArrayList();
        }

        public TProfile(long id,string name,string screenName,bool verified)
        {
            ProfileID = id;
            Name = name;
            ScreenName = screenName;
            Profetional = false;
            IsVerified = verified;
            StackTrace = new ArrayList();

        }

    }
}