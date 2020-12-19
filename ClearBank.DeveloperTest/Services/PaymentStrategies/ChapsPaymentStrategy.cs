using System;
using System.Collections.Generic;
using System.Text;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services.PaymentStrategies
{
    public class ChapsPaymentStrategy : IPaymentStrategy
    {
        public bool IsApplicable(PaymentScheme paymentScheme)
        {
            return paymentScheme == PaymentScheme.Chaps;
        }

        public bool GetResult(Account account, MakePaymentRequest request)
        {
            if (account == null || !account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Chaps) || account.Status != AccountStatus.Live)
            {
                return false;
            }

            return true;
        }
    }
}
