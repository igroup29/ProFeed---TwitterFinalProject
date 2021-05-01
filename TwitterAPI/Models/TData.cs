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
using System.Collections;

namespace TwitterAPI.Models
{
    public class TData
    {
        public string[] AppStuckTrace { get; set; }
        public List<TProfile> FinalList { get; set; }
        public string[] SearchKeys { get; set; }


        public TData()
        {
            FinalList = new List<TProfile>();
        }

    }
}