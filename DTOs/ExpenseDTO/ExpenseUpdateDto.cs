namespace ExpenseTracker.API.DTOs.Expense
{
    public class ExpenseUpdateDto
    {
        public decimal Amount { get; set; }
        public string Category { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string? Description { get; set; }
    }
}