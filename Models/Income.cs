using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FinCure.Models
{
    public class Income
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string? Category { get; set; }
        public DateTime Date { get; set; }
        public int MyProperty { get; set; }
        public int UserId { get; set; }  //Because:  One user → many incomes   This creates relationship.Without UserId  income not linked to user.Very important database design concept.

        public Users? User { get; set; }
    }
}
