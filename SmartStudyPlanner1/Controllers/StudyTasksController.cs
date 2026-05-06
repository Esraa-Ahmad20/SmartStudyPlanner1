using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartStudyPlanner1.Data;
using SmartStudyPlanner1.Models;

namespace SmartStudyPlanner1.Controllers
{
    public class StudyTasksController : Controller
    {
        private readonly AppDbContext _db;
        public StudyTasksController(AppDbContext db) { _db = db; }

        [HttpPost]
        public IActionResult Complete(int taskId, decimal timeSpent, string? notes)
        {
            var task = _db.StudyTasks.Find(taskId);
            if (task != null)
            {
                task.IsCompleted = true;
                _db.StudyTasks.Update(task);
                var existing = _db.ProgressRecords.FirstOrDefault(p => p.TaskId == taskId);
                if (existing == null)
                {
                    _db.ProgressRecords.Add(new ProgressRecord
                    {
                        TaskId = taskId,
                        ActualCompletionDate = DateTime.Now,
                        TimeSpent = timeSpent,
                        Notes = notes
                    });
                }
                _db.SaveChanges();
            }
            return RedirectToAction("TodayTasks");
        }

        public IActionResult TodayTasks()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");
            var tasks = _db.StudyTasks
                .Include(t => t.Chapter!).ThenInclude(c => c.Subject)
                .Include(t => t.StudyPlan)
                .Include(t => t.ProgressRecord)
                .Where(t => t.StudyPlan!.UserId == userId && t.ScheduledDate.Date == DateTime.Today)
                .OrderBy(t => t.Priority)
                .ToList();
            return View(tasks);
        }
    }
}