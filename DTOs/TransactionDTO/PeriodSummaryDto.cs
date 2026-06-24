namespace FinCure.DTOs.TransactionDTO
{
    public class PeriodSummaryDto
    {
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal NetBalance { get; set; }

        // Groups the transactions (e.g., by exact Date, by Week, or by Month)
        public Dictionary<string, List<TransactionResponseDto>> GroupedTransactions { get; set; } = new();
    }
}