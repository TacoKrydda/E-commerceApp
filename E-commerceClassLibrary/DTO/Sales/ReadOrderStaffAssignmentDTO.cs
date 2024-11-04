namespace E_commerceClassLibrary.DTO.Sales
{
    public class ReadOrderStaffAssignmentDTO
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int StaffId { get; set; }
        public string StaffName { get; set; } = string.Empty;
    }
}
