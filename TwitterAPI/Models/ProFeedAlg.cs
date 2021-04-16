using System;
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
        public List<IUser> Influencers { get; set; }
        public int AlgoLevel { get; set; }
        public List<int> InfluencersDagree { get; set; }
        public List<IUser> Profetionals { get; set; }




        public ProFeedAlg()
        {
            Influencers = new List<IUser>();
            InfluencersDagree = new List<int>();
            Profetionals = new List<IUser>();
            AlgoLevel = 0;
        }

    


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
                    }
                    else
                    {
                        var index = Influencers.IndexOf(forFilter[i].CreatedBy);
                        InfluencersDagree[index]++;                        
                    }
                    Console.WriteLine(Influencers);
                }

            }
            AlgoLevel++;
            PotentialInfluencersFriend();
            return Influencers;
        }

        public List<IUser> PreliminaryFiltering(IUser[] forFilter)
        {
            for (int i = 0; i < forFilter.Length; i++)
            {
                var followersCount = forFilter[i].FollowersCount;
                var friendsCount = forFilter[i].FriendsCount;
                var followerRank = (double)followersCount / (double)(followersCount + friendsCount);
                if (followersCount > MINFOLLOWERS && followersCount < MAXFOLLOWERS && followerRank > 0.92)
                {
                    if (!Influencers.Contains(forFilter[i]))
                    {
                        Influencers.Add(forFilter[i]);
                        InfluencersDagree.Add(1);
                    }
                    else
                    {
                        var index = Influencers.IndexOf(forFilter[i]);
                        InfluencersDagree[index]++;
                    }
                }
            }
            AlgoLevel++;
            return Influencers;
        }


        public void PotentialInfluencersFriend()
        {
            for (int i = 0; i < InfluencersDagree.Count; i++)
            {
                if (InfluencersDagree[i] > 1 && Profetionals.Count < 5)
                {
                    Profetionals.Add(Influencers[i]);
                }
            }
        }

        public List<IUser> SecondFilter(ITweet[] potInfluencers)
        {
           
            return new List<IUser>();
        }

        public int IsProfetional(ITweet[] tweets,string[] query)
        {
            int inTweetsCounter = 0;
            int inCalculationCounter = 0;
            foreach(ITweet tweet in tweets)
            {
                if (tweet.Hashtags.Count !=0 && tweet.IsRetweet == true)
                {
                    inCalculationCounter++;
                    if (tweet.Retweeted == true)
                        inTweetsCounter += 3;
                    var hashTagsToCheck = new ArrayList();
                    foreach (Tweetinvi.Models.Entities.IHashtagEntity entity in tweet.Hashtags)
                    {
                        hashTagsToCheck.Add(entity.Text);
                    }
                    foreach (string q in query)
                    {
                        foreach (string hashtag in hashTagsToCheck)
                        {
                            if (hashtag.Equals(q))
                            {
                                inTweetsCounter++;
                                hashTagsToCheck.Remove(hashtag);
                            }
                        }
                    }
                }
            }
            try
            {
                return (int)(inTweetsCounter / inCalculationCounter);
            }catch(DivideByZeroException dbze)
            {
                Console.WriteLine(dbze.Message);
                return 0;
            }
            
        }
        //unresolved, mayby not relevant
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

        public void ClearLists()
        {
            Influencers.Clear();
            InfluencersDagree.Clear();
        }
    }
}