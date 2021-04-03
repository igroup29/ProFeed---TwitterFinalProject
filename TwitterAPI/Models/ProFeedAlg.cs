﻿using System;
using System.Collections.Generic;
using System.Collections;
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
        const int MAXFOLLOWERS = 1000000;
        const int MINFOLLOWERS = 5000;
        private List<IUser> influencers;
        private List<int> influencersDagree;
        private List<int> profetionals;
        private int algoLevel;




        public ProFeedAlg()
        {
            Influencers = new List<IUser>();
            influencersDagree = new List<int>();
            profetionals = new List<int>();
            AlgoLevel = 0;
        }

        public List<IUser> Influencers { get => influencers; set => influencers = value; }
        public int AlgoLevel { get => algoLevel; set => algoLevel = value; }
        public List<int> InfluencersDagree { get => influencersDagree; set => influencersDagree = value; }
        public List<int> Profetionals { get => profetionals; set => profetionals = value; }


        //DONE
        public List<IUser> PreliminaryFiltering(ITweet[] forFilter)
        {
            for(int i = 0;  i<forFilter.Length;i++)
            {
                var followersCount = forFilter[i].CreatedBy.FollowersCount;
                var friendsCount = forFilter[i].CreatedBy.FriendsCount;
                var followerRank = (double)followersCount / (double)(followersCount + friendsCount);
                if (followersCount > MINFOLLOWERS && followersCount < MAXFOLLOWERS && followerRank > 0.87) 
                {
                    if (!Influencers.Contains(forFilter[i].CreatedBy))
                    {
                        Influencers.Add(forFilter[i].CreatedBy);
                        InfluencersDagree.Add(1);
                        Profetionals.Add(0);
                    }
                    else
                    {
                        var index = Influencers.IndexOf(forFilter[i].CreatedBy);
                        InfluencersDagree[index]++;
                        Profetionals[index] = 1; ;

                    }
                    Console.WriteLine(Influencers);
                }

            }
            AlgoLevel++;
            return Influencers;
        }



        public List<IUser> PotentialInfluencersFriend()
        {
            List<IUser> PotFriends = new List<IUser>();

            for (int i = 0; i < InfluencersDagree.Count; i++)
            {
                if (InfluencersDagree[i] > 1)
                {
                    PotFriends.Add(Influencers[i]);
                }

            }
            return PotFriends;

        }

        public List<IUser> SecondFilter(ITweet[] potInfluencers)
        {
           
            return new List<IUser>();
        }

        public void IsProfetional(ITweet[] tweets,string query)
        {

            foreach(ITweet tweet in tweets)
            {

            }
             
        }

        public void QuerySearchKeys(string query)
        {
            var keys = query.Split(';');
        }

        public ArrayList RankingStage(List<IUser> FinalList)
        {
            ArrayList InfluencerArray = new ArrayList();

           
          
            //Collect RT/FT/FOLLOWERS/FRIENDS and calculate General activity
            for (int i = 0; i < FinalList.Count; i++)
            {

            }

            return InfluencerArray;
        }


    }
}