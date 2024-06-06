using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MLM_app.ViewModels
{
    public class EmailSendViewModel
    {
        public EmailSendViewModel()
        {

        }
        /***************************************************************/

        [Display(Name = "Recipient Email")]
        public string Email { get; set; }
        //################################################################

        [Display(Name = "Subject")]
        public string Subject { get; set; }


        [Display(Name = "Send Activation Link")]
        public Boolean IsSenhvnrimUrl { get; set; }


        //###############################################################

        [Display(Name = "Message Content")]
        [DataType(DataType.MultilineText)]
        [UIHint("Ckeditor")]
        [AllowHtml]
        public string Body { get; set; }

        /*******************************************************************/

    }
}