using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using deneme135.Models;
using deneme135.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace deneme135.Controllers
{
    public class TestController : Controller
    {
        private readonly AppDbContext _context;

        public TestController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var tests = await _context.Tests.OrderByDescending(t => t.CreatedDate).ToListAsync();
            return View(tests);
        }

        [Authorize(Roles = "Student")]
        public async Task<IActionResult> StudentTests()
        {
            var tests = await _context.Tests
                .Where(t => t.IsActive)
                .OrderByDescending(t => t.CreatedDate)
                .ToListAsync();
            return View(tests);
        }

        [Authorize(Roles = "Student")]
        public async Task<IActionResult> TakeTest(int id)
        {
            var test = await _context.Tests.FindAsync(id);
            if (test == null || !test.IsActive)
                return NotFound();

            return View(test);
        }

        [HttpGet]
        public async Task<IActionResult> GetTest(int id)
        {
            var test = await _context.Tests.FindAsync(id);
            if (test == null)
                return Json(new { success = false, message = "Test bulunamadı." });

            return Json(new { 
                success = true, 
                id = test.Id,
                title = test.Title,
                description = test.Description,
                content = test.Content,
                isActive = test.IsActive
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SaveTest([FromBody] Test test)
        {
            try
            {
                if (test.Id == 0)
                {
                    await _context.Tests.AddAsync(test);
                }
                else
                {
                    var existingTest = await _context.Tests.FindAsync(test.Id);
                    if (existingTest == null)
                        return Json(new { success = false, message = "Test bulunamadı." });

                    existingTest.Title = test.Title;
                    existingTest.Description = test.Description;
                    existingTest.Content = test.Content;
                    existingTest.IsActive = test.IsActive;

                    _context.Tests.Update(existingTest);
                }

                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var test = await _context.Tests.FindAsync(id);
                if (test == null)
                    return Json(new { success = false, message = "Test bulunamadı." });

                _context.Tests.Remove(test);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
} 