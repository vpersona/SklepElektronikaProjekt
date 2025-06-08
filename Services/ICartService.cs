using ProjektSklepElektronika.Nowy_folder;

namespace ProjektSklepElektronika.Services
{
    public interface ICartService
    {
        decimal CalculateTotalValue(List<ProjektSklepElektronika.Nowy_folder.Cart> cartItems);
    }
}
