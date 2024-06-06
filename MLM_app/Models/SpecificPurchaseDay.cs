using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MLM_app.Models
{
    public class SpecificPurchaseDay
    {
      #region constructor
        public SpecificPurchaseDay()
        {
            //this.OrderDetails = new HashSet<OrderDetail>();
            //this.Payments = new HashSet<Payment>();
        }
        #endregion

        #region Properties
        [Key]
        [Display(Name ="Unique number")]
        public int ID { get; set; }

        [Display(Name = "User Creator")]
        public string InsertedApplicationUserId { get; set; }

        [Display(Name = "Day of mount")]
        [Required(ErrorMessage = "Please enter {0} ")]

        public string DayOfMount { get; set; }

        [Display(Name = "Date Create ")]
        [Required(ErrorMessage = "Please enter {0} ")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        [UIHint("DateTimePicker")]
        public DateTime CraeteDate { get; set; }


        [Display(Name = "status ")]
        public bool IsFinalized { get; set; }
        #endregion        
        
        #region Relations

        public virtual ApplicationUser ApplicationUser { get; set; }
        #endregion

    }
}