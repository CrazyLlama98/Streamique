using Microsoft.Extensions.Configuration;

namespace Server.IntegrationTests
{
    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration env) : base(env)
        {

        }
    }
}
