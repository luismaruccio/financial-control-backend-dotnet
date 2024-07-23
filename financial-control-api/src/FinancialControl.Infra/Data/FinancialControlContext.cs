using FinancialControl.Domain.Entities;
using FinancialControl.Infra.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace FinancialControl.Infra.Data
{
    public class FinancialControlContext(DbContextOptions<FinancialControlContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }

    }
}
