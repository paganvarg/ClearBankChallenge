using System;
using System.Collections.Generic;
using System.Text;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services.PaymentStrategies
{
    public interface IPaymentStrategyFactory
    {
        IPaymentStrategy GetPaymentStrategy(PaymentScheme paymentScheme);
    }
}
