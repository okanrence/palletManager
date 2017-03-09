using System.Configuration;
namespace MyAppTools
{

    public class MyAppToolsAppConfig
    {
        public static string DBConnection { get { return ConfigurationManager.AppSettings["DBConnection"]; } }
        public static bool UseAuth
        {
            get
            {
                if (ConfigurationManager.AppSettings["DBConnection"] == "Y")
                    return true;
                else
                    return false;
            }
        }
    }
}