using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services.PaymentStrategies
{
    public interface IPaymentStrategy
    {
        bool IsApplicable(PaymentScheme paymentScheme);
        bool GetResult(Account account, MakePaymentRequest request);
    }
}
