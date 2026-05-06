using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartStudyPlanner1.Data;

namespace SmartStudyPlanner1.Controllers
{
    [Route("Progress")]

    public class ProgressController : Controller
    {
        private readonly AppDbContext _db;
        public ProgressController(AppDbContext db) { _db = db; }

        [Route("")]
        [Route("Index")]
        public IActionResult Index()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");
            var subjects = _db.Subjects
                .Where(s => s.UserId == userId)
                .Include(s => s.Chapters!)
                    .ThenInclude(c => c.StudyTasks!)
                    .ThenInclude(t => t.ProgressRecord)
                .ToList();
            return View(subjects);
        }
    }
}