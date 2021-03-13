using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwitterAPI.Models
{
    public class TProfile
    {
        private string email;
        private int listID;
        private double rank;
        private int followers;
        private int isVerified;
        private int reach;
        private int impact;
        private double engagment;
        private string image;
        private List<string> categories;

        public string Email { get => email; set => email = value; }
        public int ListID { get => listID; set => listID = value; }
        public double Rank { get => rank; set => rank = value; }
        public int Followers { get => followers; set => followers = value; }
        public int IsVerified { get => isVerified; set => isVerified = value; }
        public int Reach { get => reach; set => reach = value; }
        public int Impact { get => impact; set => impact = value; }
        public double Engagment { get => engagment; set => engagment = value; }
        public string Image { get => image; set => image = value; }
        public List<string> Categories { get => categories; set => categories = value; }

        public TProfile(string email, int listID, double rank, int followers, int isVerified, int reach, int impact, double engagment, string image, List<string> categories)
        {
            Email= email;
            ListID = listID;
            Rank = rank;
            Followers = followers;
            IsVerified = isVerified;
            Reach = reach;
            Impact = impact;
            Engagment = engagment;
            Image = image;
            Categories = categories;
        }

        public int InsertProfile(Item item)
        {
            DBservices dbs = new DBservices();

            int numAffected = dbs.insertListItem(item);
            return numAffected;
        }

        public void DeleteItem(int id)
        {
            DBservices dbs = new DBservices();

            dbs.DeleteItemFromList(id);
        }

        public int EditItem(int ItemID, Item newItem)
        {
            DBservices dbs = new DBservices();

            int numAffected = dbs.EditItemInList(newItem, ItemID);
            return numAffected;
        }

        public List<Item> GetItems()
        {
            DBservices dbs = new DBservices();
            return dbs.getItems();
        }

        public List<Item> GetListItems(int id)
        {
            DBservices dbs = new DBservices();
            return dbs.GetItemstoList(id);
        }

    }
}