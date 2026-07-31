namespace LorryTransport.Application.DTOs
{
    public class LoadEntryDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string FromLocation { get; set; } = string.Empty;
        public string ToLocation { get; set; } = string.Empty;
        public int CustomerId { get; set; }
        public string MaterialName { get; set; } = string.Empty;
        public int VehicleId { get; set; }
        public int DriverId { get; set; }
        public decimal AdvanceAmount { get; set; }
        public decimal LoadingCharge { get; set; }
        public decimal RatePerTon { get; set; }
        public decimal TotalTons { get; set; }
        public decimal FreightAmount { get; set; }
        public decimal DieselAmount { get; set; }
        public decimal DriverSalary { get; set; }
        public decimal CleanerSalary { get; set; }
        public decimal Commission { get; set; }
        public decimal OtherExpenses { get; set; }
        public decimal ExpenseTotal { get; set; }
        public decimal Profit { get; set; }
        public decimal DriverBalance { get; set; }
        public string? Notes { get; set; }
    }

    public class CreateLoadEntryDto
    {
        public DateTime Date { get; set; }
        public string FromLocation { get; set; } = string.Empty;
        public string ToLocation { get; set; } = string.Empty;
        public int CustomerId { get; set; }
        public string MaterialName { get; set; } = string.Empty;
        public int VehicleId { get; set; }
        public int DriverId { get; set; }
        public decimal AdvanceAmount { get; set; }
        public decimal LoadingCharge { get; set; }
        public decimal RatePerTon { get; set; }
        public decimal TotalTons { get; set; }
        public decimal DieselAmount { get; set; }
        public decimal DriverSalary { get; set; }
        public decimal CleanerSalary { get; set; }
        public decimal Commission { get; set; }
        public decimal OtherExpenses { get; set; }
        public string? Notes { get; set; }
    }
}
