using ProjektSklepElektronika.Nowy_folder;

namespace ProjektSklepElektronika.Services
{
    public class CartService : ICartService
    {
        public decimal CalculateTotalValue(List<Cart> cartItems)
        {
            decimal total = 0;

            foreach (var item in cartItems)
            {
                decimal itemTotal = item.price * item.quantity;

                // Promocja: -10% jeśli ilość >= 10 sztuk
                if (item.quantity >= 10)
                {
                    itemTotal *= 0.9m; // 10% zniżki
                }

                total += itemTotal;
            }

            // Promocja ogólna: -5% jeśli wartość koszyka przekracza 1000 zł
            if (total > 1000)
            {
                total *= 0.95m; // 5% zniżki
            }

            return total;
        }
    }
}

