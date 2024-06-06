using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace MLM_app.Models
{
    #region IndexViewModel
    public class IndexViewModel
    {
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }
    }
    #endregion

    #region ManageLoginsViewModel
    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationDescription> OtherLogins { get; set; }
    }
    #endregion

    #region FactorViewModel
    public class FactorViewModel
    {
        public string Purpose { get; set; }
    }
    #endregion

    #region SetPasswordViewModel
    public class SetPasswordViewModel
    {


        [Required(ErrorMessage = "please enter  {0} ")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Previous password")]
        [Compare("NewPassword", ErrorMessage = "The passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
    #endregion

    #region ChangePasswordViewModel
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Please enter {0} ")]
        [DataType(DataType.Password)]
        [Display(Name = "Previous password")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Please enter {0} ")]
        [StringLength(100, ErrorMessage = "The value of the field {0} must be at least {2} characters long!", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }
    #endregion

    #region AddPhoneNumberViewModel
    public class AddPhoneNumberViewModel
    {
        [Required(ErrorMessage = "Please enter {0} ")]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Number { get; set; }
    }
    #endregion

    #region VerifyPhoneNumberViewModel
    public class VerifyPhoneNumberViewModel
    {
        [Required(ErrorMessage = "Please enter {0} ")]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }
    #endregion

    #region ConfigureTwoFactorViewModel
    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
    }
    #endregion
}