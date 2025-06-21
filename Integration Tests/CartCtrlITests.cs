using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using Xunit;
using ProjektSklepElektronika.Nowy_folder;
using ProjektSklepElektronika.Services;

namespace ProjektSklepElektronika.Tests
{
    public class CartCtrlITests
    {
        private readonly HttpClient _client;

        public CartCtrlITests()
        {
            var builder = new WebHostBuilder()
                .ConfigureServices(services =>
                {
                   
                    services.AddScoped<ICartService, CartService>();
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
        public async Task GetCart_ShouldReturnOk()
        {
            var response = await _client.GetAsync("/api/cart");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task PostCart_ShouldCreateItem()
        {
            var item = new Cart
            {
                name = "Test Product",
                price = 100,
                quantity = 3
            };

            var response = await _client.PostAsJsonAsync("/api/cart", item);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task GetTotal_ShouldReturnValue()
        {
            var response = await _client.GetAsync("/api/cart/total");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var total = await response.Content.ReadFromJsonAsync<decimal>();
            Assert.NotNull(total);
            Assert.True(total >= 0);
        }
    }
}
