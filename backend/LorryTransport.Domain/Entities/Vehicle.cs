namespace LorryTransport.Domain.Entities
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string VehicleNumber { get; set; } = string.Empty;
        public string? VehicleType { get; set; }
        public ICollection<LoadEntry>? LoadEntries { get; set; }
    }
}
