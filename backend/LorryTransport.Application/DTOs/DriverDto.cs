namespace LorryTransport.Application.DTOs
{
    public class DriverDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? LicenseNumber { get; set; }
    }

    public class DriverLedgerDto
    {
        public int DriverId { get; set; }
        public string DriverName { get; set; } = string.Empty;
        public decimal TotalAdvanceGiven { get; set; }
        public decimal TotalSalary { get; set; }
        public decimal TotalExtraPaid { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal RemainingBalance { get; set; }
    }
}
