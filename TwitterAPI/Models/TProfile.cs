using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwitterAPI.Models
{
    public class TProfile
    {
        public long ProfileID { get; set; }
        public string Name { get; set; }
        public string ScreenName { get; set; }
        public string Email { get; set; }
        public bool Profetional { get; set; }
        public int ListID { get; set; }
        public double Rank { get; set; }
        public int Followers { get; set; }
        public bool IsVerified { get; set; }
        public double Reach { get; set; }
        public double Impact { get; set; }
        public double Engagment { get; set; }
        public string Image { get; set; }
        //category will obligate relation table in DB
        //public List<string> Categories { get; set; }

        public TProfile(long id,string name,string screenName,bool verified)
        {
            ProfileID = id;
            Name = name;
            ScreenName = screenName;
            Profetional = false;
            IsVerified = verified;
        }


        //public int InsertProfile(Item item)
        //{
        //    DBservices dbs = new DBservices();

        //    int numAffected = dbs.insertListItem(item);
        //    return numAffected;
        //}

        //public void DeleteItem(int id)
        //{
        //    DBservices dbs = new DBservices();

        //    dbs.DeleteItemFromList(id);
        //}

        //public int EditItem(int ItemID, Item newItem)
        //{
        //    DBservices dbs = new DBservices();

        //    int numAffected = dbs.EditItemInList(newItem, ItemID);
        //    return numAffected;
        //}

        //public List<Item> GetItems()
        //{
        //    DBservices dbs = new DBservices();
        //    return dbs.getItems();
        //}

        //public List<Item> GetListItems(int id)
        //{
        //    DBservices dbs = new DBservices();
        //    return dbs.GetItemstoList(id);
        //}

    }
}