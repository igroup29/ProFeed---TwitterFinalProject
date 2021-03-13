using TwitterAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TwitterAPI.Controllers
{
    public class HubListController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<TList> Get()
        {
            TList myList = new TList();
            return myList.GetLists();
        }

        //// GET api/<controller>/5
        //public int Get([FromUri] int tListid,[FromBody] int userId)
        //{
        //    TList tl = new TList();
        //    return tl.IsAutorizedUser(tListid, userId);
        //}
        
        // POST api/<controller>
        public TList Post([FromBody]TList tList)
        {
            TList myList = new TList();
            myList.InsertList(tList);

            return myList;
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public int Delete(int id)
        {
            TList myList = new TList();
            myList.DeleteList(id);

            return id;
        }
    }
}