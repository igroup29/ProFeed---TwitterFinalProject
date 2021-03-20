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
    public class ProFeedAlg
    {
        private List<IUser> influencers;
        private int algoLevel;


        public ProFeedAlg()
        {
            Influencers = new List<IUser>();
            AlgoLevel = 0;
        } 

        public List<IUser> Influencers { get => influencers; set => influencers = value; }
        public int AlgoLevel { get => algoLevel; set => algoLevel = value; }

        public List<IUser> PreliminaryFiltering(ITweet[] forFilter)
        {
            for(int i = 0;  i<forFilter.Length;i++)
            {
                var followersCount = forFilter[i].CreatedBy.FollowersCount;
                if (followersCount > 10000)
                {
                    Influencers.Add(forFilter[i].CreatedBy);
                    Console.WriteLine(Influencers);
                }

            }
            return Influencers;
        }
        public void SecondFiltration()
        {

        }

    }
}