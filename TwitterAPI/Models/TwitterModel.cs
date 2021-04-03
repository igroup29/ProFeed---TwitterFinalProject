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
        const string ApiKey = "8gJiTw1ohwk6EyfrKduvLJdII";
        const string AccessTokenSecret= "b4k5NBgHL5L9ExbL9sJrrDeuYJ4nj88X8IY2VPnqZ0yzG"; 
        const string AccessToken= "1325866978804985857-3Jcrx5kswoPQdxRWB0Fvw8QlJpBPrK";
        const string ApiKeySecret= "UovMbaBiR2CFS30wn9MHCgJdh9sebdVEUab35koi1V8r7NqnNw";
        public TwitterClient tc;

        public TwitterModel()
        {
            tc = new TwitterClient(ApiKey, ApiKeySecret, AccessToken, AccessTokenSecret);

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

        public async Task<ITweet[]> GetTwittsByQuery(string Query,int RetweetMin)
        {
            string newQuery = Query.Replace("HASHTAG", "#");
            // var parameters = new SearchTweetsParameters("#UNBOXING tech min_retweets:2")     
             var parameters = new SearchTweetsParameters(newQuery + " min_retweets:"+ RetweetMin)          
            {
                IncludeEntities = true,
                Lang = LanguageFilter.English,
                SearchType = SearchResultType.Mixed,
                TweetMode = TweetMode.Extended,                      
            };
            ITweet[] tweets = null;
            try
            {
                 tweets = await tc.Search.SearchTweetsAsync(parameters);
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        
            return tweets;

        }


        public async Task<ITweet[]> GetUserTimeline(IUser potInfluencer)
        {
            //Requests / 15-min window (user auth) - 900
            //
            ITweet[] utl= null;
            try
            {
               utl = await tc.Timelines.GetUserTimelineAsync(potInfluencer.Id);
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            return utl;
        }

        public async Task<object> GetUserFriends(int id)
        {
            // var parameters = new SearchTweetsParameters("#UNBOXING tech min_retweets:2") 
            var friends = await tc.Users.GetFriendsAsync(id);
            return friends;
        }

        public async Task<object> GetUserByID(IUser test)
        {
            Tweetinvi.Models.V2.UserV2 user = null;
            try
            {
                var userResponse = await tc.UsersV2.GetUserByIdAsync(test.Id);
                user = userResponse.User;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return user;
        }
    }
}