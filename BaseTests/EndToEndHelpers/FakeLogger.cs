using Serilog;

namespace BaseTests.EndToEndHelpers
{
    public class FakeLogger
    {
        private readonly ILogger _logger;

        public FakeLogger() => _logger = new LoggerConfiguration().CreateLogger();

        public ILogger GetLogger() => _logger;
    }
}