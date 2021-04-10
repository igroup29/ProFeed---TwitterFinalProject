using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
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
        const string AccessTokenSecret = "b4k5NBgHL5L9ExbL9sJrrDeuYJ4nj88X8IY2VPnqZ0yzG";
        const string AccessToken = "1325866978804985857-3Jcrx5kswoPQdxRWB0Fvw8QlJpBPrK";
        const string ApiKeySecret = "UovMbaBiR2CFS30wn9MHCgJdh9sebdVEUab35koi1V8r7NqnNw";
        public TwitterClient tc;

        public TwitterModel()
        {
            tc = new TwitterClient(ApiKey, ApiKeySecret, AccessToken, AccessTokenSecret);
        }

        public async Task<ITweet[]> GetTwittsByQuery(string Query, int RetweetMin)
        {
            string newQuery = Query.Replace("HASHTAG", "#");
            // var parameters = new SearchTweetsParameters("#UNBOXING tech min_retweets:2")     
            var parameters = new SearchTweetsParameters(newQuery + " min_retweets:" + RetweetMin)
            {
                IncludeEntities = true,
                Lang = LanguageFilter.English,
                SearchType = SearchResultType.Mixed,
                TweetMode = TweetMode.Extended,
            };

            return await tc.Search.SearchTweetsAsync(parameters);
        }


        public async Task<ITweet[]> GetUserTimeline(IUser potInfluencer)
        {
            //Requests / 15-min window (user auth) - 900
            //
            return await tc.Timelines.GetUserTimelineAsync(potInfluencer.Id);
        }

        public async Task<IUser[]> GetUserFriends(long id)
        {
            return await tc.Users.GetFriendsAsync(id);
        }

        public async Task<object> GetUserByID(IUser user)
        {
            var userResult =  await tc.UsersV2.GetUserByIdAsync(user.Id);
            return userResult.User;
        }
    }
}