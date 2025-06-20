using Xunit;
using ProjektSklepElektronika.Controllers;
using ProjektSklepElektronika.Nowy_folder;
using Microsoft.AspNetCore.Mvc;
using ProjektSklepElektronika.Services;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace ProjektSklepElektronika.Tests
{
    public class CartControllerTests
    {
        private readonly CartController _controller;
        private readonly Mock<ICartService> _mockCartService;

        public CartControllerTests()
        {
            _mockCartService = new Mock<ICartService>();
            _controller = new CartController(_mockCartService.Object);
        }

        [Fact]
        public void Post_AddsNewItem_ReturnsCreatedItem()
        {
            var newItem = new Cart { name = "Produkt", price = 100, quantity = 2 };

            var result = _controller.Post(newItem);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedItem = Assert.IsType<Cart>(createdResult.Value);
            Assert.Equal(newItem.name, returnedItem.name);
        }

        [Fact]
        public void GetTotalCartValue_ReturnsCorrectValue()
        {
            var cart = new List<Cart>
            {
                new Cart { name = "P1", quantity = 1, price = 100 },
                new Cart { name = "P2", quantity = 10, price = 200 },
            };
            _mockCartService.Setup(s => s.CalculateTotalValue(It.IsAny<List<Cart>>()))
                .Returns(1900); // zakładana wartość z rabatami

            var result = _controller.GetTotalCartValue();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(1900m, okResult.Value); // m = decimal literal

        }
    }
}