using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Services.Protocols;
using MLM_app.Infrastructure;

namespace MLM_app.Models
{
    [System.ComponentModel.DataAnnotations.Schema.Table("Contacts")]
    public class Contact : System.Object
    {

        #region constructor
        /// <summary>
        /// constructor
        /// </summary>
        public Contact()
        {
            InsertDateTime = System.DateTime.Now;

        }
        #endregion end  constructor

        #region Fields of the  Contact Us  table
        //########################################|ContactId|###########################################       
        [System.ComponentModel.DataAnnotations.Key]
        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.Schema.Column
        ("ContactId", Order = 0, TypeName = "bigint")]
        //[System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated
        //    (System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]
        public int ContactId { get; set; }
        //###################################################################################################                

        //########################################|Name|#######################################################        
        [System.ComponentModel.DataAnnotations.MaxLength(50)]
        [System.ComponentModel.DataAnnotations.Schema.Column
        ("Name", Order = 1, TypeName = "nvarchar")]
        [Display(Name = "Name")]
        [Required(ErrorMessage = "Please enter {0} ")]

        public string Name { get; set; }
        //###################################################################################################

        //########################################|Email|#######################################################        
        [System.ComponentModel.DataAnnotations.MaxLength(100)]

        [System.ComponentModel.DataAnnotations.Schema.Column
        ("Email", Order = 2, TypeName = "varchar")]
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Please enter {0} ")]
        [EmailAddress(ErrorMessage = "format of {0} entered is incorrect")]
        public string Email { get; set; }
        //###################################################################################################

        //########################################|Subject|#######################################################                
        [System.ComponentModel.DataAnnotations.MaxLength(100)]
        [System.ComponentModel.DataAnnotations.Schema.Column
        ("Subject", Order = 3, TypeName = "nvarchar")]
        [Display(Name = "Topic")]
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
        [Display(Name = "Date and Time")]
        public DateTime InsertDateTime { get; set; }

        public Nullable<int> langugaeId { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        [Required(ErrorMessage = "Please enter {0} ")]
        [Display(Name = "Result")]
        public string Captcha { get; set; }
        //################################################################################################### 

        #endregion

    }
}