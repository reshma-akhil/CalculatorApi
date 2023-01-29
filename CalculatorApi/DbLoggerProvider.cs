namespace CalculatorApi
{
    public class DbLoggerProvider : ILoggerProvider
    {
        IConfiguration configuration;
        public DbLoggerProvider(IConfiguration configuration) 
        { 
            this.configuration = configuration;
        }
        public ILogger CreateLogger(string categoryName)
        {
            return new DbLogger(configuration);
        }

        public void Dispose()
        {
        }
    }
}
