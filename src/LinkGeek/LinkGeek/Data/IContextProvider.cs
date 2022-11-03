namespace LinkGeek.Data
{
    // All services are using IContextProvider to make them testable
    public interface IContextProvider
    {
        public ApplicationDbContext GetContext();
    }
}
