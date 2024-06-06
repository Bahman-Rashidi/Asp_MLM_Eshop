using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MLM_app.Infrastructure;

namespace MLM_app.Models
{
    public class Setting
    {
        #region constructor
        /// <summary>
        /// constructor
        /// </summary>
        public Setting()
        {
            Infrastructure.Utility oUtility = new Utility();
            InsertDateTime = oUtility.DateMilady();

        }

        #endregion end  constructor

        #region Option Table Fields

        //########################################|OptionId|###########################################       
        [System.ComponentModel.DataAnnotations.Key]
        [System.ComponentModel.DataAnnotations.Required]
        // Change the field name as follows
        [System.ComponentModel.DataAnnotations.Schema.Column
            ("SettingId", Order = 0, TypeName = "int")]
        // These two attributes exclude the field from auto-incrementing property
        //[System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated
        //    (System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]
        public int SettingId { get; set; }

        //###################################################################################################        


        //########################################|Logo|#######################################################        
        [System.ComponentModel.DataAnnotations.MaxLength(2000)]
        [System.ComponentModel.DataAnnotations.Schema.Column
            ("LogoUrl", Order = 1, TypeName = "varchar")]
        [Display(Name = "Website Logo")]
        [UIHint("Upload")]
        public string LogoUrl { get; set; }
        //###################################################################################################

        //########################################|Logo|#######################################################        
        [System.ComponentModel.DataAnnotations.MaxLength(4000)]
        [System.ComponentModel.DataAnnotations.Schema.Column
            ("About", Order = 2, TypeName = "nvarchar")]
        [Display(Name = "About us")]
        [AllowHtml]
        [UIHint("Ckeditor")]
        public string About { get; set; }
        //###################################################################################################

        //########################################|Logo|#######################################################        
        [System.ComponentModel.DataAnnotations.MaxLength(4000)]
        [System.ComponentModel.DataAnnotations.Schema.Column
            ("Contact", Order = 3, TypeName = "nvarchar")]
        [Display(Name = "contact us")]
        [AllowHtml]
        [UIHint("Ckeditor")]
        public string Contact { get; set; }
        //###################################################################################################

        //########################################|InsertDateTime|############################################
        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.Schema.Column
        ("InsertDateTime", Order = 4, TypeName = "datetime")]
        [Display(Name = "Submit Date and Time")]
       
        public DateTime InsertDateTime { get; set; }
        //################################################################################################### 



        #endregion


    }
}