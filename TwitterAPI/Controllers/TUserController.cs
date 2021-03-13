using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TwitterAPI.Models;

namespace TwitterAPI.Controllers
{
    public class UserController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<TUser> Get()
        {
            TUser myUser = new TUser();
            return myUser.GetUsers();
        }

        // GET api/<controller>/5
        public int Get(string email)
        {
            TUser myUser = new TUser();
            return myUser.IsExist(email);

        }

        // GET api/<controller>/5
        public List<TList> Get(TUser user)
        {
            TUser myUser = new TUser();
            return myUser.GetUserLists(user);
             
        }

        // POST api/<controller>
        public int Post([FromBody]TUser user)
        {
            TUser myUser = new TUser();
            myUser.InsertUser(user);
            return myUser.IsExist(user.Email);
        }

        // PUT api/<controller>/5
        public int Put([FromBody]TUser user)
        {
            TUser myUser = new TUser();
            myUser.EditUser(user);
            return myUser.IsExist(user.Email);
        }

        // DELETE api/<controller>/5
        public void Delete1(int id)
        {

        }
    }
}