using Xunit;
using ProjektSklepElektronika.Controllers;
using ProjektSklepElektronika.Nowy_folder;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ProjektSklepElektronika.Tests
{
    public class CategoryControllerTests
    {
        [Fact]
        public void Post_AddsCategory_ReturnsCreatedCategory()
        {
            var controller = new CategoryController();
            var category = new Category { name = "Laptopy" };

            var result = controller.Post(category);

            var created = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returned = Assert.IsType<Category>(created.Value);
            Assert.Equal("Laptopy", returned.name);
        }

        [Fact]
        public void Get_WithWrongId_ReturnsNotFound()
        {
            var controller = new CategoryController();
            var result = controller.Get(999);
            Assert.IsType<NotFoundResult>(result.Result);
        }
    }
}