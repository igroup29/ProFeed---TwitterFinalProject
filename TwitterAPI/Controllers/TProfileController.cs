//using TwitterAPI.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Web.Http;

//namespace TwitterAPI.Controllers
//{
//    public class TProfileController : ApiController
//    {
//        // GET api/<controller>
//        public IEnumerable<TProfile> Get()
//        {
//            TProfile myProfile = new TProfile();
//            return null;//myProfile.GetProfiles();
//        }

//        // GET api/<controller>/5
//        public List<TProfile> Get([FromUri] int id)
//        {
//            TProfile myProfile = new TProfile();
//            //return myItem.GetItems();
//            return null;// myProfile.GetListProfiles(id);

//        }

//        // POST api/<controller>
//        public List<TProfile> Post([FromBody]TProfile tProfile)
//        {
//            TProfile myProfile = new TProfile();
//            myProfile.InsertProfile(tProfile);
//            return myProfile.GetListItems(tProfile.ListID);

//        }

//        // PUT api/<controller>/5
//        public List<Item> Put([FromUri]int id, [FromBody]Item NewItem)
//        {     
//            Item myItem = new Item();
//            myItem.EditItem(id, NewItem);

//            return myItem.GetListItems(NewItem.ListID);
//        }

//        // DELETE api/<controller>/5
//        public int Delete(int id)
//        {
//            Item myItem = new Item();
//            myItem.DeleteItem(id);

//            return id;
//        }
//    }
//}