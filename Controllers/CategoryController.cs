using ProjektSklepElektronika.Nowy_folder;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private static List<Category> _categories = new List<Category>();

    [HttpGet]
    public ActionResult<IEnumerable<Category>> Get() => Ok(_categories);

    [HttpGet("{id}")]
    public ActionResult<Category> Get(int id)
    {
        var category = _categories.FirstOrDefault(c => c.id == id);
        return category == null ? NotFound() : Ok(category);
    }

    [HttpPost]
    public ActionResult<Category> Post([FromBody] Category newCategory)
    {
        newCategory.id = _categories.Any() ? _categories.Max(c => c.id) + 1 : 1;
        _categories.Add(newCategory);
        return CreatedAtAction(nameof(Get), new { id = newCategory.id }, newCategory);
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] Category updatedCategory)
    {
        var existing = _categories.FirstOrDefault(c => c.id == id);
        if (existing == null) return NotFound();

        existing.name = updatedCategory.name;
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var existing = _categories.FirstOrDefault(c => c.id == id);
        if (existing == null) return NotFound();

        _categories.Remove(existing);
        return NoContent();
    }
}
