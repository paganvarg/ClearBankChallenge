using System;
using System.Collections.Generic;
using System.Text;
using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Data.Repositories;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Services.PaymentStrategies;
using ClearBank.DeveloperTest.Types;
using FakeItEasy;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace ClearBank.DeveloperTest.Tests.Services
{
    [TestFixture]
    public class PaymentServiceTests
    {
        private ClearBankContext _context;
        private ClearBankContext _backupContext;
        private IConfigurationService _configurationService;
        private PaymentService _objectUnderTest;
        private AccountRepository _backupAccountRepository;
        private AccountRepository _accountRepository;
        private const string ExistingAccountNumber = "1234";
        private const decimal RequestAmount = 13.50m;
        private const decimal ExistingAccountBalance = 102.50m;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new ClearBankContext(options);
            _accountRepository = new AccountRepository(_context);
            _accountRepository.Add(new Account()
            {
                AccountNumber = ExistingAccountNumber,
                AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs | AllowedPaymentSchemes.Chaps,
                Balance = ExistingAccountBalance,
                Status = AccountStatus.Live
            });
            _accountRepository.Add(new Account()
            {
                AccountNumber = "2345",
                AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments | AllowedPaymentSchemes.Chaps,
                Balance = 502.60m,
                Status = AccountStatus.InboundPaymentsOnly
            });

            var backupOptions = new DbContextOptionsBuilder()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _backupContext = new ClearBankContext(backupOptions);
            _backupAccountRepository = new AccountRepository(_backupContext);
            _backupAccountRepository.Add(new Account()
            {
                AccountNumber = ExistingAccountNumber,
                AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs | AllowedPaymentSchemes.Chaps,
                Balance = ExistingAccountBalance,
                Status = AccountStatus.Live
            });
            _backupAccountRepository.Add(new Account()
            {
                AccountNumber = "2345",
                AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments | AllowedPaymentSchemes.Chaps,
                Balance = 502.60m,
                Status = AccountStatus.InboundPaymentsOnly
            });

            var accountDataStoreFactory = new AccountDataStoreFactory(new IAccountDataStore[]
                {new AccountDataStore(_accountRepository), new BackupAccountDataStore(_backupAccountRepository)});
            _configurationService = A.Fake<IConfigurationService>();

            var paymentStrategyFactory = new PaymentStrategyFactory(new IPaymentStrategy[]{new BacsPaymentStrategy(), new ChapsPaymentStrategy(), new FasterPaymentsPaymentStrategy()});
            _objectUnderTest = new PaymentService(accountDataStoreFactory, paymentStrategyFactory,_configurationService);
        }

        [TearDown]
        public void TearDown()
        {
            _context?.Dispose();
            _backupContext?.Dispose();
        }

        [Test]
        public void GivenIMakeValidPayment_ThenSuccessIsReturnedAndRequestedAmountIsDeducted()
        {
            A.CallTo(() => _configurationService.GetDataStoreType()).Returns("Standard");
            var request = new MakePaymentRequest()
            {
                Amount = RequestAmount,
                DebtorAccountNumber = ExistingAccountNumber,
                PaymentScheme = PaymentScheme.Bacs
            };

            var response = _objectUnderTest.MakePayment(request);

            response.Success.Should().BeTrue();
            var account = _accountRepository.GetAccountByAccountNumber(ExistingAccountNumber);
            account.Balance.Should().Be(ExistingAccountBalance - RequestAmount);
            var backupAccount = _backupAccountRepository.GetAccountByAccountNumber(ExistingAccountNumber);
            backupAccount.Balance.Should().Be(ExistingAccountBalance);
        }

        [Test]
        public void GivenIMakeValidPaymentFromBackupAccount_ThenSuccessIsReturnedAndRequestedAmountIsDeducted()
        {
            A.CallTo(() => _configurationService.GetDataStoreType()).Returns("Backup");
            var request = new MakePaymentRequest()
            {
                Amount = RequestAmount,
                DebtorAccountNumber = ExistingAccountNumber,
                PaymentScheme = PaymentScheme.Bacs
            };

            var response = _objectUnderTest.MakePayment(request);

            response.Success.Should().BeTrue();
            var account = _accountRepository.GetAccountByAccountNumber(ExistingAccountNumber);
            account.Balance.Should().Be(ExistingAccountBalance);
            var backupAccount = _backupAccountRepository.GetAccountByAccountNumber(ExistingAccountNumber);
            backupAccount.Balance.Should().Be(ExistingAccountBalance - RequestAmount);
        }

        [Test]
        public void GivenIMakeInvalidPaymentFromBackupAccount_ThenFailureIsReturnedAndRequestedAmountIsNotDeducted()
        {
            A.CallTo(() => _configurationService.GetDataStoreType()).Returns("Backup");
            var request = new MakePaymentRequest()
            {
                Amount = RequestAmount,
                DebtorAccountNumber = ExistingAccountNumber,
                PaymentScheme = PaymentScheme.FasterPayments
            };

            var response = _objectUnderTest.MakePayment(request);

            response.Success.Should().BeFalse();
            var account = _accountRepository.GetAccountByAccountNumber(ExistingAccountNumber);
            account.Balance.Should().Be(ExistingAccountBalance);
            var backupAccount = _backupAccountRepository.GetAccountByAccountNumber(ExistingAccountNumber);
            backupAccount.Balance.Should().Be(ExistingAccountBalance);
        }
    }
}
