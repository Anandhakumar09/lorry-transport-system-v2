namespace LorryTransport.Domain.Entities
{
    public class DriverPayment
    {
        public int Id { get; set; }
        public int DriverId { get; set; }
        public Driver? Driver { get; set; }

        public DateTime Date { get; set; }
        public decimal AdvanceGiven { get; set; }
        public decimal Salary { get; set; }
        public decimal ExtraPaid { get; set; }
        public decimal TotalPaid { get; set; }
        public string? Remarks { get; set; }
    }
}
