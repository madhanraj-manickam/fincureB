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
    }
}
