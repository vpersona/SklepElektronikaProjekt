using Microsoft.AspNetCore.TestHost;
using ProjektSklepElektronika.Nowy_folder;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace ProjektSklepElektronika.Tests
{
    public class ClientCtrlITests
    {
        private readonly HttpClient _client;

        public ClientCtrlITests()
        {
            var builder = new WebHostBuilder()
                .ConfigureServices(services =>
                {
                    services.AddControllers();
                    services.AddEndpointsApiExplorer();
                    services.AddSwaggerGen();
                })
                .Configure(app =>
                {
                    app.UseRouting();
                    app.UseSwagger();
                    app.UseSwaggerUI();
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapControllers();
                    });
                });

            var server = new TestServer(builder);
            _client = server.CreateClient();
        }

        [Fact]
        public async Task GetClients_ShouldReturnOk()
        {
            var response = await _client.GetAsync("/api/client");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetClientByEmail_ShouldReturnClient()
        {
            
            var email = "jan.kowalski@gmail.com";
            var response = await _client.GetAsync($"/api/client/{email}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetClientByEmail_ShouldReturnNotFound()
        {
            var response = await _client.GetAsync($"/api/client/notfound@example.com");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
