using System.Text.Json.Serialization;
using WebbShopClassLibrary.Models.Production;

namespace WebbShopClassLibrary.Models.Sales
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
