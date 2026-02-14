using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentApi.Data;
using PaymentApi.Models;
using PaymentApi.Utilities;
using System.IdentityModel.Tokens.Jwt;
using dto.payment;

namespace PaymentApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PaymentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        private int? GetCurrentUserId()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : null;
        }

        // GET: api/Payments
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<Payment>>>> GetPayments()
        {
            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized(ApiResponse<IEnumerable<Payment>>.ErrorResponse("Unauthorized"));

            var payments = await _context.Payments
                .Where(p => p.UserId == userId)
                .ToListAsync();

            return Ok(ApiResponse<IEnumerable<Payment>>.SuccessResponse(payments, "Payments retrieved successfully"));
        }

        // GET: api/Payments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Payment>>> GetPayment(int id)
        {
            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized(ApiResponse<Payment>.ErrorResponse("Unauthorized"));

            var payment = await _context.Payments
                .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);

            if (payment == null)
            {
                return NotFound(ApiResponse<Payment>.ErrorResponse("Payment not found or access denied"));
            }

            return Ok(ApiResponse<Payment>.SuccessResponse(payment, "Payment retrieved successfully"));
        }

        // POST: api/Payments
        [HttpPost]
        public async Task<ActionResult<ApiResponse<Payment>>> PostPayment(CreatePaymentDto paymentDto)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized(ApiResponse<Payment>.ErrorResponse("Unauthorized access"));
            }

            var payment = new Payment
            {
                TransactionId = paymentDto.TransactionId,
                Amount = paymentDto.Amount,
                Status = paymentDto.Status,
                PaymentDate = DateTime.UtcNow,
                UserId = userId.Value
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPayment", new { id = payment.Id }, ApiResponse<Payment>.SuccessResponse(payment, "Payment created successfully"));
        }
    }
}
