namespace LorryTransport.Application.DTOs
{
    public class DashboardDto
    {
        public int TodaysTrips { get; set; }
        public int ThisMonthTrips { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal TotalProfit { get; set; }
        public decimal TotalDieselExpense { get; set; }
        public decimal TotalDriverSalary { get; set; }
        public decimal TotalCleanerSalary { get; set; }
        public decimal TotalCommission { get; set; }
        public decimal PendingDriverBalance { get; set; }
    }
}
