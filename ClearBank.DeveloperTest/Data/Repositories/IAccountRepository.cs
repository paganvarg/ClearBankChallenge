using System;
using System.Collections.Generic;
using System.Text;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Data.Repositories
{
    public interface IAccountRepository : IRepository<Account>
    {
        Account GetAccountByAccountNumber(string accountNumber);
    }
}
