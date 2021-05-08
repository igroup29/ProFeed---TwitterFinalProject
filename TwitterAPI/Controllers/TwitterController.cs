using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TwitterAPI.Models;
using System.Web.Http;

namespace TwitterAPI.Controllers
{
    public class TwitterController : ApiController
    {
        ProFeedApp proFeedApp = new ProFeedApp();

        // GET api/<controller>  
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet]
        [Route("api/Twitter")]
        public async Task<TData> GetAsync([FromUri] string request)
        {
            try
            {
                //TwitterModel myModel = new TwitterModel();
                //return await myModel.GetTwittsByQuery(request, RetweetMin);
                //  return await proFeedApp.StartSearch(request, RetweetMin);
                return await proFeedApp.StartSearch(request);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}