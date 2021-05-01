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
        private ProFeedAlg proFeedAlg;
        private TwitterModel twitterModel;
        private List<TProfile> finalList;
        private string[] searchQuery = { "fintech","stockExchange" ,"forex"};
        private SemaphoreSlim mutex = new SemaphoreSlim(1, 1);

        //var inf = alg.PreliminaryFiltering(tweets);

        public ProFeedApp()
        {
            ProFeedAlg = new ProFeedAlg();
            TwitterModel = new TwitterModel();
            FinalList = new List<TProfile>();

        }
        public ProFeedApp(string query)
        {
            ProFeedAlg = new ProFeedAlg();
            TwitterModel = new TwitterModel();
            FinalList = new List<TProfile>();
             

        }

        public ProFeedAlg ProFeedAlg { get => proFeedAlg; set => proFeedAlg = value; }
        public TwitterModel TwitterModel { get => twitterModel; set => twitterModel = value; }
        public List<TProfile> FinalList { get => finalList; set => finalList = value; }
        public string[] SearchQuery { get => searchQuery; set => searchQuery = value; }

        public async Task Work(IUser user,int index)
        {
            try
            {
                var timeline = await TwitterModel.GetUserTimeline(user);
                var inBusiness = ProFeedAlg.IsProfetional(timeline, searchQuery);
                InsertDataToTProfile(user,index);
                if (inBusiness > MINRANGE)
                {
                    FinalList.Last().Profetional = true;
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
                var inBusiness = ProFeedAlg.IsProfetional(timeline, searchQuery);
                if (inBusiness > PRORANGE)
                {
                    InsertDataToTProfile(user,index);
                    FinalList.Last().Profetional = true;
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
            FinalList.Add(new TProfile(user.Id, user.Name, user.ScreenName, user.Verified));
            FinalList.Last().Followers = user.FollowersCount;
            FinalList.Last().Image = user.ProfileImageUrl400x400;
            FinalList.Last().ProfileUrl = "https://twitter.com/" + user.ScreenName;
            FinalList.Last().Website = user.Url;
            FinalList.Last().Banner = user.ProfileBannerURL;
            FinalList.Last().Description = user.Description;
            FinalList.Last().Location = user.Location;
            FinalList.Last().Reach = ProFeedAlg.InfluencersDagree[index];
        }

        public async Task<List<TProfile>> StartSearch(string Query, int RetweeTwitterModelin)
        {
            //step 1 -
            var tweets = await TwitterModel.GetTwittsByQuery(Query, RetweeTwitterModelin);
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
            return FinalList;
        }
    }
}