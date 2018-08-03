using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Shared.ConfigProvider
{
    /// <summary>
    /// Singleton class of Configuraiton Manager
    /// </summary>
    public class AppConfigurationManager
    {
        private static object _syncRoot = new Object();
        private static AppConfigurationManager _configurations;
        public Configurations Keys;

        private AppConfigurationManager()
        {
            InitializeProperties();
        }        

        public static AppConfigurationManager Instance
        {
            get
            {
                if(_configurations == null)
                {
                    lock (_syncRoot)
                    {
                        _configurations = new AppConfigurationManager();
                    }
                }

                return _configurations;
            }
        }


        private void InitializeProperties()
        {            
            Keys = new Configurations();
            var appSetting = System.Configuration.ConfigurationManager.AppSettings;
            Keys.PublicHolidays = appSetting["PublicHolidays"].Split('|');
            Keys.Weekends = appSetting["Weekends"].Split('|');
            Keys.TotalAllowedDaysToReturnBook = Convert.ToInt32(appSetting["DefaultReturnDaysCount"]);
            Keys.Penalty = Convert.ToInt32(appSetting["PenaltyAmount"]);
        }

        public class Configurations
        {
            public string[] Weekends { get; set; }
            public string[] PublicHolidays { get; set; }
            public int Penalty { get; set; }
            public int TotalAllowedDaysToReturnBook { get; set; }
        }

    }
}
