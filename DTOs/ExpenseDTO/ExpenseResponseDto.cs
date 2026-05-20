namespace ExpenseTracker.API.DTOs.Expense
{
    public class ExpenseResponseDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string Category { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string? Description { get; set; }
        public int UserId { get; set; }
    }
}