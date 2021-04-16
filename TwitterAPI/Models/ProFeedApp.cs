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
        const int MAXDEGEE = 2;
        const int MINRANGE = 2;
        private ProFeedAlg pFAlg;
        private TwitterModel tm;
        private List<TProfile> finalList;
        private string[] searchQuery = { "fintech","stockExchange" ,"forex"};

        //var inf = alg.PreliminaryFiltering(tweets);

        public ProFeedApp()
        {
            PFAlg = new ProFeedAlg();
            TM = new TwitterModel();
            FinalList = new List<TProfile>();

        }
        public ProFeedApp(string query)
        {
            PFAlg = new ProFeedAlg();
            TM = new TwitterModel();
            FinalList = new List<TProfile>();
             

        }

        public ProFeedAlg PFAlg { get => pFAlg; set => pFAlg = value; }
        public TwitterModel TM { get => tm; set => tm = value; }
        public List<TProfile> FinalList { get => finalList; set => finalList = value; }
        public string[] SearchQuery { get => searchQuery; set => searchQuery = value; }

        public async Task Work(IUser user,int index)
        {
            try
            {
                var timeline = await TM.GetUserTimeline(user);
                var inBusiness = PFAlg.IsProfetional(timeline, searchQuery);
                FinalList.Add(new TProfile(user.Id, user.Name, user.ScreenName,user.Verified));
                if (inBusiness > MINRANGE && PFAlg.InfluencersDagree[index] > MAXDEGEE)
                {
                    FinalList.Last().Profetional = true;
                    if (!PFAlg.Profetionals.Equals(user))
                    {
                        if (PFAlg.Profetionals.Count > 4)
                            PFAlg.Profetionals.RemoveAt(0);
                        PFAlg.Profetionals.Add(user);
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
                var timeline = await TM.GetUserTimeline(user);
                var inBusiness = PFAlg.IsProfetional(timeline, searchQuery);
                if (inBusiness > MINRANGE && PFAlg.InfluencersDagree[index] > MAXDEGEE)
                {
                    FinalList.Add(new TProfile(user.Id, user.Name, user.ScreenName, user.Verified));
                    FinalList.Last().Profetional = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task WaitAllTasks(List<Task> tasks)
        {
            Task step2 = Task.WhenAll(tasks.ToArray());
            await step2;
        }
        public void InitiateDataContainers(List<Task> list,int )
        {
          
        }

        public async Task<List<TProfile>> StartSearch(string Query, int RetweetMin)
        {
            //step 1 -
            var tweets = await TM.GetTwittsByQuery(Query, RetweetMin);
            var influencers = PFAlg.PreliminaryFiltering(tweets);
            //step 2 - 
            List<Task> threads = new List<Task>();
            int influecersIndex = 0;
            foreach(IUser iUser in influencers)
            {
                //await TM.GetUserTimeline(iUser);
                var newTaskToAdd = Task.Factory.StartNew(async()=>await Work(iUser,influecersIndex));
                influecersIndex++;
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
            PFAlg.ClearLists();
            threads.Clear();
            influecersIndex = 0;
            foreach (IUser user in PFAlg.Profetionals)
            {
                TM.UpdateTwitterClient();
                try
                {
                    var userFriends = await TM.GetUserFriends(user.Id);
                    influencers = PFAlg.PreliminaryFiltering(userFriends);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                foreach (IUser iUser in influencers)
                {
                    var temp = Task.Factory.StartNew(async () => await Work2(iUser, influecersIndex));
                    influecersIndex++;
                    threads.Add(temp);
                }
                try
                {
                    await WaitAllTasks(threads);
                    PFAlg.ClearLists();
                    threads.Clear();
                    influecersIndex = 0;
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