using ProjektSklepElektronika.Nowy_folder;

namespace ProjektSklepElektronika.Services
{
    public interface ICartService
    {
        decimal CalculateTotalValue(List<Cart> cartItems);
    }
}
