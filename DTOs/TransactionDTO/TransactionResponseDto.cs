namespace FinCure.DTOs.TransactionDTO
{
    public class TransactionResponseDto
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty; // Income or Expense
        public decimal Amount { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? AccountName { get; set; }
    }
}