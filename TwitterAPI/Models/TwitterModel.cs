using System;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Parameters;
using Tweetinvi.Exceptions;
using Tweetinvi.Models;


namespace TwitterAPI.Models
{
    public class TwitterModel
    {
        //const string ApiKey = "8gJiTw1ohwk6EyfrKduvLJdII";
        //const string AccessTokenSecret = "b4k5NBgHL5L9ExbL9sJrrDeuYJ4nj88X8IY2VPnqZ0yzG";
        //const string AccessToken = "1325866978804985857-3Jcrx5kswoPQdxRWB0Fvw8QlJpBPrK";
        //const string ApiKeySecret = "UovMbaBiR2CFS30wn9MHCgJdh9sebdVEUab35koi1V8r7NqnNw";
        public TwitterClient Client { get; set; }

        public TwitterModel()
        {
            Client = new TwitterClient(ProFeedApiParameters.TwitterModelParameters.ApiKey, ProFeedApiParameters.TwitterModelParameters.ApiKeySecret, ProFeedApiParameters.TwitterModelParameters.AccessToken, ProFeedApiParameters.TwitterModelParameters.AccessTokenSecret);
     
        }
        public void UpdateTwitterClient()
        {
            Client = new TwitterClient(ProFeedApiParameters.TwitterModelParameters.ApiKey, ProFeedApiParameters.TwitterModelParameters.ApiKeySecret, ProFeedApiParameters.TwitterModelParameters.AccessToken, ProFeedApiParameters.TwitterModelParameters.AccessTokenSecret);
        }

        public async Task<ITweet[]> GetTwittsByQuery(string Query, int RetweetMin)
        {
            // some code to illustrate how to catch Tweetinvi exceptions.
            try
            {
                var user = await Client.Users.GetAuthenticatedUserAsync();
                string newQuery = Query.Replace("HASHTAG", "#");
                // var parameters = new SearchTweetsParameters("#UNBOXING tech min_retweets:2")     
                var parameters = new SearchTweetsParameters(newQuery + " min_retweets:" + RetweetMin)
                {
                    IncludeEntities = true,
                    Lang = LanguageFilter.English,
                    SearchType = SearchResultType.Mixed,
                    TweetMode = TweetMode.Extended,
                };
                Client.Config.RateLimitTrackerMode = RateLimitTrackerMode.TrackAndAwait;
                return await Client.Search.SearchTweetsAsync(parameters);
            }
            catch (TwitterException te)
            {
                Console.WriteLine(te.Message);
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
          }


        public async Task<ITweet[]> GetUserTimeline(IUser potInfluencer)
        {
            //Requests / 15-min window (user auth) - 900
            //
            try
            {
                Client.Config.RateLimitTrackerMode = RateLimitTrackerMode.TrackAndAwait;
                Client.Config.HttpRequestTimeout = TimeSpan.FromSeconds(60);
                return await Client.Timelines.GetUserTimelineAsync(potInfluencer.Id);
            }catch(Exception ex)
            {
                var exeption  = ex.ToString();
                return null;
            }
        }

        public async Task<IUser[]> GetUserFriends(long id)
        {
            try
            {
                Client.Config.RateLimitTrackerMode = RateLimitTrackerMode.TrackAndAwait;
                return await Client.Users.GetFriendsAsync(id);
            }catch(Exception ex)
            {
                var exeption = ex.ToString();
                return null;
            }
        }

        public async Task<IUser> GetUserByID(long userId)
        {
            try
            {
                Client.Config.RateLimitTrackerMode = RateLimitTrackerMode.TrackAndAwait;
                Client.Config.HttpRequestTimeout = TimeSpan.FromSeconds(60);
                //var userResult = await tc.UsersV2.GetUserByIdAsync(user.Id);
                //return userResult.User;
                var tweetinviUser = await Client.Users.GetUserAsync(userId);
                return tweetinviUser;
            }
            catch(Exception ex)
            {
                var exeption = ex.ToString();
                return null;
            }
        }
    }
}