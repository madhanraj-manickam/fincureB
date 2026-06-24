using FinCure.Models;
using Microsoft.EntityFrameworkCore;

namespace FinCure.Data
{
    public class FinCureDBContext : DbContext
    {
        public FinCureDBContext(DbContextOptions<FinCureDBContext> options) : base(options)
        {

        }
        public DbSet<Users> Users { get; set; }

        public DbSet<Income> Incomes { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Investment> Investments { get; set; }

        public DbSet<Accounts> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Budget> Budgets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ensure the User email is unique at the database level
            modelBuilder.Entity<Users>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}
