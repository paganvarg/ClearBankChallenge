using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Data.Repositories
{
    public class AccountRepository : Repository<Account>, IAccountRepository
    {
        public AccountRepository(ClearBankContext context) : base(context)
        {
        }

        public Account GetAccountByAccountNumber(string accountNumber)
        {
            return GetAll().FirstOrDefault(a => a.AccountNumber == accountNumber);
        }
    }
}
