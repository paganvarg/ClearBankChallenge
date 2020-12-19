using System;
using System.Collections.Generic;
using System.Text;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services.PaymentStrategies
{
    public class FasterPaymentsPaymentStrategy : IPaymentStrategy
    {
        public bool IsApplicable(PaymentScheme paymentScheme)
        {
            return paymentScheme == PaymentScheme.FasterPayments;
        }

        public bool GetResult(Account account, MakePaymentRequest request)
        {
            if (account == null || !account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.FasterPayments) || account.Balance < request.Amount)
            {
                return false;
            }

            return true;
        }
    }
}
