using System.ComponentModel.DataAnnotations;

namespace dto.payment
{
    public class CreatePaymentDto
    {
        public string? TransactionId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }

        public string? Status { get; set; }
    }
}
