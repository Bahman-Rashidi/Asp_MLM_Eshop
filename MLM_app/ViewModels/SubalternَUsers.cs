using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MLM_app.ViewModels
{
    //Subordinate Users
    public class SubalternUsers
    {
      
        public bool IsExist { get; set; }
     
        public string UserId { get; set; }
        [Display(Name = "User ")]
        public string UserName { get; set; }

        //right1 right2 left1 left2
        public string Location { get; set; }
        [Display(Name = "Date Submit")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        [UIHint("DateTimePicker")]
        public DateTime CreatePersianDate { get; set; }


        [Display(Name = "Last purchase")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        [UIHint("DateTimePicker")]
        public DateTime LastedPurchas { get; set; }


        [Display(Name = " User ")]
        public string SubUserPass { get; set; }





    }
}