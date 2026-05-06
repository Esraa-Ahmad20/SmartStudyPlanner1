using Microsoft.AspNetCore.Mvc;
using SmartStudyPlanner1.Data;
using SmartStudyPlanner1.Models;

namespace SmartStudyPlanner1.Controllers
{
    public class ResourcesController : Controller
    {
        private readonly AppDbContext _db;
        public ResourcesController(AppDbContext db) { _db = db; }

        public IActionResult Index(int subjectId)
        {
            var resources = _db.Resources.Where(r => r.SubjectId == subjectId).ToList();
            ViewBag.SubjectId = subjectId;
            return View(resources);
        }

        [HttpPost]
        public IActionResult Create(Resource resource)
        {
            _db.Resources.Add(resource);
            _db.SaveChanges();
            return RedirectToAction("Index", new { subjectId = resource.SubjectId });
        }

        public IActionResult Delete(int id)
        {
            var resource = _db.Resources.Find(id);
            if (resource != null)
            {
                int subjectId = resource.SubjectId;
                _db.Resources.Remove(resource);
                _db.SaveChanges();
                return RedirectToAction("Index", new { subjectId });
            }
            return RedirectToAction("Index", "Subjects");
        }

        // GET: Edit
        public IActionResult Edit(int id)
        {
            var resource = _db.Resources.Find(id);
            if (resource == null)
                return RedirectToAction("Index", "Subjects");

            return View(resource);
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Resource resource)
        {
            if (ModelState.IsValid)
            {
                _db.Resources.Update(resource);
                _db.SaveChanges();
                return RedirectToAction("Index", new { subjectId = resource.SubjectId });
            }
            return View(resource);
        }
    }
}
