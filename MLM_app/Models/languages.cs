using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MLM_app.Models
{
    public class language
    {

       
        [Key]
        public int Id { get; set; }


        [Display(Name = "language Id")]
        [Required(ErrorMessage = "Please enter {0} ")]
        public int lanId { get; set; }






        [Display(Name = "language name")]
        [Required(ErrorMessage = "Please enter {0} ")]

        public string LanName { get; set; }


    }
}
