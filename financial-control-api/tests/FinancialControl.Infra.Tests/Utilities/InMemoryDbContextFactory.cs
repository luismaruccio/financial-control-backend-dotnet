using FinancialControl.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace FinancialControl.Infra.Tests.Utilities
{
    public static class InMemoryDbContextFactory
    {
        public static FinancialControlContext Create()
        {
            var options = new DbContextOptionsBuilder<FinancialControlContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new FinancialControlContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        public static void Destroy(FinancialControlContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
