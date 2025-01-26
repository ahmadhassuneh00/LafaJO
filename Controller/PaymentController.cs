using FinalProjAPI.Data;
using FinalProjAPI.Dto;
using FinalProjAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProjAPI.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly DataContextDapper _dapper;
        private readonly DataContextEF _entityFramWork;

        public PaymentController(DbContextOptions<DataContextEF> options, IPaymentRepository paymentRepository, IConfiguration configuration)
        {
            _paymentRepository = paymentRepository;
            _dapper = new DataContextDapper(configuration);
            _entityFramWork = new DataContextEF(options, configuration);
        }
        [HttpGet("GetPaymentCard")]
        public IActionResult GetPaymentCard()
        {
            var PaymentCards = _paymentRepository.GetPaymentCards();
            if (PaymentCards == null || !PaymentCards.Any())
            {
                return NotFound(new { message = "No cards found" });
            }
            return Ok(PaymentCards);
        }
        [HttpPost("addPaymentCard")]
        public async Task<IActionResult> AddPaymentCard([FromForm] PaymentDto payment)
        {
            // Check if the model is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid input", errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }

            // Continue with the logic if model is valid
            var cardEntity = new Payments
            {
                CardNumber = payment.CardNumber,
                expirationDate = payment.expirationDate,
                cvv = payment.cvv,
                cardHolder = payment.cardHolder
            };

            await _paymentRepository.AddPaymentCardAsync(cardEntity);

            if (_paymentRepository.SaveChanges())
            {
                return Ok(new { message = "Card added successfully" });
            }

            return StatusCode(500, new { message = "Failed to add payment card. Please try again." });
        }


    }
}