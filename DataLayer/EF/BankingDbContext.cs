using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EF
{
    public class BankingDbContext : DbContext
    {
        private readonly string? _connectionString;
        public BankingDbContext(DbContextOptions<BankingDbContext> options) : base(options) 
        {
            Database.EnsureCreated();    
        }

        public BankingDbContext(string connectionString)
        {
            _connectionString = connectionString;
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=BankingDb;Trusted_Connection=True;MultipleActiveResultSets=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BankAccount>().Property(ba => ba.AccountType).HasConversion<string>();
            modelBuilder.Entity<BankAccount>()
                .Property(b => b.Balance)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Credit>()
                .Property(c => c.SumCredit)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Transaction>()
                .Property(t => t.SumTransaction)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Credit>().Property(credit => credit.Status).HasConversion<string>();

            modelBuilder.Entity<BankAccount>()
                .HasOne(b => b.Account)
                .WithMany(a => a.BankAccounts)
                .HasForeignKey(b => b.AccountId);

            modelBuilder.Entity<Account>().Property(account => account.Role).HasConversion<string>();



            modelBuilder.Entity<BankAccount>()
                .ToTable(t => t.HasCheckConstraint("Balance", "Balance >= 0"));

            modelBuilder.Entity<Account>()
                .HasIndex(a => a.Email)
                .IsUnique();

            modelBuilder.Entity<Account>()
                .HasIndex(a => a.UserName)
                .IsUnique();

            modelBuilder.Entity<Account>()
                .HasIndex(a => a.PhoneNumber)
                .IsUnique();

            modelBuilder.Entity<Transaction>()
                .ToTable(t => t.HasCheckConstraint("SumTransaction", "SumTransaction >= 0"));

            modelBuilder.Entity<Transaction>().Property(t => t.TransactionType).HasConversion<string>();

            // добавим дефолтного модератора для демонстрации
            modelBuilder.Entity<Account>().HasData(new Account()
            {
                Id = 40,
                UserName = "manager",
                PhoneNumber = "+375290000000",
                Email = "manager@mail.ru",
                HashPassword = string.Join("", SHA256.HashData(Encoding.UTF8.GetBytes("12345")).Select(x => x.ToString("X2"))),
                Role = Roles.Manager,
                DateBirth = DateTime.Parse("01/01/2002")
            });
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Credit> Credits { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }

    public class BankingDbContextFactory : IDesignTimeDbContextFactory<BankingDbContext>
    {
        public BankingDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BankingDbContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=BankingDb;Trusted_Connection=True;MultipleActiveResultSets=true", b => b.MigrationsAssembly("DataLayer"));

            return new BankingDbContext(optionsBuilder.Options);
        }
    }
}
