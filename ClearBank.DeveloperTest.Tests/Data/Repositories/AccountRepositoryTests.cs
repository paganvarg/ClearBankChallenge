using System;
using System.Collections.Generic;
using System.Text;
using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Data.Repositories;
using ClearBank.DeveloperTest.Types;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace ClearBank.DeveloperTest.Tests.Data.Repositories
{
    [TestFixture]
    public class AccountRepositoryTests
    {
        private ClearBankContext _context;
        private IAccountRepository _objectUnderTest;
        private const string ExistingAccountNumber = "1234";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var options = new DbContextOptionsBuilder()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new ClearBankContext(options);
            _objectUnderTest = new AccountRepository(_context);
            _objectUnderTest.Add(new Account()
            {
                AccountNumber = ExistingAccountNumber,
                AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs | AllowedPaymentSchemes.Chaps,
                Balance = 102.50m,
                Status = AccountStatus.Live
            });
            _objectUnderTest.Add(new Account()
            {
                AccountNumber = "2345",
                AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments | AllowedPaymentSchemes.Chaps,
                Balance = 502.60m,
                Status = AccountStatus.InboundPaymentsOnly
            });
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _context?.Dispose();
        }

        [Test]
        public void GivenISearchForExistingAccount_ThenAccountIsReturned()
        {
            var account = _objectUnderTest.GetAccountByAccountNumber(ExistingAccountNumber);

            account.Should().NotBeNull();
            account.AccountNumber.Should().Be(ExistingAccountNumber);
        }

        [Test]
        public void GivenISearchForNonExistingAccount_ThenNoAccountIsReturned()
        {
            const string nonExistingAccountNumber = "3456";
            var account = _objectUnderTest.GetAccountByAccountNumber(nonExistingAccountNumber);

            account.Should().BeNull();
        }

        [Test]
        public void GivenIUpdateAccount_ThenUpdatedAccountIsPersistedInDatabase()
        {
            // ARRANGE
            var account = _objectUnderTest.GetAccountByAccountNumber(ExistingAccountNumber);
            var accountAllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments;
            account.AllowedPaymentSchemes = accountAllowedPaymentSchemes;
            var accountBalance = 50.70m;
            account.Balance = accountBalance;
            var accountStatus = AccountStatus.Disabled;
            account.Status = accountStatus;

            // ACT
            _objectUnderTest.Update(account);

            // ASSERT
            account = _objectUnderTest.GetAccountByAccountNumber(ExistingAccountNumber);
            account.Should().NotBeNull();
            account.AccountNumber.Should().Be(ExistingAccountNumber);
            account.AllowedPaymentSchemes.Should().Be(accountAllowedPaymentSchemes);
            account.Balance.Should().Be(accountBalance);
            account.Status.Should().Be(accountStatus);
        }
    }
}
