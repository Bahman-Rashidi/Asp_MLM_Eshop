using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MLM_app.Models
{
    #region ExternalLoginConfirmationViewModel
    public class ExternalLoginConfirmationViewModel
    {
        [Required(ErrorMessage = "please enter  {0} ")]

        [EmailAddress(ErrorMessage = "format of {0} entered is incorrect")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
    #endregion

    #region ExternalLoginListViewModel

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }
    #endregion

    #region SendCodeViewModel
    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }
    #endregion

    #region VerifyCodeViewModel
    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }
    #endregion

    #region ForgotViewModel
    public class ForgotViewModel
    {


        [Required(ErrorMessage = "Please enter {0} ")]
        [EmailAddress(ErrorMessage = "The format of {0} entered is incorrect")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
    #endregion

    #region LoginViewModel
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Please enter {0} ")]
        [EmailAddress(ErrorMessage = "format of {0} entered is incorrect")]
        [Display(Name = "Email")]
      
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter {0} ")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }


        //########################################|InsertDateTime|############################################
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        [Required(ErrorMessage = "please enter  {0} ")]
        [Display(Name = "Result of Sum")]
        public string Captcha { get; set; }
        //################################################################################################### 



        [Display(Name = "remember me?")]
        public bool RememberMe { get; set; }
    }
    #endregion

    #region RegisterViewModel
    public class RegisterViewModel
    {

        [Required(ErrorMessage = "Please enter {0} ")]
        [EmailAddress(ErrorMessage = "format of {0} entered is incorrect")]
        [Display(Name = "Email")]
        [Remote("CheckExistingEmail", "Account", ErrorMessage = "This email has already been submitted!")]

        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter {0} ")]
        [StringLength(100, ErrorMessage = "The value of field {0} must be at least {2} characters!", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password confirmation")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The passwords do not match.")]
        public string ConfirmPassword { get; set; }

        //########################################|InsertDateTime|############################################
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        [Required(ErrorMessage = "Please enter {0} ")]
        [Display(Name = "ResultOfSum")]
        public string Captcha { get; set; }

       //  [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        [Required(ErrorMessage = "Please specify the user location")]
        public string LocationMLM { get; set; }


        [StringLength(200)]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please enter {0} ")]
        [Display(Name = "Card number")]
        [StringLength(16, ErrorMessage = "The bank card number must be 16 digits.", MinimumLength = 16)]
        public string kartNumber { get; set; }


        [Display(Name = "account number")]
        public string AccountNumber { get; set; }

        [Required(ErrorMessage = "Please enter {0} ")]
        [Display(Name = "First and last name")]

        public string EnglishName { get; set; }

        [Display(Name = "SIN Number")]
        [StringLength(10, ErrorMessage = "SIN Number must be 10 digits", MinimumLength = 10)]
        [Required(ErrorMessage = "Please enter {0} ")]
        [Remote("CheckExistingNationalCode", "Account", ErrorMessage = "tis SIN Number already Submittd !")]

        public string NationalCode { get; set; }
        //################################################################################################### 
    }
    #endregion

    #region RegisterViewModelBayPassw

    public class RegisterViewModelBayPassw
    {
        [Required(ErrorMessage = "Please enter {0} ")]
        [EmailAddress(ErrorMessage = "format of {0} entered is incorrect")]
        [Display(Name = "Email")]
        [Remote("CheckExistingEmail", "Account", ErrorMessage = "This email has already been submitted!")]

        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter {0} ")]
        [StringLength(10, ErrorMessage = "SIN Number must be 10 digits", MinimumLength = 10)]
       // [DataType(DataType.Password)]
        [Display(Name = "Parent  SIN Number")]
        public string ParentNationalCode { get; set; }

        [Required(ErrorMessage = "Please enter {0} ")]
        [Display(Name = "Parent Password")]
        public string ParentPasswordUser { get; set; }


        [StringLength(200)]
        [Display(Name = "Address")]
        public string Address { get; set; }


        [Required(ErrorMessage = "Please enter {0} ")]
        [StringLength(100, ErrorMessage = "The value of field {0} must be at least {2} characters!", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password confirmation")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The passwords do not match.")]
        public string ConfirmPassword { get; set; }

        //########################################|InsertDateTime|############################################
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        [Required(ErrorMessage = "Please enter {0} ")]
        [Display(Name = "ResultOfSum")]
        public string Captcha { get; set; }

        //  [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        //[Required(ErrorMessage = "Please specify the user location")]
        //public string LocationMLM { get; set; }


        [Required(ErrorMessage = "Please enter {0} ")]
        [Display(Name = "Card number")]
        [StringLength(16, ErrorMessage = "The bank card number must be 16 digits", MinimumLength = 16)]
        public string kartNumber { get; set; }


        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }



        [Display(Name = "account number")]
        public string AccountNumber { get; set; }

        [Required(ErrorMessage = "Please enter {0} ")]
        [Display(Name = "First and last name")]

        public string EnglishName { get; set; }

        [Display(Name = "SIN Number")]
        [StringLength(10, ErrorMessage = "SIN Number must be 10 digits", MinimumLength = 10)]
        [Required(ErrorMessage = "Please enter {0} ")]
        [Remote("CheckExistingNationalCode", "Account", ErrorMessage = "This SIN Number has already been submitted!")]

        public string NationalCode { get; set; }
        //################################################################################################### 
    }


    #endregion

    #region ResetPasswordViewModel
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Please enter {0} ")]
        [EmailAddress(ErrorMessage = "format of {0} entered is incorrect")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter {0} ")]
        [StringLength(100, ErrorMessage = "The value of field {0} must be at least {2} characters!", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password confirmation")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The passwords do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
    #endregion

    #region ForgotPasswordViewModel
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Please enter {0} ")]
        [EmailAddress(ErrorMessage = "format of {0} entered is incorrect")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
    #endregion
}
