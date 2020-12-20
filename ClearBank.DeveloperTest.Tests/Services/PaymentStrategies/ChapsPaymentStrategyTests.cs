using System;
using System.Collections.Generic;
using System.Text;
using ClearBank.DeveloperTest.Services.PaymentStrategies;
using ClearBank.DeveloperTest.Types;
using FluentAssertions;
using NUnit.Framework;

namespace ClearBank.DeveloperTest.Tests.Services.PaymentStrategies
{
    public class ChapsPaymentStrategyTests
    {
        private ChapsPaymentStrategy _objectUnderTest;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _objectUnderTest = new ChapsPaymentStrategy();
        }

        [Test]
        public void GivenIGetResultForNonExistentAccount_ThenFalseIsReturned()
        {
            var result = _objectUnderTest.GetResult(null, new MakePaymentRequest());

            result.Should().BeFalse();
        }

        [Test]
        public void GivenIGetResultForAccountWithoutChapsPaymentAllowed_ThenFalseIsReturned()
        {
            var account = new Account() {AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments | AllowedPaymentSchemes.Bacs};
            var result = _objectUnderTest.GetResult(account, new MakePaymentRequest());

            result.Should().BeFalse();
        }

        [Test]
        public void GivenIGetResultForNonLiveAccount_ThenFalseIsReturned()
        {
            var account = new Account() { AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps | AllowedPaymentSchemes.Bacs, Status = AccountStatus.InboundPaymentsOnly };
            var result = _objectUnderTest.GetResult(account, new MakePaymentRequest());

            result.Should().BeFalse();
        }

        [Test]
        public void GivenIGetResultForLiveAccountWithChapsPaymentAllowed_ThenTrueIsReturned()
        {
            var account = new Account() { AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs | AllowedPaymentSchemes.Chaps, Status = AccountStatus.Live };
            var result = _objectUnderTest.GetResult(account, new MakePaymentRequest());

            result.Should().BeTrue();
        }
    }
}
