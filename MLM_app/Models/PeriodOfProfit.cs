using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MLM_app.Models
{
    public class PeriodOfProfit
    {

              #region constructor
        public PeriodOfProfit()
        {
           
        }
        #endregion

        #region Properties
        [Key]
        [Display(Name ="Unique number")]
        public int ID { get; set; }

        [Display(Name = "User Creator")]
        public string InsertedApplicationUserId { get; set; }

        [Display(Name = "Start Date of Period")]
        [Required(ErrorMessage = "Please enter {0} ")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        [UIHint("DateTimePicker")]
        public DateTime StartDate { get; set; }


        [Display(Name = "Date end ")]
        [Required(ErrorMessage = "Please enter {0} ")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        [UIHint("DateTimePicker")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Date Create ")]
        [Required(ErrorMessage = "Please enter {0} ")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        [UIHint("DateTimePicker")]
        public DateTime CraeteDate { get; set; }


        // When it's 1, we've dealt with all purchases
        [Display(Name = "status ")]
        public bool IsCalculated { get; set; }


        // When it's 1, we've dealt with all purchases
        [Display(Name = "Current term")]
        public bool IsCurrentPeriod { get; set; }

        [Display(Name = "Total purchase")]
        [StringLength(500)]
        public string AllPurchase { get; set; }


        [Display(Name = "Total profit")]
        [StringLength(500)]
        public string AllProfit { get; set; }
        #endregion        
        
        #region Relations
   
        #endregion
    }
}