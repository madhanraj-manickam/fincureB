using FinCure.Models;

public class Expense
{
    public int Id { get; set; }

    public decimal Amount { get; set; }

    public string Category { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateTime Date { get; set; }

    public int UserId { get; set; }

    public Users? User { get; set; }
}