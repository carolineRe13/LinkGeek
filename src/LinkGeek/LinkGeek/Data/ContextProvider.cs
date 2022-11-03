namespace LinkGeek.Data
{
    public class ContextProvider : IContextProvider
    {
        public ApplicationDbContext GetContext()
        {
            return new ApplicationDbContext();
        }
    }
}
