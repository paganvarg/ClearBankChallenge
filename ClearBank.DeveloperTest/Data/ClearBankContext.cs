using System;
using System.Collections.Generic;
using System.Text;
using ClearBank.DeveloperTest.Types;
using Microsoft.EntityFrameworkCore;

namespace ClearBank.DeveloperTest.Data
{
    public class ClearBankContext : DbContext
    {
        public ClearBankContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SetAccounts(modelBuilder);
        }

        private void SetAccounts(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Accounts");
                entity.HasKey(x => x.AccountNumber);
                entity.Property(x => x.AccountNumber).HasMaxLength(50);
                entity.Property(x => x.AllowedPaymentSchemes).IsRequired();
                entity.Property(x => x.Balance).IsRequired();
                entity.Property(x => x.Status).IsRequired();
            });
        }
    }
}
