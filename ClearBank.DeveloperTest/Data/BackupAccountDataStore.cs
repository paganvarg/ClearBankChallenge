using ClearBank.DeveloperTest.Data.Repositories;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Data
{
    public class BackupAccountDataStore : IAccountDataStore
    {
        private readonly IAccountRepository _accountRepository;

        public BackupAccountDataStore(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public bool IsApplicable(string dataStoreType)
        {
            return dataStoreType == "Backup";
        }

        public Account GetAccount(string accountNumber)
        {
            return _accountRepository.GetAccountByAccountNumber(accountNumber);
        }

        public void UpdateAccount(Account account)
        {
            _accountRepository.Update(account);
        }
    }
}
