using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services.PaymentStrategies
{
    public class BacsPaymentStrategy : IPaymentStrategy
    {
        public bool IsApplicable(PaymentScheme paymentScheme)
        {
            return paymentScheme == PaymentScheme.Bacs;
        }

        public bool GetResult(Account account, MakePaymentRequest request)
        {
            if (account == null || !account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Bacs))
            {
                return false;
            }

            return true;
        }
    }
}
