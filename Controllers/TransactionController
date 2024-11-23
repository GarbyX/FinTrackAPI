using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FinTrackAPI.Data;
using FinTrackAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FinTrackAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Ensure only authenticated users access these endpoints
    public class TransactionController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TransactionController(AppDbContext context)
        {
            _context = context;
        }

        // Get all transactions for the logged-in user
        [HttpGet]
        public async Task<IActionResult> GetTransactions()
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value); // Assuming JWT includes UserId
            var transactions = await _context.Transactions
                .Where(t => t.UserId == userId)
                .ToListAsync();
            return Ok(transactions);
        }

        // Create a new transaction
        [HttpPost]
        public async Task<IActionResult> CreateTransaction([FromBody] Transaction transaction)
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value);
            transaction.UserId = userId;
            transaction.Date = DateTime.UtcNow;

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTransactions), new { id = transaction.Id }, transaction);
        }

        // Get a single transaction by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransaction(int id)
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value);
            var transaction = await _context.Transactions
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (transaction == null) return NotFound();
            return Ok(transaction);
        }
    }
}
