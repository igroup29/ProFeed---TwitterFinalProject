using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using TwitterAPI.Models;

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

        //// public async Task<List<TProfile>> GetAsync([FromUri] string request, [FromUri] int RetweetMin)
        [HttpGet]
        [Route("api/Twitter")]
        public async Task<List<TProfile>> GetAsync([FromUri] string request)
        {
            int RetweetMin = 4;
            try
            {
                //TwitterModel myModel = new TwitterModel();
                //return await myModel.GetTwittsByQuery(request, RetweetMin);
                //  return await proFeedApp.StartSearch(request, RetweetMin);
                return await proFeedApp.StartSearch(request, RetweetMin);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }


        // GET api/<controller>/5
       
        public string Get([FromUri] string request)
        {
            return "value";
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