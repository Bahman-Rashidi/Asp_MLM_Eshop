using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Services.Protocols;
using MLM_app.Infrastructure;

namespace MLM_app.Models
{
    public class Kartable
    {
                #region constructor
        /// <summary>
        /// constructor
        /// </summary>
        public Kartable()
        {
            InsertDateTime = System.DateTime.Now;

        }
        #endregion end  constructor

        #region Fields of the  Contact Us  table
        //########################################|ContactId|###########################################       
        [System.ComponentModel.DataAnnotations.Key]
        [System.ComponentModel.DataAnnotations.Required]
        ////Change the field name as follows
        //[System.ComponentModel.DataAnnotations.Schema.Column
        //("ContactId", Order = 0, TypeName = "bigint")]
        //// These two attributes exclude the field from auto-incrementing property
        // Change the field name as follows
        [System.ComponentModel.DataAnnotations.Schema.Column
          ("ContactId", Order = 0, TypeName = "bigint")]
        // These two attributes exclude the field from auto-incrementing property
        //[System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated
        //    (System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]
        public int KartableId { get; set; }
        //###################################################################################################                

        //########################################|Name|#######################################################        
        [System.ComponentModel.DataAnnotations.MaxLength(50)]
        [System.ComponentModel.DataAnnotations.Schema.Column
        ("Name", Order = 1, TypeName = "nvarchar")]
        [Display(Name = "Sender ID")]
        [Required(ErrorMessage = "Please enter {0} ")]

        public string AzKiUserId { get; set; }


        //########################################|Name|#######################################################        
        [System.ComponentModel.DataAnnotations.MaxLength(50)]
        [System.ComponentModel.DataAnnotations.Schema.Column
        ("Name", Order = 1, TypeName = "nvarchar")]
        [Display(Name ="Sender")]
        [Required(ErrorMessage = "Please enter {0} ")]

        public string AzKi { get; set; }
        //###################################################################################################




        //########################################|Name|#######################################################        
        [System.ComponentModel.DataAnnotations.MaxLength(50)]
        [System.ComponentModel.DataAnnotations.Schema.Column
        ("Name", Order = 1, TypeName = "nvarchar")]
        [Display(Name = "Recipient ID")]
        [Required(ErrorMessage = "Please enter {0} ")]

        public string BeKiUserId { get; set; }

        [System.ComponentModel.DataAnnotations.MaxLength(50)]
        [System.ComponentModel.DataAnnotations.Schema.Column
        ("Name", Order = 1, TypeName = "nvarchar")]
        [Display(Name = " Recipient")]
        [Required(ErrorMessage = "Please enter {0} ")]

        public string BeKi { get; set; }
        //###################################################################################################

   

        //########################################|Subject|#######################################################                
        [System.ComponentModel.DataAnnotations.MaxLength(100)]
        [System.ComponentModel.DataAnnotations.Schema.Column
        ("Subject", Order = 3, TypeName = "nvarchar")]
        [Display(Name = "Subject")]
        [Required(ErrorMessage = "Please enter {0} ")]
        public string Subject { get; set; }
        //###################################################################################################

        //########################################|Body|#######################################################        
        [System.ComponentModel.DataAnnotations.MaxLength(4000)]
        [System.ComponentModel.DataAnnotations.Schema.Column
        ("Body", Order = 4, TypeName = "nvarchar")]
        [Display(Name = "Message")]
        [Required(ErrorMessage = "Please enter {0} ")]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }
        //###################################################################################################

        //########################################|InsertDateTime|############################################
        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.Schema.Column
        ("InsertDateTime", Order = 5, TypeName = "datetime")]
        [Display(Name = "Date and Time of Registration")]
        public DateTime InsertDateTime { get; set; }


        //########################################|InsertDateTime|############################################
        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.Schema.Column
        ("InsertDateTime", Order = 5, TypeName = "datetime")]
        [Display(Name = "Date and Time of View")]
        public DateTime ViewDateTime { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        [Required(ErrorMessage = "Please enter {0} ")]
        [Display(Name = "ResultOfSum")]
        public string Captcha { get; set; }
        //################################################################################################### 

        #endregion

        #region Table Relationships

        #endregion
    }
}