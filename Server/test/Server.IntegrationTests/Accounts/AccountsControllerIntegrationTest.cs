using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Server.Data.DTOs;
using Xunit;

namespace Server.IntegrationTests.Accounts
{
    public class AccountsControllerIntegrationTest
    {
        private readonly TestServer _server;

        public AccountsControllerIntegrationTest()
        {
            var webHostBuilder = WebHost.CreateDefaultBuilder();
            webHostBuilder.UseContentRoot(Directory.GetCurrentDirectory());
            webHostBuilder.UseStartup<TestStartup>();
            _server = new TestServer(webHostBuilder);
        }

        [Fact]
        public async Task ValidRegister()
        {
            var register = new RegistrationDto() { AccountImageId = 3, Email = "test-integration@test.com", NickName = "Helko", Password = "test1", Phone = "0755415977" };
            var content = new StringContent(JsonConvert.SerializeObject(register), Encoding.UTF8, "application/json");
            var response = await _server.CreateClient()
               .PostAsync("api/accounts/register", content);
            response.EnsureSuccessStatusCode();
        }
    }
}
