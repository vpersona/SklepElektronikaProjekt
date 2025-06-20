using ProjektSklepElektronika.Nowy_folder;
using ProjektSklepElektronika.Services;
using Microsoft.AspNetCore.Mvc;


[Route("api/[controller]")]
[ApiController]
public class CartController : ControllerBase
{
    private static List<Cart> _cart = new List<Cart>();
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Cart>> Get() => Ok(_cart);

    [HttpGet("{id}")]
    public ActionResult<Cart> Get(int id)
    {
        var item = _cart.FirstOrDefault(i => i.id == id);
        return item == null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public ActionResult<Cart> Post([FromBody] Cart newItem)
    {
        newItem.id = _cart.Any() ? _cart.Max(i => i.id) + 1 : 1;
        _cart.Add(newItem);
        return CreatedAtAction(nameof(Get), new { id = newItem.id }, newItem);
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] Cart updatedItem)
    {
        var item = _cart.FirstOrDefault(i => i.id == id);
        if (item == null) return NotFound();

        item.name = updatedItem.name;
        item.price = updatedItem.price;
        item.quantity = updatedItem.quantity;
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var item = _cart.FirstOrDefault(i => i.id == id);
        if (item == null) return NotFound();

        _cart.Remove(item);
        return NoContent();
    }

    // obliczanie wart całego koszyka
    [HttpGet("total")]
    public ActionResult<decimal> GetTotalCartValue()
    {
        var total =_cartService.CalculateTotalValue(_cart);
        return Ok(total);
    }
}
