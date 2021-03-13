using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwitterAPI.Models
{
    public class TList
    {
        private int listID;
        private int userID;
        //private DateTime lastModified;
        private double listAVGRank;
        private List<TProfile> matchingProfiles;
        // bare in mind - model for product with the following instances.
        private string product;
        private string majorCategory;
        private string secondCategory;
        private string subCategory;

        public int ListID { get => listID; set => listID = value; }
        public int UserID { get => userID; set => userID = value; }
        public double ListAVGRank { get => listAVGRank; set => listAVGRank = value; }
        public List<TProfile> MatchingProfiles { get => matchingProfiles; set => matchingProfiles = value; }
        public string Product { get => product; set => product = value; }
        public string MajorCategory { get => majorCategory; set => majorCategory = value; }
        public string SecondCategory { get => secondCategory; set => secondCategory = value; }
        public string SubCategory { get => subCategory; set => subCategory = value; }

        public TList()
        {

        }

        public TList(int listID, int userID, double listAVGRank, List<TProfile> matchingProfiles, string product, string majorCategory, string secondCategory, string subCategory)
        {
            ListID = listID;
            UserID = userID;
            ListAVGRank = listAVGRank;
            MatchingProfiles = matchingProfiles;
            Product = product;
            MajorCategory = majorCategory;
            SecondCategory = secondCategory;
            SubCategory = subCategory;
        }

        public int InsertList(TList tList)
        {
            DBservices dbs = new DBservices();

            int numAffected = dbs.insertList(tList);
            return numAffected;
        }

        //public int IsAutorizedUser(int hubListId, int userId)
        //{
        //    DBservices dbs = new DBservices();

        //    int numAffected = dbs.isAutorizedUser(hubListId, userId);
        //    return numAffected;
        //}

        public List<TList> GetLists()
        {
            DBservices dbs = new DBservices();
            return dbs.getList();
        }

        public void DeleteList(int id)
        {
            DBservices dbs = new DBservices();

            dbs.deleteList(id);
        }

    }
}