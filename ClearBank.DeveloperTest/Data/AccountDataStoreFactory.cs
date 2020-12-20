using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClearBank.DeveloperTest.Data
{
    public class AccountDataStoreFactory : IAccountDataStoreFactory
    {
        private readonly IAccountDataStore[] _accountDataStores;

        public AccountDataStoreFactory(IAccountDataStore[] accountDataStores)
        {
            _accountDataStores = accountDataStores;
        }

        public IAccountDataStore GetAccountDataStore(string dataStoreType)
        {
            var accountStore = _accountDataStores.FirstOrDefault(s => s.IsApplicable(dataStoreType));

            if (accountStore == null)
            {
                throw new ArgumentException($"No {nameof(IAccountDataStore)} found for {dataStoreType} type.");
            }

            return accountStore;
        }
    }
}
