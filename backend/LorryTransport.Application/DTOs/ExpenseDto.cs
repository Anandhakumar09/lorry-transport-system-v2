namespace LorryTransport.Application.DTOs
{
    public class ExpenseDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string ExpenseType { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string? Remarks { get; set; }
    }

    public class CreateExpenseDto
    {
        public DateTime Date { get; set; }
        public string ExpenseType { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string? Remarks { get; set; }
    }
}
