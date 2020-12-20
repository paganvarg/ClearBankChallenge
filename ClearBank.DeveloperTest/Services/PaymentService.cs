using System;
using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Types;
using System.Configuration;
using ClearBank.DeveloperTest.Services.PaymentStrategies;

namespace ClearBank.DeveloperTest.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IAccountDataStoreFactory _accountDataStoreFactory;
        private readonly IPaymentStrategyFactory _paymentStrategyFactory;
        private readonly IConfigurationService _configurationService;

        public PaymentService(IAccountDataStoreFactory accountDataStoreFactory, IPaymentStrategyFactory paymentStrategyFactory, IConfigurationService configurationService)
        {
            _accountDataStoreFactory = accountDataStoreFactory;
            _paymentStrategyFactory = paymentStrategyFactory;
            _configurationService = configurationService;
        }

        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            // I could probably convert it into more pipeline-like pattern, but the amount of code in this function is small enough for it to be readable.
            var dataStoreType = _configurationService.GetDataStoreType();

            var accountDataStore = _accountDataStoreFactory.GetAccountDataStore(dataStoreType);
            var account = accountDataStore.GetAccount(request.DebtorAccountNumber);

            var result = _paymentStrategyFactory.GetPaymentStrategy(request.PaymentScheme).GetResult(account, request);

            if (result)
            {
                account.Balance -= request.Amount;

                accountDataStore.UpdateAccount(account);
            }

            return new MakePaymentResult{Success = result};
        }
    }
}
