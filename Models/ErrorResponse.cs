namespace FinCure.Models
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }

        public string? Message { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}