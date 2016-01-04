using System;
using System.Configuration;


namespace Daisy.WebServiceProvider
{
    public static class SettingsProvider
    {
        public static string GetServiceUrl()
        {
            return ConfigurationManager.AppSettings.Get("DaisyServerAddress");
        }
    }
}
