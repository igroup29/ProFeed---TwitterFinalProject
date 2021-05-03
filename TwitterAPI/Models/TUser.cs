using System.Collections.Generic;


namespace TwitterAPI.Models
{
    public class TUser 
    {

        public int UserID { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Image { get; set; }

        public TUser() {}

        public TUser(int userID, string email, string userName, string image)
        {
            UserID = userID;
            Email = email;
            UserName = userName;
            Image = image;
        }

       

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