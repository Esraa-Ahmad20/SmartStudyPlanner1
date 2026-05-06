using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartStudyPlanner1.Data;

namespace SmartStudyPlanner1.Controllers
{
    [Route("Dashboard")]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _db;
        public DashboardController(AppDbContext db) { _db = db; }

        [Route("")]
        public IActionResult Index()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            var subjects = _db.Subjects
                .Where(s => s.UserId == userId)
                .Include(s => s.Chapters)
                .ToList();

            var todayTasks = _db.StudyTasks
                .Include(t => t.Chapter).ThenInclude(c => c!.Subject)
                .Include(t => t.StudyPlan)
                .Where(t => t.StudyPlan!.UserId == userId && t.ScheduledDate.Date == DateTime.Today)
                .ToList();

            var quote = _db.MotivationQuotes.OrderBy(q => Guid.NewGuid()).FirstOrDefault();

            ViewBag.Subjects = subjects;
            ViewBag.TodayTasks = todayTasks;
            ViewBag.Quote = quote;
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            return View();
        }
        
    }
}