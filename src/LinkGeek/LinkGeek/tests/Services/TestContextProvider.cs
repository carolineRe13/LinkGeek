using LinkGeek.Data;

namespace LinkGeek.tests.Services
{
    public class TestContextProvider : IContextProvider
    {
        public ApplicationDbContext? Context { get; set; }

        public ApplicationDbContext GetContext()
        {
            return Context;
        }
    }
}
