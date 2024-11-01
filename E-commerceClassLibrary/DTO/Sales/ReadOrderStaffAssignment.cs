namespace E_commerceClassLibrary.DTO.Sales
{
    public class ReadOrderStaffAssignment
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int StaffId { get; set; }
        public string StaffName { get; set; } = string.Empty;
    }
}
