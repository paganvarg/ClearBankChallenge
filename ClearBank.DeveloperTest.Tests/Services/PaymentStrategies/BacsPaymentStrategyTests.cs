using System;
using System.Collections.Generic;
using System.Text;
using ClearBank.DeveloperTest.Services.PaymentStrategies;
using ClearBank.DeveloperTest.Types;
using FluentAssertions;
using NUnit.Framework;

namespace ClearBank.DeveloperTest.Tests.Services.PaymentStrategies
{
    public class BacsPaymentStrategyTests
    {
        private BacsPaymentStrategy _objectUnderTest;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _objectUnderTest = new BacsPaymentStrategy();
        }

        [Test]
        public void GivenIGetResultForNonExistentAccount_ThenFalseIsReturned()
        {
            var result = _objectUnderTest.GetResult(null, new MakePaymentRequest());

            result.Should().BeFalse();
        }

        [Test]
        public void GivenIGetResultForAccountWithoutBacsPaymentAllowed_ThenFalseIsReturned()
        {
            var account = new Account() {AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments | AllowedPaymentSchemes.Chaps};
            var result = _objectUnderTest.GetResult(account, new MakePaymentRequest());

            result.Should().BeFalse();
        }

        [Test]
        public void GivenIGetResultForAccountWithBacsPaymentAllowed_ThenTrueIsReturned()
        {
            var account = new Account() { AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs | AllowedPaymentSchemes.Chaps };
            var result = _objectUnderTest.GetResult(account, new MakePaymentRequest());

            result.Should().BeTrue();
        }
    }
}
