using System;
using System.Collections.Generic;
using System.Text;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Data
{
    public interface IAccountDataStore
    {
        bool IsApplicable(string dataStoreType);
        Account GetAccount(string accountNumber);
        void UpdateAccount(Account account);
    }
}
