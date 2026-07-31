namespace LorryTransport.Domain.Entities
{
    public class Driver
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? LicenseNumber { get; set; }
        public ICollection<LoadEntry>? LoadEntries { get; set; }
        public ICollection<DriverPayment>? DriverPayments { get; set; }
    }
}
