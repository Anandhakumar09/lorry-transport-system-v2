namespace LorryTransport.Domain.Entities
{
    public class Expense
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string ExpenseType { get; set; } = string.Empty; // Diesel, Driver Salary, Repair, Tyre, Food, Toll Gate, Parking, Police, Other
        public decimal Amount { get; set; }
        public string? Remarks { get; set; }

        public int? LoadEntryId { get; set; }
        public LoadEntry? LoadEntry { get; set; }
    }
}
