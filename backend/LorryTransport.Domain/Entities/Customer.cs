namespace LorryTransport.Domain.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public ICollection<LoadEntry>? LoadEntries { get; set; }
    }
}
