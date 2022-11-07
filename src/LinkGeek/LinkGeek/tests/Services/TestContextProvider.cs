using LinkGeek.Data;
using Microsoft.EntityFrameworkCore;

namespace LinkGeek.tests.Services
{
    public class TestContextProvider : IContextProvider
    {
        public TestContextProvider(DbContextOptions<ApplicationDbContext> contextOptions)
        {
            ContextOptions = contextOptions;
        }

        private DbContextOptions<ApplicationDbContext> ContextOptions { get; set;  }

        public ApplicationDbContext GetContext()
        {
            return new ApplicationDbContext(ContextOptions);
        }
    }
}
