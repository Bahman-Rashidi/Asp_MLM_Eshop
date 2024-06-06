using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MLM_app.Models
{
    public class SubUserPassword
    {
        #region constructor
        public SubUserPassword()
        {
           
        }
        #endregion

        #region Properties
        [Key]
        public int SubUserPasswordId { get; set; }


        [StringLength(10, ErrorMessage = "SIN Number must be 10 digits", MinimumLength = 10)]
        [Required(ErrorMessage = "Please enter {0} ")]
        public string NationalCode { get; set; }

        
        public string ApplicationUserId { get; set; }


        [Display(Name = "Date Createtion of passwords")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        [UIHint("DateTimePicker")]
        public DateTime CraeteDate { get; set; }



    
        [NotMapped]
        [Display(Name = "Date Createtion of passwords")]
        [Required(ErrorMessage = "Please enter {0} ")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        [UIHint("DateTimePicker")]
        public DateTime persianDate {
            get { return CraeteDate; }
        }


         [StringLength(6)]
         public string L1_pass { get; set; }
         public bool IsL1_passUsed { get; set; }
         [Display(Name = "Password usage date")]
         [UIHint("DateTimePicker")]
         public Nullable<DateTime> L1_passUsedDate { get; set; }

         [StringLength(6)]
         public string L2_pass { get; set; }
         public bool IsL2_passUsed { get; set; }
         [Display(Name = "Password usage date")]
         [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
         [UIHint("DateTimePicker")]
         public Nullable<DateTime> L2_passUsedDate { get; set; }


         [StringLength(6)]
         public string R1_pass { get; set; }
         public bool IsR1_passUsed { get; set; }
         [Display(Name = "Password usage date")]
         [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
         [UIHint("DateTimePicker")]
         public Nullable<DateTime> R1_passUsedDate { get; set; }

         [StringLength(6)]
         public string R2_pass { get; set; }
         public bool IsR2_passUsed { get; set; }
         [Display(Name = "Password usage date")]
         [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
         [UIHint("DateTimePicker")]
         public Nullable<DateTime> R2_passUsedDate { get; set; }



        #endregion



         #region Relations
         //Navigation Properties
         public virtual ApplicationUser ApplicationUser { get; set; }
         #endregion

    }
}