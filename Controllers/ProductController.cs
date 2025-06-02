using Microsoft.AspNetCore.Mvc;
using ProjektSklepElektronika.Nowy_folder;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjektSklepElektronika.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private static List<Products> _products = new List<Products>();

        // GET: api/Product
        [HttpGet]
        public ActionResult<IEnumerable<Products>> Get()
        {
            return Ok(_products.Where(p => !p.deleted));
        }

        // GET: api/Product/5
        [HttpGet("{id}")]
        public ActionResult<Products> Get(int id)
        {
            var product = _products.FirstOrDefault(p => p.id == id && !p.deleted);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        // POST: api/Product
        [HttpPost]
        public ActionResult<Products> Post([FromBody] Products newProduct)
        {
            newProduct.id = _products.Any() ? _products.Max(p => p.id) + 1 : 1;
            newProduct.created_at = DateTime.UtcNow;
            newProduct.updated_at = DateTime.UtcNow;
            newProduct.deleted = false;

            _products.Add(newProduct);
            return CreatedAtAction(nameof(Get), new { id = newProduct.id }, newProduct);
        }

        // PUT: api/Product/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Products updatedProduct)
        {
            var existingProduct = _products.FirstOrDefault(p => p.id == id && !p.deleted);
            if (existingProduct == null)
                return NotFound();

            
            existingProduct.name = updatedProduct.name;
            existingProduct.ean = updatedProduct.ean;
            existingProduct.price = updatedProduct.price;
            existingProduct.stock = updatedProduct.stock;
            existingProduct.category = updatedProduct.category;
            existingProduct.updated_by = updatedProduct.updated_by;
            existingProduct.updated_at = DateTime.UtcNow;

            return NoContent();
        }

        // DELETE: api/Product/5 (Soft delete)
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var product = _products.FirstOrDefault(p => p.id == id && !p.deleted);
            if (product == null)
                return NotFound();

            product.deleted = true;
            product.updated_at = DateTime.UtcNow;

            return NoContent();
        }


        

    }
}
