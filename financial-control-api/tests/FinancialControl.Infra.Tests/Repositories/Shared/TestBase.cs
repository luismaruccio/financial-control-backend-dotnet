using FinancialControl.Infra.Data;
using FinancialControl.Infra.Tests.Utilities;

namespace FinancialControl.Infra.Tests.Repositories.Shared
{
    public abstract class TestBase : IDisposable
    {
        protected readonly FinancialControlContext Context;

        protected TestBase()
        {
            Context = InMemoryDbContextFactory.Create();
        }

        public void Dispose()
        {
            InMemoryDbContextFactory.Destroy(Context);
            GC.SuppressFinalize(this);
        }
    }
}
