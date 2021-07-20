using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Tweetinvi.Models;


namespace TwitterAPI.Models
{
    public class ProFeedApp
    {
        //const double PRO_RANGE = 0.35;
        //const double MIN_RANGE = 0.25;
        //const int MINIMUM_RETWEETS = 5;
        //private string[] searchQuery = { "fintech", "stockExchange", "forex" };
        private string[] searchQuery;
        private SemaphoreSlim taskSemaphore = new SemaphoreSlim(1, 1);

        public ProFeedAlg ProFeedAlgorithm { get; set; }
        public TwitterModel ProFeedTwitterModel { get; set; }
        public TData SearchData { get; set; }

        public ProFeedApp()
        {
            ProFeedAlgorithm = new ProFeedAlg();
            ProFeedTwitterModel = new TwitterModel();
            SearchData = new TData();
        }
        //unresolved
        public ProFeedApp(string query)
        {
            ProFeedAlgorithm = new ProFeedAlg();
            ProFeedTwitterModel = new TwitterModel();
            SearchData = new TData();
            //to be continued, with search keys from controller
        }

        //was work before
        public async Task InfluencersToProfile(IUser user,int index)
        {
            try
            {
                var timeline = await ProFeedTwitterModel.GetUserTimeline(user);
                var fullUser = await ProFeedTwitterModel.GetUserByID(user.Id);
                //need to test                
                await taskSemaphore.WaitAsync();
                SearchData.FinalList.Add(new TProfile());
                var inBusiness = ProFeedAlgorithm.IsProfetional(timeline, SearchData.SearchKeys, SearchData.FinalList.Last());
                //to here
                InsertDataToTProfile(user, index);
                if (inBusiness > ProFeedApiParameters.ProFeedAppParameters.MIN_RANGE)
                {
                    SearchData.FinalList.Last().Profetional = true;
                    if (!ProFeedAlgorithm.Profetionals.Equals(user))
                    {
                        if (ProFeedAlgorithm.Profetionals.Count > 4)
                            ProFeedAlgorithm.Profetionals.RemoveAt(0);
                        ProFeedAlgorithm.Profetionals.Add(user);
                    }

                }
                ProFeedAlgorithm.RankingStage(fullUser,SearchData.FinalList.Last());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                taskSemaphore.Release();

            }

        }
        //was work2 before
        public async Task InfluencersFriendsSearch(IUser user, int index)
        {
            try
            {
                var timeline = await ProFeedTwitterModel.GetUserTimeline(user);
                var fullUser = await ProFeedTwitterModel.GetUserByID(user.Id);

                //need to test
                await taskSemaphore.WaitAsync();
                
                SearchData.FinalList.Add(new TProfile());
                var inBusiness = ProFeedAlgorithm.IsProfetional(timeline, SearchData.SearchKeys, SearchData.FinalList.Last());
                //to here
                if (inBusiness > ProFeedApiParameters.ProFeedAppParameters.PRO_RANGE)
                {
                    
                    InsertDataToTProfile(user,index);
                    SearchData.FinalList.Last().Profetional = true;
                    ProFeedAlgorithm.RankingStage(fullUser, SearchData.FinalList.Last());

                }
                else
                {
                    SearchData.FinalList.Remove(SearchData.FinalList.Last());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                taskSemaphore.Release();
            }
        }

        public async Task WaitAllTasks(List<Task> tasks)
        {
            Task step2 = Task.WhenAll(tasks.ToArray());
            await step2;
        }

        public bool CheckIfProfileExist(long id)
        {
            foreach(TProfile profile in SearchData.FinalList)
            {
                if (profile.ProfileEquals(id))
                    return false;
            }
            return true; 
        }

        public void InsertDataToTProfile(IUser user,int index)
        {
            SearchData.FinalList.Last().ProfileID = user.Id;
            SearchData.FinalList.Last().Name = user.Name;
            SearchData.FinalList.Last().ScreenName = user.ScreenName;
            SearchData.FinalList.Last().IsVerified = user.Verified;
            SearchData.FinalList.Last().Followers = user.FollowersCount;
            SearchData.FinalList.Last().Image = user.ProfileImageUrl400x400;
            SearchData.FinalList.Last().ProfileUrl = "https:"+"//twitter.com/" + user.ScreenName;
            SearchData.FinalList.Last().Website = user.Url;        
            SearchData.FinalList.Last().Description = user.Description;
            SearchData.FinalList.Last().Location = user.Location;
        }

        public async Task<TData> StartSearch(string query)
        {
            //step 1 -
            searchQuery = ProFeedAlgorithm.QuerySearchKeys(query);
            SearchData.SearchKeys.AddRange(searchQuery);
            string insertToAppStackTrace = "SearchQuery:" + query;
            SearchData.AppStackTrace.Add(insertToAppStackTrace);
            var tweets = await ProFeedTwitterModel.GetTwittsByQuery((string)SearchData.SearchKeys[(SearchData.SearchKeys.Count - 1)], ProFeedApiParameters.ProFeedAppParameters.MINIMUM_RETWEETS);
            insertToAppStackTrace = "Number of tweet in initial request:" + tweets.Length;
            SearchData.AppStackTrace.Add(insertToAppStackTrace);

            var influencers = ProFeedAlgorithm.PreliminaryFiltering(tweets);
            insertToAppStackTrace = "Number of Influencers after preliminary filtering:" + influencers.Count;
            SearchData.AppStackTrace.Add(insertToAppStackTrace);

            //step 2 - 
            List<Task> threads = new List<Task>();
            int influencerIndex = 0;
      
            foreach (IUser iUser in influencers)
            {
                //await TwitterModel.GetUserTimeline(iUser);
                influencerIndex = influencers.IndexOf(iUser);
                var newTaskToAdd = Task.Factory.StartNew(async()=>await InfluencersToProfile(iUser, influencerIndex));
                threads.Add(newTaskToAdd.Unwrap());
            }
            try
            {
                await WaitAllTasks(threads);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            //works to here 11-04-21
            //step 3-
            insertToAppStackTrace = "Number of Influencers with potential influencers friends:" + ProFeedAlgorithm.Profetionals.Count;
            SearchData.AppStackTrace.Add(insertToAppStackTrace);

            ProFeedAlgorithm.ClearLists();
            threads.Clear();
            int influencerCount = SearchData.FinalList.Count;
            int potentialInfluencerIndex = 1;
            foreach (IUser user in ProFeedAlgorithm.Profetionals)
            {
               // TwitterModel.UpdateTwitterClient();
                try
                {
                    var userFriends = await ProFeedTwitterModel.GetUserFriends(user.Id);
                    insertToAppStackTrace = "Number of friends for influencer" +potentialInfluencerIndex +":" + userFriends.Length;
                    SearchData.AppStackTrace.Add(insertToAppStackTrace);    
                    
                    influencers = ProFeedAlgorithm.PreliminaryFiltering(userFriends);
                    insertToAppStackTrace = "Number of potential influencers from Influencer" + potentialInfluencerIndex + " friends:" + influencers.Count;
                    SearchData.AppStackTrace.Add(insertToAppStackTrace);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                foreach (IUser iUser in influencers)
                {
                    influencerIndex = influencers.IndexOf(iUser);
                    var temp = Task.Factory.StartNew(async () => await InfluencersFriendsSearch(iUser, influencerIndex));
                    threads.Add(temp);
                }
                try
                {
                    await WaitAllTasks(threads);
                    ProFeedAlgorithm.ClearLists();
                    threads.Clear();
                    insertToAppStackTrace = "Number of new influencers from Influencer" + potentialInfluencerIndex + " friends:" + (SearchData.FinalList.Count - influencerCount);
                    SearchData.AppStackTrace.Add(insertToAppStackTrace);
                    potentialInfluencerIndex++;
                    influencerCount = SearchData.FinalList.Count;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            insertToAppStackTrace = "Final number of influencers" + SearchData.FinalList.Count;
            SearchData.AppStackTrace.Add(insertToAppStackTrace);


            var result = SearchData.FinalList.OrderByDescending(OrderFunc).Take(10).ToList();

            SearchData.FinalList = result;

            return SearchData;
        }
        private double OrderFunc(TProfile profile)
        {
            return profile.Rank;
        }
    }
}