using ProjektSklepElektronika.Nowy_folder;

[Route("api/[controller]")]
[ApiController]
public class ClientController : ControllerBase
{
    private static List<Client> _clients = new List<Client>
    {
        new Client {
            name = "Jan", surname = "Kowalski", city = "Warszawa", country = "Polska",
            postal_code = "00-001", house_nb = "12A", email = "jan.kowalski@gmail.com",
            phone_number = "123456789"
        }
    };

    // GET: api/Client
    [HttpGet]
    public ActionResult<IEnumerable<Client>> GetClients()
    {
        return Ok(_clients);
    }

    // GET: api/Client/1
    [HttpGet("{email}")]
    public ActionResult<Client> GetClient(string email)
    {
        var client = _clients.FirstOrDefault(c => c.email == email);
        if (client == null)
            return NotFound();

        return Ok(client);
    }
}
