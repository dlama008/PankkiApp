// PankkiApp/Models/Customer.cs
namespace PankkiApp.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // Alustettu tyhjäksi merkkijonoksi
        public string Email { get; set; } = string.Empty; // Alustettu tyhjäksi merkkijonoksi
    }
}
