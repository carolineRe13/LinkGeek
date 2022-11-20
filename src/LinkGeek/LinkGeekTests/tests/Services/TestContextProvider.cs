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
        
        public TestContextProvider(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        private DbContextOptions<ApplicationDbContext>? ContextOptions { get; set;  }
        private ApplicationDbContext? DbContext { get; set;  }

        public ApplicationDbContext GetContext()
        {
            if (DbContext != null)
            {
                return DbContext;
            }
            return new ApplicationDbContext(ContextOptions);
        }
    }
}
