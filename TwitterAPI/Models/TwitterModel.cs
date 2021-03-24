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

    public class TwitterModel
    {
        const string ApiKey = "IS2fcuHkE8Cmvoq7eeErjGFyW";
        const string AccessTokenSecret= "m0HOCvV9jB6PDgcYvFVZMaXIgzc6mJ4E6GAZhSvwjXDxg"; 
        const string AccessToken= "1349014948106670084-vbVbV6SzbRh4RH8u6putpqOQDGG5Yx";
        const string ApiKeySecret= "xBZ6eTfKTJULxwiCQz5s7cSNeoEv5wnDDRHqfB42RwQ7rewl82";

        public TwitterModel()
        {

        }

        //public async Task2<object> test()
        //{
        //    var tc = new TwitterClient(ApiKey, ApiKeySecret, AccessToken, AccessTokenSecret);
        //    var parameters = new SearchTweetsParameters("#Gaming")
        //    {
        //        IncludeEntities = true,
        //        Lang = LanguageFilter.English,
        //        SearchType = SearchResultType.Mixed,
        //        TweetMode = TweetMode.Extended
        //    };
        //    var tweets = await tc.Search.SearchTweetsAsync(parameters);
        //    return tweets;
        //}

        public async Task<object> GetTwittsByQuery(string Query,int RetweetMin)
        {
            string newQuery = Query.Replace("HASHTAG", "#");
            var tc = new TwitterClient(ApiKey, ApiKeySecret, AccessToken, AccessTokenSecret);
            // var parameters = new SearchTweetsParameters("#UNBOXING tech min_retweets:2")     
             var parameters = new SearchTweetsParameters(newQuery + " min_retweets:"+ RetweetMin)          
            {
                IncludeEntities = true,
                Lang = LanguageFilter.English,
                SearchType = SearchResultType.Mixed,
                TweetMode = TweetMode.Extended,                      
            };
            var tweets = await tc.Search.SearchTweetsAsync(parameters);
            var algo = new ProFeedAlg();
            var potInfluencer = algo.PreliminaryFiltering(tweets);
            var PotFriends=algo.SecondFiltration(potInfluencer);
           // var TimeLine = GetUserTimeline(PotFriends);
            return tweets;

        }


        public async Task<object> GetUserTimeline(List<IUser> PotFriends)
        {
            var tc = new TwitterClient(ApiKey, ApiKeySecret, AccessToken, AccessTokenSecret);
         
            var utl = await tc.Timelines.GetUserTimelineAsync(PotFriends[1].Id);

            return utl;
        }

        public async Task<object> GetUserFriends(int id)
        {
            // var parameters = new SearchTweetsParameters("#UNBOXING tech min_retweets:2") 
            var tc = new TwitterClient(ApiKey, ApiKeySecret, AccessToken, AccessTokenSecret);
            var friends = await tc.Users.GetFriendsAsync(id);
            return friends;
        }
    }
}