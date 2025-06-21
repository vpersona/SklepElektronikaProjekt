using Microsoft.AspNetCore.TestHost;
using ProjektSklepElektronika.Nowy_folder;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace ProjektSklepElektronika.Tests
{
    public class CategoryCtrlITests
    {
        private readonly HttpClient _client;

        public CategoryCtrlITests()
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
        public async Task GetCategories_ShouldReturnOk()
        {
            var response = await _client.GetAsync("/api/category");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task PostCategory_ShouldCreateCategory()
        {
            var category = new Category
            {
                name = "Test Category"
            };

            var response = await _client.PostAsJsonAsync("/api/category", category);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task GetCategoryById_ShouldReturnCategory()
        {
            
            var newCategory = new Category { name = "Temp" };
            var postResponse = await _client.PostAsJsonAsync("/api/category", newCategory);
            var createdCategory = await postResponse.Content.ReadFromJsonAsync<Category>();

            var response = await _client.GetAsync($"/api/category/{createdCategory.id}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task PutCategory_ShouldReturnNoContent()
        {
            var newCategory = new Category { name = "OldName" };
            var postResponse = await _client.PostAsJsonAsync("/api/category", newCategory);
            var createdCategory = await postResponse.Content.ReadFromJsonAsync<Category>();

            var updatedCategory = new Category { id = createdCategory.id, name = "NewName" };
            var putResponse = await _client.PutAsJsonAsync($"/api/category/{createdCategory.id}", updatedCategory);
            Assert.Equal(HttpStatusCode.NoContent, putResponse.StatusCode);
        }

        [Fact]
        public async Task DeleteCategory_ShouldReturnNoContent()
        {
            var newCategory = new Category { name = "ToDelete" };
            var postResponse = await _client.PostAsJsonAsync("/api/category", newCategory);
            var createdCategory = await postResponse.Content.ReadFromJsonAsync<Category>();

            var deleteResponse = await _client.DeleteAsync($"/api/category/{createdCategory.id}");
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
        }
    }
}
