using System;
using System.Collections.Generic;
using System.Text;
using ClearBank.DeveloperTest.Services.PaymentStrategies;
using ClearBank.DeveloperTest.Types;
using FluentAssertions;
using NUnit.Framework;

namespace ClearBank.DeveloperTest.Tests.Services.PaymentStrategies
{
    [TestFixture]
    public class PaymentStrategyFactoryTests
    {
        private PaymentStrategyFactory _objectUnderTest;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            IPaymentStrategy bacsPaymentStrategy = new BacsPaymentStrategy();
            IPaymentStrategy fasterPaymentsPaymentStrategy = new FasterPaymentsPaymentStrategy();
            IPaymentStrategy chapsPaymentStrategy = new ChapsPaymentStrategy();
            _objectUnderTest = new PaymentStrategyFactory(new []{bacsPaymentStrategy, chapsPaymentStrategy, fasterPaymentsPaymentStrategy});
        }

        [Test]
        public void GivenIGetBacsPaymentStrategy_ThenBacsPaymentStrategyIsReturned()
        {
            var paymentStrategy = _objectUnderTest.GetPaymentStrategy(PaymentScheme.Bacs);

            (paymentStrategy is BacsPaymentStrategy).Should().BeTrue();
        }

        [Test]
        public void GivenIGetChapsPaymentStrategy_ThenChapsPaymentStrategyIsReturned()
        {
            var paymentStrategy = _objectUnderTest.GetPaymentStrategy(PaymentScheme.Chaps);

            (paymentStrategy is ChapsPaymentStrategy).Should().BeTrue();
        }

        [Test]
        public void GivenIGetFasterPaymentsPaymentStrategy_ThenFasterPaymentsPaymentStrategyIsReturned()
        {
            var paymentStrategy = _objectUnderTest.GetPaymentStrategy(PaymentScheme.FasterPayments);

            (paymentStrategy is FasterPaymentsPaymentStrategy).Should().BeTrue();
        }

        [Test]
        public void GivenIGetNonExistentPaymentStrategy_ThenArgumentExceptionIsThrown()
        {
            var paymentStrategyFactory = new PaymentStrategyFactory(new IPaymentStrategy []{new BacsPaymentStrategy()});

            Action act = () => paymentStrategyFactory.GetPaymentStrategy(PaymentScheme.Chaps);

            act.Should().Throw<ArgumentException>();
        }
    }
}
