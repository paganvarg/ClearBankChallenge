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
            return ConfigurationManager.AppSettings["DataStoreType"];
        }
    }
}
