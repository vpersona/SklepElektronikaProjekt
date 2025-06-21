using Microsoft.AspNetCore.TestHost;
using ProjektSklepElektronika.Nowy_folder;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace ProjektSklepElektronika.Tests
{
    public class ProductCtrlITests
    {
        private readonly HttpClient _client;

        public ProductCtrlITests()
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
        public async Task GetProducts_ShouldReturnOk()
        {
            var response = await _client.GetAsync("/api/product");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task PostProduct_ShouldCreateProduct()
        {
            var product = new Products
            {
                name = "Test Product",
                ean = "1234567890123",
                price = 99.99m,
                stock = 10,
                category = new Category { id = 1, name = "Test Category" },
                deleted = false,
                created_by = Guid.NewGuid(),
                updated_by = Guid.NewGuid(),
                created_at = DateTime.UtcNow,
                updated_at = DateTime.UtcNow
            };

            var response = await _client.PostAsJsonAsync("/api/product", product);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task GetProductById_ShouldReturnProduct()
        {
            
            var product = new Products
            {
                name = "Temp Product",
                ean = "9876543210987",
                price = 50m,
                stock = 5,
                category = new Category { id = 1, name = "Temp Category" },
                deleted = false,
                created_by = Guid.NewGuid(),
                updated_by = Guid.NewGuid(),
                created_at = DateTime.UtcNow,
                updated_at = DateTime.UtcNow
            };

            var postResponse = await _client.PostAsJsonAsync("/api/product", product);
            var createdProduct = await postResponse.Content.ReadFromJsonAsync<Products>();

            var response = await _client.GetAsync($"/api/product/{createdProduct.id}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task PutProduct_ShouldReturnNoContent()
        {
            var product = new Products
            {
                name = "Old Product",
                ean = "1111111111111",
                price = 20m,
                stock = 3,
                category = new Category { id = 1, name = "Old Category" },
                deleted = false,
                created_by = Guid.NewGuid(),
                updated_by = Guid.NewGuid(),
                created_at = DateTime.UtcNow,
                updated_at = DateTime.UtcNow
            };

            var postResponse = await _client.PostAsJsonAsync("/api/product", product);
            var createdProduct = await postResponse.Content.ReadFromJsonAsync<Products>();

            var updatedProduct = new Products
            {
                id = createdProduct.id,
                name = "Updated Product",
                ean = "2222222222222",
                price = 25m,
                stock = 4,
                category = new Category { id = 1, name = "Updated Category" },
                updated_by = Guid.NewGuid()
            };

            var putResponse = await _client.PutAsJsonAsync($"/api/product/{createdProduct.id}", updatedProduct);
            Assert.Equal(HttpStatusCode.NoContent, putResponse.StatusCode);
        }

        [Fact]
        public async Task DeleteProduct_ShouldReturnNoContent()
        {
            var product = new Products
            {
                name = "Delete Product",
                ean = "3333333333333",
                price = 30m,
                stock = 2,
                category = new Category { id = 1, name = "Delete Category" },
                deleted = false,
                created_by = Guid.NewGuid(),
                updated_by = Guid.NewGuid(),
                created_at = DateTime.UtcNow,
                updated_at = DateTime.UtcNow
            };

            var postResponse = await _client.PostAsJsonAsync("/api/product", product);
            var createdProduct = await postResponse.Content.ReadFromJsonAsync<Products>();

            var deleteResponse = await _client.DeleteAsync($"/api/product/{createdProduct.id}");
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
        }
    }
}
