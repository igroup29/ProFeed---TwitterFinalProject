using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Threading;
using Tweetinvi;
using Tweetinvi.Parameters;
using Tweetinvi.Models;
using Tweetinvi.Parameters.Enum;
using Tweetinvi.Parameters.V2;
using System.Collections;

namespace TwitterAPI.Models
{
    public class ProFeedApp
    {
        const double PRORANGE = 0.35;
        const double MINRANGE = 0.25;
        private string[] searchQuery = { "fintech", "stockExchange", "forex" };

        //var inf = alg.PreliminaryFiltering(tweets);
        public ProFeedAlg ProFeedAlg { get; set; }
        public TwitterModel TwitterModel { get; set; }
        public TData SearchData { get; set; }

        public ProFeedApp()
        {
            ProFeedAlg = new ProFeedAlg();
            TwitterModel = new TwitterModel();
            SearchData = new TData();
            SearchData.SearchKeys.AddRange(searchQuery);

        }
        public ProFeedApp(string query)
        {
            ProFeedAlg = new ProFeedAlg();
            TwitterModel = new TwitterModel();
            SearchData = new TData();
            //to be continued, with search keys from controller
        }

        

        public async Task Work(IUser user,int index)
        {
            try
            {
                var timeline = await TwitterModel.GetUserTimeline(user);
                //need to test
                SearchData.FinalList.Add(new TProfile());
                var inBusiness = ProFeedAlg.IsProfetional(timeline, SearchData.SearchKeys, SearchData.FinalList.Last());
                //to here
                InsertDataToTProfile(user,index);
                if (inBusiness > MINRANGE)
                {
                    SearchData.FinalList.Last().Profetional = true;
                    if (!ProFeedAlg.Profetionals.Equals(user))
                    {
                        if (ProFeedAlg.Profetionals.Count > 4)
                            ProFeedAlg.Profetionals.RemoveAt(0);
                        ProFeedAlg.Profetionals.Add(user);
                    }

                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public async Task Work2(IUser user, int index)
        {
            try
            {
                var timeline = await TwitterModel.GetUserTimeline(user);
                //need to test
                SearchData.FinalList.Add(new TProfile());
                var inBusiness = ProFeedAlg.IsProfetional(timeline, SearchData.SearchKeys, SearchData.FinalList.Last());
                //to here
                if (inBusiness > PRORANGE)
                {
                    InsertDataToTProfile(user,index);
                    SearchData.FinalList.Last().Profetional = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public async Task WaitAllTasks(List<Task> tasks)
        {
            Task step2 = Task.WhenAll(tasks.ToArray());
            await step2;
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
            SearchData.FinalList.Last().Reach = ProFeedAlg.InfluencersDagree[index];
        }

        public async Task<TData> StartSearch(string query, int reTweeTwitterModelin)
        {
            //step 1 -
            string insertToAppStackTrace = "SearchQuery:" + query;
            SearchData.AppStackTrace.Add(insertToAppStackTrace);
            var tweets = await TwitterModel.GetTwittsByQuery(query, reTweeTwitterModelin);

            var influencers = ProFeedAlg.PreliminaryFiltering(tweets);
            //step 2 - 
            List<Task> threads = new List<Task>();
            int influencerIndex = 0;
            foreach (IUser iUser in influencers)
            {
                //await TwitterModel.GetUserTimeline(iUser);
                influencerIndex = influencers.IndexOf(iUser);
                var newTaskToAdd = Task.Factory.StartNew(async()=>await Work(iUser, influencerIndex));
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
            ProFeedAlg.ClearLists();
            threads.Clear();
            foreach (IUser user in ProFeedAlg.Profetionals)
            {
               // TwitterModel.UpdateTwitterClient();
                try
                {
                    var userFriends = await TwitterModel.GetUserFriends(user.Id);
                    influencers = ProFeedAlg.PreliminaryFiltering(userFriends);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                foreach (IUser iUser in influencers)
                {
                    influencerIndex = influencers.IndexOf(iUser);
                    var temp = Task.Factory.StartNew(async () => await Work2(iUser, influencerIndex));
                    threads.Add(temp);
                }
                try
                {
                    await WaitAllTasks(threads);
                    ProFeedAlg.ClearLists();
                    threads.Clear();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return SearchData;
        }
    }
}