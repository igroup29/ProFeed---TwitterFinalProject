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
    public class TSeaechKey
    {
        private string mainCategory;
        private string subCategory;
        private string[] searchKeys;
        private string[] hashtags;

        public TSeaechKey()
        {
        }

        public TSeaechKey(string query)
        {

            MainCategory = mainCategory;
            SubCategory = subCategory;
            SearchKeys = searchKeys;
            Hashtags = hashtags;
        }

        public string MainCategory { get => mainCategory; set => mainCategory = value; }
        public string SubCategory { get => subCategory; set => subCategory = value; }
        public string[] SearchKeys { get => searchKeys; set => searchKeys = value; }
        public string[] Hashtags { get => hashtags; set => hashtags = value; }
    }
}