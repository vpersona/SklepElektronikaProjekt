using Xunit;
using Microsoft.AspNetCore.Mvc;
using ProjektSklepElektronika.Controllers;
using ProjektSklepElektronika.Nowy_folder;
using System;

namespace ProjektSklepElektronika.Tests
{
    public class ProductControllerTests
    {
        [Fact]
        public void Post_CreatesProduct_ReturnsCreated()
        {
            var controller = new ProductController();
            var product = new Products
            {
                name = "Monitor",
                ean = "1234567890123",
                price = 999.99m,
                stock = 10,
                category = new Category { name = "Monitory" },
                created_by = Guid.NewGuid(),
                updated_by = Guid.NewGuid()
            };

            var result = controller.Post(product);

            var created = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returned = Assert.IsType<Products>(created.Value);
            Assert.Equal("Monitor", returned.name);
        }

        [Fact]
        public void Delete_SoftDeletesProduct_ReturnsNoContent()
        {
            var controller = new ProductController();

            var product = new Products
            {
                name = "Tablet",
                ean = "2223334445556",
                price = 599.99m,
                stock = 5,
                category = new Category { name = "Tablety" },
                created_by = Guid.NewGuid(),
                updated_by = Guid.NewGuid()
            };
            controller.Post(product);

            var result = controller.Delete(product.id);
            Assert.IsType<NoContentResult>(result);
        }
    }
}