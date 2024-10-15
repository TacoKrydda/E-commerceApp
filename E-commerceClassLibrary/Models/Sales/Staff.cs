using System.Text.Json.Serialization;

namespace WebbShopClassLibrary.Models.Sales
{
    public class Staff
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

        [JsonIgnore]
        public List<OrderStaffAssignment>? OrderStaffAssignments { get; set; } = new List<OrderStaffAssignment>();
    }
}
