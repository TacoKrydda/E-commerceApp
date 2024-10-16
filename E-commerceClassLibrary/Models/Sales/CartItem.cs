using E_commerceClassLibrary.Models.Production;
using System.Text.Json.Serialization;

namespace E_commerceClassLibrary.Models.Sales
{
    public class CartItem
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        [JsonIgnore]
        public Order? Order { get; set; }
        [JsonIgnore]
        public Product? Product { get; set; }
    }
}
