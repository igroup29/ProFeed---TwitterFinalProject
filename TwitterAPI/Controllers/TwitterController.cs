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
        //// GET api/<controller>
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}
        ProFeedApp proFeedApp = new ProFeedApp();

        [HttpGet]
        //
        public async Task<List<TProfile>> GetAsync([FromUri] string request, [FromUri] int RetweetMin)
        {
            try
            {
                //TwitterModel myModel = new TwitterModel();
                //return await myModel.GetTwittsByQuery(request, RetweetMin);
                return await proFeedApp.StartSearch(request, RetweetMin);

            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }


        // GET api/<controller>/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

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