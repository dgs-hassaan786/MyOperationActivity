using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace Foundation.Shared.Utilities
{
    public class Utilities
    {
        private ConfigProvider.AppConfigurationManager.Configurations Configurations = ConfigProvider.AppConfigurationManager.Instance.Keys;

        private static object _syncRoot = new Object();
        private static Utilities _utilities;

        private Utilities()
        {

        }

        public static Utilities Instance
        {
            get
            {
                if (_utilities == null)
                {
                    lock (_syncRoot)
                    {
                        _utilities = new Utilities();
                    }
                }

                return _utilities;
            }
        }

        #region Pulbic methods

        public DateTime GetDateAfterSpecifiedBusinessDays(int days)
        {
            DateTime targetedDate = DateTime.Now;

            int count = 0;
            while (count < days)
            {
                targetedDate = targetedDate.AddDays(1);
                if (IsBusinessDay(targetedDate))
                    count++;
            }

            return targetedDate;
        }


        public double CalcultePenaltyAmount(DateTime actualReturnDate, DateTime reqReturnedDate)
        {
            System.TimeSpan daysDiff = actualReturnDate.Subtract(reqReturnedDate);

            if (daysDiff.Days > 0)
            {
                int penaltyDays = GetTotalPenaltyDaysCount(reqReturnedDate, daysDiff.Days);

                return penaltyDays * ConfigProvider.AppConfigurationManager.Instance.Keys.Penalty;
            }

            return 0;
        }

        public void SetPageMessage(string message, Severity level, System.Web.UI.MasterPage master)
        {
            Label messageLabel = (Label)master.FindControl("lblMessage");
            messageLabel.Text = message;

            if (level == Severity.Error)
                messageLabel.ForeColor = System.Drawing.Color.Red;
            else
                messageLabel.ForeColor = System.Drawing.Color.Green;
        }

        #endregion

        #region Private Methods

        private int GetTotalPenaltyDaysCount(DateTime reqReturnDate, int penaltyDays)
        {
            int actualPenaltyDays = 0;
            for (int i = 1; i <= penaltyDays; i++)
            {
                DateTime date = reqReturnDate.AddDays(i);

                if (IsBusinessDay(date))
                    actualPenaltyDays += 1;
            }

            return actualPenaltyDays;
        }

        private bool IsBusinessDay(DateTime date)
        {
            bool _isBusinessDay = true;

            if (IsWeekend(date) || IsHoliday(date))
                _isBusinessDay = false;

            return _isBusinessDay;
        }

        private bool IsWeekend(DateTime date)
        {
            bool _isWeekend = false;

            if (Configurations.Weekends.Contains(date.DayOfWeek.ToString().ToLower()))
                _isWeekend = true;

            return _isWeekend;
        }

        private bool IsHoliday(DateTime date)
        {
            bool _isHoliday = false;


            for (int i = 0; i < Configurations.PublicHolidays.Length; i++)
            {
                var publicHoliday = Configurations.PublicHolidays[i].Trim();
                string[] holidayDate = publicHoliday.Split('-');

                if (holidayDate != null && holidayDate.Length == 2)
                {
                    int day = Convert.ToInt32(publicHoliday.Trim().Split('-')[0]);
                    int month = Convert.ToInt32(publicHoliday.Trim().Split('-')[1]);

                    if (date.Day == day && date.Month == month)
                    {
                        _isHoliday = true;
                        break;
                    }
                }
            }

            return _isHoliday;
        }


        #endregion
        
        public enum Severity
        {
            Info,
            Error
        }

    }
}
