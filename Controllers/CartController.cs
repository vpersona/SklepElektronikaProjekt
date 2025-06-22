using ProjektSklepElektronika.Nowy_folder;
using ProjektSklepElektronika.Services;
using Microsoft.AspNetCore.Mvc;
using ProjektSklepElektronika.Models.Auth;


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

    // finalizacja zamowienia
    [HttpPost("checkout")]
    public ActionResult Checkout([FromBody] CheckoutDto dto)
    {
        if (string.IsNullOrEmpty(dto.Email))
            return BadRequest("Email jest wymagany");

        if (!_cart.Any())
            return BadRequest("Koszyk jest pusty");

        var total = _cartService.CalculateTotalValue(_cart);
        var items = _cart.Select(i => new { i.name, i.quantity, i.price }).ToList();

        // Wygeneruj dane faktury
        var invoice = new
        {
            InvoiceNumber = Guid.NewGuid().ToString().Substring(0, 8).ToUpper(), // losowy numer faktury
            Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
            DocumentType = "Faktura",
            Email = dto.Email,
            Items = items,
            Total = total,
            Message = "Dziękujemy za zakupy!"
        };

        //wysyłka na maila
        Console.WriteLine($"=== FAKTURA WYSŁANA NA EMAIL: {dto.Email} ===");
        Console.WriteLine($"Numer: {invoice.InvoiceNumber}");
        Console.WriteLine($"Data: {invoice.Date}");
        Console.WriteLine($"Typ: {invoice.DocumentType}");
        Console.WriteLine($"Pozycje:");
        foreach (var item in items)
        {
            Console.WriteLine($"- {item.name} x {item.quantity} = {item.price * item.quantity} zł");
        }
        Console.WriteLine($"Razem: {total} zł\n");

        
        _cart.Clear();

        return Ok(invoice);
    }
}
