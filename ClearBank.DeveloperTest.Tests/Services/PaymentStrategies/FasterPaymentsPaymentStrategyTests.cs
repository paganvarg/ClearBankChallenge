using System;
using System.Collections.Generic;
using System.Text;
using ClearBank.DeveloperTest.Services.PaymentStrategies;
using ClearBank.DeveloperTest.Types;
using FluentAssertions;
using NUnit.Framework;

namespace ClearBank.DeveloperTest.Tests.Services.PaymentStrategies
{
    public class FasterPaymentsPaymentStrategyTests
    {
        private FasterPaymentsPaymentStrategy _objectUnderTest;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _objectUnderTest = new FasterPaymentsPaymentStrategy();
        }

        [Test]
        public void GivenIGetResultForNonExistentAccount_ThenFalseIsReturned()
        {
            var result = _objectUnderTest.GetResult(null, new MakePaymentRequest());

            result.Should().BeFalse();
        }

        [Test]
        public void GivenIGetResultForAccountWithoutFasterPaymentsPaymentAllowed_ThenFalseIsReturned()
        {
            var account = new Account() {AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps | AllowedPaymentSchemes.Bacs};
            var result = _objectUnderTest.GetResult(account, new MakePaymentRequest());

            result.Should().BeFalse();
        }

        [Test]
        public void GivenIGetResultForAccountWithLowerBalanceThanRequestedAmount_ThenFalseIsReturned()
        {
            var account = new Account() { AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments | AllowedPaymentSchemes.Bacs, Balance = 9.99m };
            var result = _objectUnderTest.GetResult(account, new MakePaymentRequest(){Amount = 10.0m});

            result.Should().BeFalse();
        }

        [Test]
        public void GivenIGetResultForLiveAccountWithFasterPaymentsPaymentAllowedAndBalanceHigherThanRequestedAmount_ThenTrueIsReturned()
        {
            var account = new Account() { AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments | AllowedPaymentSchemes.Chaps, Balance = 10.0m };
            var result = _objectUnderTest.GetResult(account, new MakePaymentRequest() {Amount = 9.99m});

            result.Should().BeTrue();
        }

        [Test]
        public void GivenIGetResultForLiveAccountWithFasterPaymentsPaymentAllowedAndBalanceEqualRequestedAmount_ThenTrueIsReturned()
        {
            var account = new Account() { AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs | AllowedPaymentSchemes.FasterPayments, Balance = 10.0m };
            var result = _objectUnderTest.GetResult(account, new MakePaymentRequest() { Amount = 10.0m });

            result.Should().BeTrue();
        }
    }
}
