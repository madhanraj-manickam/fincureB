//using System.Security.Claims;
//using FinCure.Data;
//using ExpenseTracker.API.DTOs.Expense;
//using FinCure.Models;
//using FinCure.Data;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace ExpenseTracker.API.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    [Authorize]
//    public class ExpenseController : ControllerBase
//    {
//        private readonly FinCureDBContext _context;

//        public ExpenseController(FinCureDBContext context)
//        {
//            _context = context;
//        }


//        // CREATE EXPENSE
//        // POST: api/expense

//        [HttpPost]
//        public async Task<IActionResult> CreateExpense(ExpenseCreateDto dto)
//        {
//            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

//            if (userIdClaim == null)
//                return Unauthorized("User not found.");

//            int userId = int.Parse(userIdClaim);

//            var expense = new Expense
//            {
//                Amount = dto.Amount,
//                Category = dto.Category,
//                Date = dto.Date,
//                Description = dto.Description,
//                UserId = userId
//            };

//            _context.Expenses.Add(expense);
//            await _context.SaveChangesAsync();

//            var response = new ExpenseResponseDto
//            {
//                Id = expense.Id,
//                Amount = expense.Amount,
//                Category = expense.Category,
//                Date = expense.Date,
//                Description = expense.Description,
//                UserId = expense.UserId
//            };

//            return CreatedAtAction(
//                nameof(GetExpenseById),
//                new { id = expense.Id },
//                response);
//        }


//        // GET ALL EXPENSES FOR LOGGED-IN USER
//        // GET: api/expense

//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<ExpenseResponseDto>>> GetAllExpenses()
//        {
//            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

//            if (userIdClaim == null)
//                return Unauthorized("User not found.");

//            int userId = int.Parse(userIdClaim);

//            var expenses = await _context.Expenses
//                .Where(e => e.UserId == userId)
//                .OrderByDescending(e => e.Date)
//                .Select(e => new ExpenseResponseDto
//                {
//                    Id = e.Id,
//                    Amount = e.Amount,
//                    Category = e.Category,
//                    Date = e.Date,
//                    Description = e.Description,
//                    UserId = e.UserId
//                })
//                .ToListAsync();

//            return Ok(expenses);
//        }


//        // GET SINGLE EXPENSE BY ID
//        // GET: api/expense/{id}

//        [HttpGet("{id}")]
//        public async Task<IActionResult> GetExpenseById(int id)
//        {
//            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

//            if (userIdClaim == null)
//                return Unauthorized("User not found.");

//            int userId = int.Parse(userIdClaim);

//            var expense = await _context.Expenses
//                .Where(e => e.Id == id && e.UserId == userId)
//                .Select(e => new ExpenseResponseDto
//                {
//                    Id = e.Id,
//                    Amount = e.Amount,
//                    Category = e.Category,
//                    Date = e.Date,
//                    Description = e.Description,
//                    UserId = e.UserId
//                })
//                .FirstOrDefaultAsync();

//            if (expense == null)
//                return NotFound("Expense not found.");

//            return Ok(expense);
//        }


//        // UPDATE EXPENSE
//        // PUT: api/expense/{id}

//        [HttpPut("{id}")]
//        public async Task<IActionResult> UpdateExpense(int id, ExpenseUpdateDto dto)
//        {
//            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

//            if (userIdClaim == null)
//                return Unauthorized("User not found.");

//            int userId = int.Parse(userIdClaim);

//            var expense = await _context.Expenses
//                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);

//            if (expense == null)
//                return NotFound("Expense not found.");

//            expense.Amount = dto.Amount;
//            expense.Category = dto.Category;
//            expense.Date = dto.Date;
//            expense.Description = dto.Description;

//            await _context.SaveChangesAsync();

//            var response = new ExpenseResponseDto
//            {
//                Id = expense.Id,
//                Amount = expense.Amount,
//                Category = expense.Category,
//                Date = expense.Date,
//                Description = expense.Description,
//                UserId = expense.UserId
//            };

//            return Ok(response);
//        }


//        // DELETE EXPENSE
//        // DELETE: api/expense/{id}

//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteExpense(int id)
//        {
//            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

//            if (userIdClaim == null)
//                return Unauthorized("User not found.");

//            int userId = int.Parse(userIdClaim);

//            var expense = await _context.Expenses
//                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);

//            if (expense == null)
//                return NotFound("Expense not found.");

//            _context.Expenses.Remove(expense);
//            await _context.SaveChangesAsync();

//            return Ok(new
//            {
//                message = "Expense deleted successfully."
//            });
//        }
//    }
//}




//---------------------------------------------------------------------------------------------------------------


using ExpenseTracker.API.DTOs.Expense;
using FinCure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinCure.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ExpenseController : ControllerBase
    {
        private readonly ExpenseService _expenseService;

        public ExpenseController(ExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        [HttpPost]
        public async Task<IActionResult> AddExpense(ExpenseCreateDto dto)
        {
            int userId = GetUserId();
            var result = await _expenseService.AddExpenseAsync(userId, dto);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllExpenses()
        {
            int userId = GetUserId();
            var result = await _expenseService.GetAllAsync(userId);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetExpenseById(int id)
        {
            int userId = GetUserId();
            var result = await _expenseService.GetByIdAsync(id, userId);

            if (result == null)
                return NotFound("Expense not found.");

            return Ok(result);
        }

        [HttpGet("category/{category}")]
        public async Task<IActionResult> GetExpenseByCategory(string category)
        {
            int userId = GetUserId();
            var result = await _expenseService.GetByCategoryAsync(userId, category);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExpense(int id, ExpenseUpdateDto dto)
        {
            int userId = GetUserId();
            var result = await _expenseService.UpdateAsync(id, userId, dto);

            if (result == null)
                return NotFound("Expense not found.");

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            int userId = GetUserId();
            bool deleted = await _expenseService.DeleteAsync(id, userId);

            if (!deleted)
                return NotFound("Expense not found.");

            return Ok(new { message = "Expense deleted successfully." });
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null)
                throw new UnauthorizedAccessException("User not found.");

            return int.Parse(userIdClaim);
        }
    }
}