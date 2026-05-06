using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartStudyPlanner1.Data;
using SmartStudyPlanner1.Models;

namespace SmartStudyPlanner1.Controllers
{
    public class ChaptersController : Controller
    {
        private readonly AppDbContext _db;
        public ChaptersController(AppDbContext db) { _db = db; }

        public IActionResult Create(int subjectId)
        {
            ViewBag.SubjectId = subjectId;
            return View();
        }

        [HttpPost]
        public IActionResult Create(Chapter chapter)
        {
            _db.Chapters.Add(chapter);
            _db.SaveChanges();
            return RedirectToAction("Details", "Subjects", new { id = chapter.SubjectId });
        }

        public IActionResult Delete(int id)
        {
            var chapter = _db.Chapters.Find(id);
            if (chapter != null)
            {
                int subjectId = chapter.SubjectId;
                _db.Chapters.Remove(chapter);
                _db.SaveChanges();
                return RedirectToAction("Details", "Subjects", new { id = subjectId });
            }
            return RedirectToAction("Index", "Subjects");
        }

        // ✅ GET: Edit - بيجيب بيانات الـ Chapter ويفتح صفحة التعديل
        public IActionResult Edit(int id)
        {
            var chapter = _db.Chapters.Find(id);
            if (chapter == null)
                return RedirectToAction("Index", "Subjects");

            return View(chapter);
        }

        // ✅ POST: Edit - بيحفظ التعديلات
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Chapter chapter)
        {
            if (ModelState.IsValid)
            {
                _db.Chapters.Update(chapter);
                _db.SaveChanges();

                return RedirectToAction("Details", "Subjects", new { id = chapter.SubjectId });
            }

            return View(chapter);
        }
    }
}