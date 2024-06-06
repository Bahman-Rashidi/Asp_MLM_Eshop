using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MLM_app.ViewModels
{
    public class UserForChartVm
    {

        public string UserId { get; set; }
        public string ImageUrl { get; set; }
        public string EnglishName { get; set; }
        //public string Persianame { get; set; }
        public String Email { get; set; }
        public string ParentUserId { get; set; }
        public string Location { get; set; }
        public bool IsPurchased { get; set; }

    }
}