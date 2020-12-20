using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services.PaymentStrategies
{
    public class PaymentStrategyFactory : IPaymentStrategyFactory
    {
        private readonly IPaymentStrategy[] _paymentStrategies;

        public PaymentStrategyFactory(IPaymentStrategy[] paymentStrategies)
        {
            _paymentStrategies = paymentStrategies;
        }

        public IPaymentStrategy GetPaymentStrategy(PaymentScheme paymentScheme)
        {
            var paymentStrategy = _paymentStrategies.FirstOrDefault(s => s.IsApplicable(paymentScheme));

            if (paymentStrategy == null)
            {
                // In production code I would probably throw a more specific exception
                throw new ArgumentException($"No {nameof(IPaymentStrategy)} found for {paymentScheme} payment scheme.");
            }

            return paymentStrategy;
        }
    }
}
