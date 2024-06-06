using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MLM_app.Infrastructure
{
    public class BankBankPayResult
    {
        #region Display payment result messages

        /// <summary>
        /// This method takes one input and returns the result message
        /// </summary>
        /// <param name="resultId"></param>
        /// <returns></returns>
        public static string ResultText(string resultId)
        {
            string result = "";
            switch (resultId)
            {
                case "0":
                    result = "Transaction completed successfully";
                    break;
                case "11":
                    result = "Invalid card number";
                    break;
                case "12":
                    result = "Insufficient balance";
                    break;
                case "13":
                    result = "Incorrect password";
                    break;
                case "14":
                    result = "Exceeded the maximum number of password attempts";
                    break;
                case "15":
                    result = "Invalid card";
                    break;
                case "16":
                    result = "Exceeded the maximum withdrawal limit";
                    break;
                case "17":
                    result = "User has canceled the transaction";
                    break;
                case "18":
                    result = "Card expiration date has passed";
                    break;
                // Other cases...
                default:
                    result = "Unknown error";
                    break;
            }
            return result;
        }

        #endregion
    }
}