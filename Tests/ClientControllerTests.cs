using Xunit;
using ProjektSklepElektronika.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace ProjektSklepElektronika.Tests
{
    public class ClientControllerTests
    {
        [Fact]
        public void GetClient_WithExistingEmail_ReturnsClient()
        {
            var controller = new ClientController();

            var result = controller.GetClient("jan.kowalski@gmail.com");

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var client = Assert.IsType<ProjektSklepElektronika.Nowy_folder.Client>(ok.Value);
            Assert.Equal("Jan", client.name);
        }

        [Fact]
        public void GetClient_WithInvalidEmail_ReturnsNotFound()
        {
            var controller = new ClientController();
            var result = controller.GetClient("nieistnieje@mail.com");
            Assert.IsType<NotFoundResult>(result.Result);
        }
    }
}