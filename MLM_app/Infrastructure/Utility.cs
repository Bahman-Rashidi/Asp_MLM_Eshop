using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using MLM_app.Models;

namespace MLM_app.Infrastructure
{
    /// <summary>
    /// This class handles date management, error submission, visit count submission, and download submission
    /// </summary>
    public class Utility : System.Web.UI.Page
    {
        public string MiladiDate = "";

        PersianCalendar oPersianCalendar = new PersianCalendar();

        #region// constructor
        public Utility()
        {

        }
        #endregion

        #region Numeric Date Method
        public string Date()
        {
            string strDate = "";
            ApplicationDbContext oDataBaseContext = null;
            try
            {
                // Receive Date from the server
                oDataBaseContext = new ApplicationDbContext();
                var strDateTime = oDataBaseContext.Database.SqlQuery<DateTime>("SELECT GETDATE()").FirstOrDefault();

                //################################################ Date ##########################################################
                DateTime oDateTime = strDateTime;

                strDate = oPersianCalendar.GetYear(oDateTime).ToString("D2") + "/" +
                                oPersianCalendar.GetMonth(oDateTime) + "/" +
                                oPersianCalendar.GetDayOfMonth(oDateTime).ToString("D2");
            }
            catch (System.Exception ex)
            {
            }
            finally
            {
                if (oDataBaseContext != null)
                {
                    oDataBaseContext.Dispose();
                    oDataBaseContext = null;
                }
            }

            return strDate;
        }
        #endregion

        #region Time Method
        public string Time()
        {
            string strTime = "";
            ApplicationDbContext db = null;
            try
            {
                db = new ApplicationDbContext();

                // We get the time from the server
                var strDateTime = db.Database.SqlQuery<DateTime>("SELECT GETDATE()").FirstOrDefault();
                //################################################Hour################################################################

                DateTime Time = strDateTime;
                string SX = Time.Hour.ToString("D2");
                string SY = Time.Minute.ToString("D2");
                string SZ = Time.Second.ToString("D2");
                string SW = Time.Millisecond.ToString("D2");
                //strTime = SX + ":" + SY + ":" + SZ + "." + SW;
                strTime = SX + ":" + SY + ":" + SZ;
            }
            catch (System.Exception ex)
            {
            }
            finally
            {
                if (db != null)
                {
                    db.Dispose();
                    db = null;
                }
            }
            return strTime;
        }
        #endregion

     
        #region   System Gregorian Date
        public DateTime DateMilady()
        {
            string strDate = "";
            DateTime oDateTime = new DateTime();
            ApplicationDbContext oDataBaseContext = null;
            try
            {
                oDataBaseContext = new ApplicationDbContext();
                var strDateTime = oDataBaseContext.Database.SqlQuery<DateTime>("SELECT GETDATE()").FirstOrDefault();

                //################################################show Date ##########################################################
                oDateTime = strDateTime;

                //strDate = oPersianCalendar.GetYear(oDateTime).ToString("D2") + "/" +
                //                oPersianCalendar.GetMonth(oDateTime) + "/" +
                //                oPersianCalendar.GetDayOfMonth(oDateTime).ToString("D2");
                //MiladiDate = strDate;
            }
            catch (System.Exception ex)
            {

            }
            finally
            {
                if (oDataBaseContext != null)
                {
                    oDataBaseContext.Dispose();
                    oDataBaseContext = null;
                }
            }

            return oDateTime;
        }
        #endregion

    }
}