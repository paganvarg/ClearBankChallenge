using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace ClearBank.DeveloperTest.Services
{
    public class ConfigurationService : IConfigurationService
    {
        public string GetDataStoreType()
        {
            // This could be replaced with injecting IConfiguration object in the constructor
            return ConfigurationManager.AppSettings["DataStoreType"];
        }
    }
}
