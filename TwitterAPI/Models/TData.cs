using System.Collections.Generic;
using System.Collections;

namespace TwitterAPI.Models
{
    public class TData
    {
        public ArrayList AppStackTrace { get; set; }
        public ArrayList SearchKeys { get; set; }
        public List<TProfile> FinalList { get; set; }

        public TData()
        {
            AppStackTrace = new ArrayList();
            SearchKeys = new ArrayList();
            FinalList = new List<TProfile>();
        }
    }
}