using System;
using System.Collections.Generic;
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
    public class ProFeedApp
    {
        private ProFeedAlg pFAlg;
        private TwitterModel tm;
        private List<IUser> influencers;
        private List<TProfile> finalList;

        //var inf = alg.PreliminaryFiltering(tweets);

        public ProFeedApp()
        {
            PFAlg = new ProFeedAlg();
            TM = new TwitterModel();
            //Influencers = new List<IUser>();
            FinalList = new List<TProfile>();

        }

        public ProFeedAlg PFAlg { get => pFAlg; set => pFAlg = value; }
        public TwitterModel TM { get => tm; set => tm = value; }
        public List<IUser> Influencers { get => influencers; set => influencers = value; }
        public List<TProfile> FinalList { get => finalList; set => finalList = value; }


        public List<TProfile> StartSearch(string Query, int RetweetMin)
        {
            //var tweets = (TM.GetTwittsByQuery(Query, RetweetMin)).Result;
            //Influencers = PFAlg.PreliminaryFiltering(tweets);

            return finalList;
        }



    }


}