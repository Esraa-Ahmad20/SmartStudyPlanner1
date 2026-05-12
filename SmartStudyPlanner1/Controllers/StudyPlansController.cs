using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartStudyPlanner1.Data;
using SmartStudyPlanner1.Models;

namespace SmartStudyPlanner1.Controllers
{
    [Route("StudyPlans")]
    public class StudyPlansController : Controller
    {
        private readonly AppDbContext _db;
        public StudyPlansController(AppDbContext db) { _db = db; }

        private int GetUserId() => HttpContext.Session.GetInt32("UserId") ?? 0;

        [Route("")]
        [Route("Index")]
        public IActionResult Index()
        {
            if (GetUserId() == 0)
                return RedirectToAction("Login", "Account");

            var plans = _db.StudyPlans
                .Where(p => p.UserId == GetUserId())
                .Include(p => p.StudyTasks)
                .ToList();

            return View(plans);
        }

        [Route("Create")]
        public IActionResult Create()
        {
            var subjects = _db.Subjects
                .Where(s => s.UserId == GetUserId())
                .ToList();

            ViewBag.Subjects = subjects;
            return View();
        }

        [HttpPost]
        [Route("Create")]
        public IActionResult Create(StudyPlan plan)
        {
            if (GetUserId() == 0)
                return RedirectToAction("Login", "Account");

            plan.UserId = GetUserId();

            _db.StudyPlans.Add(plan);
            _db.SaveChanges();

            GenerateTasks(plan.PlanId, plan.StartDate, plan.EndDate, plan.UserId);

            return RedirectToAction("Index");
        }

        private void GenerateTasks(int planId, DateTime? start, DateTime? end, int userId)
        {
            if (start == null || end == null) return;

            var subjects = _db.Subjects
                .Where(s => s.UserId == userId)
                .Include(s => s.Chapters)
                .ToList();

            var allChapters = subjects
                .SelectMany(s => s.Chapters ?? new List<Chapter>())
                .ToList();

            if (!allChapters.Any()) return;

            var totalDays = (end.Value - start.Value).Days;
            if (totalDays <= 0) return;

            int dayIndex = 0;

            foreach (var chapter in allChapters)
            {
                _db.StudyTasks.Add(new StudyTask
                {
                    PlanId = planId,
                    ChapterId = chapter.ChapterId,
                    ScheduledDate = start.Value.AddDays(dayIndex % totalDays),
                    IsCompleted = false,
                    Priority = 1
                });

                dayIndex++;
            }

            _db.SaveChanges();
        }

        [Route("Details/{id}")]
        public IActionResult Details(int id)
        {
            var plan = _db.StudyPlans
                .Include(p => p.StudyTasks!)
                    .ThenInclude(t => t.Chapter!)
                    .ThenInclude(c => c.Subject)
                .FirstOrDefault(p => p.PlanId == id && p.UserId == GetUserId());

            if (plan == null) return NotFound();

            return View(plan);
        }

        [Route("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var plan = _db.StudyPlans.Find(id);

            if (plan != null && plan.UserId == GetUserId())
            {
                _db.StudyPlans.Remove(plan);
                _db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [Route("Edit/{id}")]
        public IActionResult Edit(int id)
        {
            if (GetUserId() == 0)
                return RedirectToAction("Login", "Account");

            var plan = _db.StudyPlans.FirstOrDefault(p => p.PlanId == id && p.UserId == GetUserId());
            if (plan == null) return NotFound();

            return View(plan);
        }

        [HttpPost]
        [Route("Edit/{id}")]
        public IActionResult Edit(int id, StudyPlan updated)
        {
            if (GetUserId() == 0)
                return RedirectToAction("Login", "Account");

            var plan = _db.StudyPlans.FirstOrDefault(p => p.PlanId == id && p.UserId == GetUserId());
            if (plan == null) return NotFound();

            plan.PlanName = updated.PlanName;
            plan.StartDate = updated.StartDate;
            plan.EndDate = updated.EndDate;
            plan.DailyStudyHours = updated.DailyStudyHours;

            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("UpdatePriority")]
        public IActionResult UpdatePriority([FromBody] UpdatePriorityRequest request)
        {
            if (GetUserId() == 0)
                return Json(new { success = false });

            var task = _db.StudyTasks
                .Include(t => t.StudyPlan)
                .FirstOrDefault(t => t.TaskId == request.TaskId && t.StudyPlan!.UserId == GetUserId());

            if (task == null)
                return Json(new { success = false });

            task.Priority = request.Priority;
            _db.SaveChanges();

            return Json(new { success = true });
        }

        public class UpdatePriorityRequest
        {
            public int TaskId { get; set; }
            public int Priority { get; set; }
        }
    }
}
