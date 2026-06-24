namespace FinCure.DTOs.TransactionDTO
{
    public class CalendarDayDto
    {
        public string Date { get; set; } = string.Empty; // Format: YYYY-MM-DD
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal NetBalance { get; set; }
        public bool HasActivity => TotalIncome > 0 || TotalExpense > 0;
    }
}