using System.Collections.Generic;


namespace TwitterAPI.Models
{
    public class TList
    {

        public int ListID { get; set; }
        public int UserID { get; set; }
        public double ListAVGRank { get; set; }
        public List<TProfile> MatchingProfiles { get; set; }
        public string Product { get; set; }
        public string MajorCategory { get; set; }
        public string SecondCategory { get; set; }
        public string SubCategory { get; set; }

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