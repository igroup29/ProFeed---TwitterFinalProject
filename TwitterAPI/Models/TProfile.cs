﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwitterAPI.Models
{
    public class TProfile
    {
        public long ProfileID { get; set; }
        public string Name { get; set; }
        public string ScreenName { get; set; }
        public string ProfileUrl { get; set; }
        public string Website { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public bool Profetional { get; set; }
        public int ListID { get; set; }
        public double Rank { get; set; }
        public int Followers { get; set; }
        public bool IsVerified { get; set; }
        public double Reach { get; set; }
        public double Impact { get; set; }
        public double Engagment { get; set; }
        public string Image { get; set; }
        public ArrayList StackTrace {get;set;}
        //category will obligate relation table in DB
        //public List<string> Categories { get; set; }

        public TProfile()
        {
            Rank = 0;
            Reach = 0;
            Impact = 0;
            Engagment = 0;
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