using System;
using System.Collections.Generic;
using System.Collections;
using Tweetinvi.Models;



namespace TwitterAPI.Models
{
    public class ProFeedAlg
    {
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
                if (followersCount >ProFeedApiParameters.ProFeedAlgParameters.MinFolowers && followersCount < ProFeedApiParameters.ProFeedAlgParameters.MaxFolowers && followerRank > ProFeedApiParameters.ProFeedAlgParameters.MinRankFirstStep) 
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
                if (followersCount > ProFeedApiParameters.ProFeedAlgParameters.MinFolowers && followersCount < ProFeedApiParameters.ProFeedAlgParameters.MaxFolowers && followerRank > ProFeedApiParameters.ProFeedAlgParameters.MinRankScondStep)
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

        public double IsProfetional(ITweet[] tweets,ArrayList query,TProfile profile)
        {
            //(#replies+#retweets)/#followers*100
            double engagementRate = 0;       
            int tweetsCounter = 0;
            int retweetCounter = 0;
            int inCalculationCounter = 0;
            double returnCalculation = 0;
            profile.TimelineCount = tweets.Length;
            try
            {
                foreach (ITweet tweet in tweets)
                {
                    if (tweet.IsRetweet == false)
                    {
                        profile.OriginalTweets++;
                        double replyCount = 0;
                        if (tweet.ReplyCount != null)
                            replyCount = (double)tweet.ReplyCount;
                        engagementRate = 100 * ((double)tweet.RetweetCount + replyCount) / tweet.CreatedBy.FollowersCount;                    
                        profile.TweetsEngagmentRate += engagementRate;
                        if (tweet.Hashtags.Count != 0)
                        {
                            inCalculationCounter++;
                            if (tweet.Retweeted == true)
                            {
                                retweetCounter++;
                                tweetsCounter += 3;
                            }
                            var hashTagsToCheck = new ArrayList();
                            foreach (Tweetinvi.Models.Entities.IHashtagEntity entity in tweet.Hashtags)
                            {
                                hashTagsToCheck.Add(entity.Text);
                            }
                            foreach (string q in query)
                            {
                                foreach (string hashtag in hashTagsToCheck.ToArray())
                                {
                                    if (q.Equals(hashtag.ToLower()))
                                    {
                                        tweetsCounter++;
                                        hashTagsToCheck.Remove(hashtag);
                                    }
                                }
                            }
                        }
                    }
                }
                returnCalculation = (double)tweetsCounter / inCalculationCounter;
            }
            catch (DivideByZeroException dbze)
            {
                Console.WriteLine(dbze.Message);
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            //{ "Number of tweets in timeline:", "Calculated Tweets:", "Number of Tweets contaning query:", "Number of ReTweeted tweets:" };
            
            profile.CalculatedTweets = inCalculationCounter;
            profile.QueryIncludedTweets = tweetsCounter;
            profile.RetweetedTweets = retweetCounter;


            if (inCalculationCounter * ProFeedApiParameters.ProFeedAlgParameters.AMPLIFIER < tweets.Length)
                return 0;
        
            return returnCalculation;


        }

    
        //unresolved, mayby not relevant
        public string[] QuerySearchKeys(string query)
        {
            var keys = query.Split(',');
            for(int i=0;i<keys.Length;i++)
            {
                keys[i] = keys[i].Replace(" ","");
            }
            return keys;
        }

        public void RankingStage(IUser user,TProfile profile)
        {
            double calculationSupport = 0;
            calculationSupport = Math.Round(profile.TweetsEngagmentRate, 2);
            profile.TweetsEngagmentRate = calculationSupport;
            //engagement -> (total engagement/ impact)*100
            if (profile.OriginalTweets != 0)
            {
                calculationSupport = Math.Round((100 * profile.TweetsEngagmentRate / profile.OriginalTweets), 2);
                profile.Engagment = calculationSupport;
            }
            //impact/impressions ->followers*originalTweets
            profile.Impact = profile.Followers * profile.OriginalTweets;
            //general activity -> (originalTweets+replies+retweets)/number of tweets
            int replyCount = 0;
            if (user.Status.ReplyCount != null)
                replyCount = (int)user.Status.ReplyCount;
            if (profile.TimelineCount != 0)
            {
                calculationSupport = Math.Round(((double)(profile.OriginalTweets + replyCount + user.Status.RetweetCount) / profile.TimelineCount), 2);
                profile.GeneralActivity = calculationSupport;
            }
            RankFinalStage(profile);
        }

        public void RankFinalStage(TProfile profile)
        {
            profile.Rank += (profile.Followers < ProFeedApiParameters.ProFeedInfluencerParameters.InfluencerVolumeSmall) ? 2 : (profile.Followers < ProFeedApiParameters.ProFeedInfluencerParameters.InfluencerVolumeMedium) ? 1 : 0;
            profile.Rank += (profile.Engagment > ProFeedApiParameters.ProFeedInfluencerParameters.InfluencerEngagementLarge) ? 3 : (profile.Engagment > ProFeedApiParameters.ProFeedInfluencerParameters.InfluencerEngagementMedium) ? 2 : (profile.Engagment > ProFeedApiParameters.ProFeedInfluencerParameters.InfluencerEngagementMedium) ? 1 : 0;
            profile.Rank += (profile.GeneralActivity > ProFeedApiParameters.ProFeedInfluencerParameters.InfluencerGneralActivityLarge) ? 3 : (profile.GeneralActivity > ProFeedApiParameters.ProFeedInfluencerParameters.InfluencerGneralActivityMedium) ? 2 : 0;
            profile.Rank += (profile.Profetional) ? 2 : 0;
        }

        public void ClearLists()
        {
            Influencers.Clear();
            InfluencersDagree.Clear();
        }
    }
}