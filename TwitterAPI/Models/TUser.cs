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
    public class TUser 
    {
        private int userID;
        private string email;
        private string userName;
        private string image;
      

        public TUser() {}

        public TUser(int userID, string email, string userName, string image)
        {
            UserID = userID;
            Email = email;
            UserName = userName;
            Image = image;
        }

        public int UserID { get => userID; set => userID = value; }
        public string Email { get => email; set => email = value; }
        public string UserName { get => userName; set => userName = value; }
        public string Image { get => image; set => image = value; }

        public int InsertUser(TUser user)
        {
            DBservices dbs = new DBservices();

            int numAffected = dbs.insertUser(user);
            return numAffected;
        }

        public List<TUser> GetUsers()
        {
            DBservices dbs = new DBservices();
            return dbs.getUsers();
        }

        public List<TList> GetUserLists(TUser user)
        {
            DBservices dbs = new DBservices();
            return dbs.getUserLists(user);
        }
        //@param email - user email 
        //if user exist return user id else return 0
        public int IsExist(string email)
        {
            DBservices dbs = new DBservices();
            return dbs.isExist(email);
        }
        public int EditUser(TUser user)
        {
            DBservices dbs = new DBservices();

            int numAffected = dbs.editUser(user);
            return numAffected;
        }
    }
}