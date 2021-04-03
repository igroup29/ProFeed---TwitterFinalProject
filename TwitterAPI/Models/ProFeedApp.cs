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
        private ProFeedAlg pFAlg;
        private TwitterModel tm;
        private List<IUser> influencers;
        private TProfile[] finalList;

        //var inf = alg.PreliminaryFiltering(tweets);

        public ProFeedApp()
        {
            PFAlg = new ProFeedAlg();
            TM = new TwitterModel();
            //Influencers = new List<IUser>();
            FinalList = new TProfile[] { };

        }
        public ProFeedApp(string query)
        {
            PFAlg = new ProFeedAlg();
            TM = new TwitterModel();
            //Influencers = new List<IUser>();
            FinalList = new TProfile[] { };

        }

        public ProFeedAlg PFAlg { get => pFAlg; set => pFAlg = value; }
        public TwitterModel TM { get => tm; set => tm = value; }
        public List<IUser> Influencers { get => influencers; set => influencers = value; }
        public TProfile[] FinalList { get => finalList; set => finalList = value; }


        public async Task<TProfile[]> StartSearch(string Query, int RetweetMin)
        {
            //step 1 - 
            var tweets = await TM.GetTwittsByQuery(Query, RetweetMin);
            Influencers = PFAlg.PreliminaryFiltering(tweets);
            //step 2 - 
            // ArrayList 
            List<Thread> threads = new List<Thread>();
            foreach(IUser iUser in Influencers)
            {
                //await TM.GetUserTimeline(iUser);
                var work = new ThreadWork();
                work.user = iUser;
                threads.Add(new Thread(new ThreadStart(work.Work)));
                threads.Last().Start();
            }
            //var PotFriends = algo.SecondFiltration(potInfluencer);
            //var user = await GetUserByID(potInfluencer[0]);
            //var TimeLine = await GetUserTimeline(PotFriends);
            //var user = await TM.GetUserByID(Influencers.First());
            System.Threading.Thread.Sleep(1000);
            return finalList;
        }
        //public void ThreadWork(IUser user)
        //{
        //    var timeline =  TM.GetUserTimeline(user);

        //}

    }

    class ThreadWork
    {
        public IUser user;
        public void Work()
        {
            var TM = new TwitterModel();
            var PFAlg = new ProFeedAlg();
            var timeline =  TM.GetUserTimeline(user);

        }
    }
}