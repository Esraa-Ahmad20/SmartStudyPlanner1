using Microsoft.AspNetCore.Mvc;
using SmartStudyPlanner1.Data;
using SmartStudyPlanner1.Models;

namespace SmartStudyPlanner1.Controllers
{
    [Route("Pomodoro")]

    public class PomodoroController : Controller
    {
        private readonly AppDbContext _db;
        public PomodoroController(AppDbContext db) { _db = db; }

        [Route("")]
        [Route("Index")]
        public IActionResult Index()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");
            var subjects = _db.Subjects.Where(s => s.UserId == userId).ToList();
            ViewBag.Subjects = subjects;
            return View();
        }

        [HttpPost]
        [Route("Save")]

        public IActionResult Save([FromBody] PomodoroSession session)
        {
            session.StartTime = DateTime.Now;
            _db.PomodoroSessions.Add(session);
            _db.SaveChanges();
            return Json(new { success = true });
        }
    }
}