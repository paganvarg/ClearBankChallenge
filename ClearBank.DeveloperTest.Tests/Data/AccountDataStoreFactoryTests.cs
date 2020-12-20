using System;
using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Data.Repositories;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;

namespace ClearBank.DeveloperTest.Tests.Data
{
    [TestFixture]
    public class AccountDataStoreFactoryTests
    {
        private AccountDataStoreFactory _objectUnderTest;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            IAccountDataStore accountDataStore = new AccountDataStore(A.Fake<IAccountRepository>());
            IAccountDataStore backupAccountDataStore = new BackupAccountDataStore(A.Fake<IAccountRepository>());
            _objectUnderTest = new AccountDataStoreFactory(new [] {accountDataStore, backupAccountDataStore});
        }

        [Test]
        public void GivenIGetAccountDataStoreForStandardDataStoreType_ThenStandardDataStoreIsReturned()
        {
            var accountDataStore = _objectUnderTest.GetAccountDataStore("Standard");

            (accountDataStore is AccountDataStore).Should().BeTrue();
        }

        [Test]
        public void GivenIGetAccountDataStoreForBackupDataStoreType_ThenBackupDataStoreIsReturned()
        {
            var accountDataStore = _objectUnderTest.GetAccountDataStore("Backup");

            (accountDataStore is BackupAccountDataStore).Should().BeTrue();
        }

        [Test]
        public void GivenIGetAccountDataStoreForNonExistentDataStoreType_ThenArgumentExceptionIsThrown()
        {
            var accountDataStoreFactory = new AccountDataStoreFactory(new []{ new AccountDataStore(A.Fake<IAccountRepository>())});

            Action act = () => accountDataStoreFactory.GetAccountDataStore("Backup");

            act.Should().Throw<ArgumentException>();
        }
    }
}
