using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BookCheckInAndOut.Utilities
{
    public static class Utilities
    {
        /// <summary>
        /// This function calculates the date after specified number of work/business days
        /// Friday and Saturday are treated as off days.
        /// </summary>
        /// <param name="days">Business days</param>
        /// <returns>Target Date</returns>
        public static DateTime GetDateAfterSpecifiedBusinessDays(int days)
        {
            var startDay = DateTime.Now;
            var businessDaysCount = 0;
            var requiredTotalDays = 0;
            while (businessDaysCount < days)
            {
                if (startDay.DayOfWeek != DayOfWeek.Friday && startDay.DayOfWeek != DayOfWeek.Saturday)
                {
                    businessDaysCount++;
                }
                requiredTotalDays++;
                if (businessDaysCount != days) startDay = startDay.AddDays(1);
            }

            return startDay;
        }


        public static void SetPageMessage(string message, Severity level, System.Web.UI.MasterPage master)
        {
            Label messageLabel = (Label)master.FindControl("lblMessage");
            messageLabel.Text = message;

            if (level == Severity.Error)
                messageLabel.ForeColor = System.Drawing.Color.Red;
            else
                messageLabel.ForeColor = System.Drawing.Color.Green;
        }


        public enum Severity
        {
            Info,
            Error
        }
    }


}