using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartStudyPlanner1.Data;
using SmartStudyPlanner1.Models;

namespace SmartStudyPlanner1.Controllers
{
    [Route("Subjects")]
    public class SubjectsController : Controller
    {
        private readonly AppDbContext _db;
        public SubjectsController(AppDbContext db) { _db = db; }
        private int GetUserId() => HttpContext.Session.GetInt32("UserId") ?? 0;
        [Route("")]
        public IActionResult Index()
        {
            if (GetUserId() == 0) return RedirectToAction("Login", "Account");
            var subjects = _db.Subjects
                .Where(s => s.UserId == GetUserId())
                .Include(s => s.Chapters)
                .ToList();
            return View(subjects);
        }
        [Route("Create")]
        public IActionResult Create() => View();

        [HttpPost]
        [Route("Create")]
        public IActionResult Create(Subject subject)
        {
            if (GetUserId() == 0) return RedirectToAction("Login", "Account");
            subject.UserId = GetUserId();
            _db.Subjects.Add(subject);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        [Route("Edit/{id}")]
        public IActionResult Edit(int id)
        {
            var subject = _db.Subjects.Find(id);
            if (subject == null || subject.UserId != GetUserId()) return NotFound();
            return View(subject);
        }

        [HttpPost]
        [Route("Edit/{id}")]
        public IActionResult Edit(Subject subject)
        {
            subject.UserId = GetUserId();
            _db.Subjects.Update(subject);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        [Route("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var subject = _db.Subjects.Find(id);
            if (subject != null && subject.UserId == GetUserId())
            {
                _db.Subjects.Remove(subject);
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        [Route("Details/{id}")]
        public IActionResult Details(int id)
        {
            var subject = _db.Subjects
                .Include(s => s.Chapters)
                .Include(s => s.Resources)
                .FirstOrDefault(s => s.SubjectId == id && s.UserId == GetUserId());
            if (subject == null) return NotFound();
            return View(subject);
        }
    }
}