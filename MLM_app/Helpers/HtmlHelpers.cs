using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace MLM_app.Helpers
{


    // Create Extension Method and Helper
    // To create a helper, we create a class that is public and static, and within it, we have public and static functions.
    // The essence of an extension method is to add something to a class for which we do not have the source code.
    // Note that if you intend to write a text box or label, let's make the output of type HTML string.
    // Once we have created the string, we call Helper.Raw from the helper to ensure that the information is not encoded.

    /// <summary>
    /// HTML Helper class
    /// Public and static
    /// </summary>

    public static class HtmlHelpers
    {

        #region

        /// <summary>
  
        ///  Public and static function
        /// For this function to be joined to a class that we do not have access to,
        /// we must write the first parameter as follows:

        /// </summary>
        /// <param name="helper"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string DTTextBox(this System.Web.Mvc.HtmlHelper helper, string name)
        {
            string strResult =
                string.Format("<input id=\"{0}\" name=\"{0}\" type=\"text\" value=\"\" />", name);

            return (strResult);
        }
        #endregion

        #region

        /// <summary>
        /// Public and static function
        /// In order for this function to be joined to a class that we do not have access to,
        /// we must write the first parameter as follows:
        /// This method is of type HTML Helper, which is suitable for text boxes and labels.
        /// The output of this method is in raw format.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="target"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static System.Web.IHtmlString DtxLabel
            (this System.Web.Mvc.HtmlHelper helper, string target, string text)
        {
            string strResult =
                string.Format("<div class='caption'><label for='{0}'>{1}</label></div>", target, text);

            return (helper.Raw(strResult));
        }
        #endregion

        #region

        /// <summary>
        /// Public and static function
        /// To enable this function to be joined to a class that we do not have access to,
        /// we must write the first parameter as follows:
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="name"></param>
        /// <param name="caption"></param>
        /// <returns></returns>
        public static string DTButton(this System.Web.Mvc.HtmlHelper helper, string name, string caption)
        {
            string strResult =
                string.Format("<input id=\"{0}\" name=\"{0}\" type=\"submit\" value=\"{1}\" class=\"button\" />", name, caption);

            return (strResult);
        }
        #endregion

        #region

        /// <summary>
        /// A public and static function
        /// To allow this function to be attached to a class that we do not have access to,
        /// we need to write the first parameter as follows:
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static string DTReset(this System.Web.Mvc.HtmlHelper helper)
        {
            string strResult =
                string.Format("<input type=\"reset\" value=\"Reset\" class=\"button\" />");

            return (strResult);
        }
        #endregion

        #region Truncate
        public static string Truncate(this System.Web.Mvc.HtmlHelper helper, string input, int length)
        {
            if (String.IsNullOrEmpty(input))
            {
                return null;
            }

            if (input.Length <= length)
            {
                return input;
            }
            else
            {
                return (input.Substring(0, length) + "<text>...</text>");
            }
        }
        #endregion

        #region calculation  Sum stuff
        public static MvcHtmlString TotalPrice(this HtmlHelper html, int OrderedCount, int ProductPrice)
        {
            int total = OrderedCount * ProductPrice;
           // return new MvcHtmlString(total.ToString("#,0 $"));
            return new MvcHtmlString(total.ToString("#,0 $"));

        }
        #endregion

        #region Discount calculation
        /********************************************************************/
        //  Discount calculation
        public static MvcHtmlString OffProduct(this HtmlHelper html, int off, int price, bool strike = false)
        {
            if (off == 0)
            {
                // we separate the numbers into groups of three and append the string
                return new MvcHtmlString(price.ToString("#,0 $"));
               // return new MvcHtmlString(price.ToString("#,0 $"));


            }
            else if (strike == false)
            {
                int calcOff = price * off / 100;

                return
                    new MvcHtmlString(string.Format("<strike>{0}</strike><br/>{1}", price.ToString("#,0 $"),
                        (price - calcOff).ToString("#,0 $")));

            }
            else if (strike == true)
            {
                return new MvcHtmlString(string.Format("{0}", (price - (price * off / 100)).ToString("#,0 $")));
            }

            return null;

        }
        #endregion
    }
}