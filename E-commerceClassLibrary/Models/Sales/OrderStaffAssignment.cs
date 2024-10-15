using System.Text.Json.Serialization;

namespace WebbShopClassLibrary.Models.Sales
{
    public class OrderStaffAssignment
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int StaffId { get; set; }
        [JsonIgnore]
        public Order? Order { get; set; }
        [JsonIgnore]
        public Staff? Staff { get; set; }
    }
}
