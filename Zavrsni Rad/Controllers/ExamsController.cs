using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Zavrsni_Rad.Models;

namespace Zavrsni_Rad.Controllers
{
    public class ExamsController : Controller
    {
        private EvidencijaDbEntities db = new EvidencijaDbEntities();

        // GET: Exams
        public ActionResult Index()
        {
            var exams = db.Exams.Include(e => e.Course).Include(e => e.LecturerPerson);
            return View(exams.ToList().OrderBy(o => o.ExamDate));
        }

        // GET: Exams/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exams exams = db.Exams.Find(id);
            if (exams == null)
            {
                return HttpNotFound();
            }
            return View(exams);
        }

        // GET: Exams/Create
        public ActionResult Create()
        {
            ViewBag.CourseID = new SelectList(db.Course, "CourseID", "CourseName");
            //ViewBag.LecturerID = new SelectList(db.LecturerPerson, "LecturerID", "Name");
            ViewData["LecturerID"] =
                new SelectList((from s in db.LecturerPerson
                select new
                {
                    LecturerID = s.LecturerID,
                    FullName = s.Name + " " + s.Surname
                }),
            "LecturerID", "FullName", null);
            return View();
        }

        // POST: Exams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ExamsID,CourseID,LecturerID,ExamDate,ExamStart,ExamText")] Exams exams)
        {
            if (ModelState.IsValid)
            {
                db.Exams.Add(exams);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CourseID = new SelectList(db.Course, "CourseID", "CourseName", exams.CourseID);
            ViewBag.LecturerID = new SelectList(db.LecturerPerson, "LecturerID", "Name", exams.LecturerID);
            return View(exams);
        }

        // GET: Exams/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exams exams = db.Exams.Find(id);
            if (exams == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseID = new SelectList(db.Course, "CourseID", "CourseName", exams.CourseID);
            //ViewBag.LecturerID = new SelectList(db.LecturerPerson, "LecturerID", "Name", exams.LecturerID);
            ViewData["LecturerID"] =
                new SelectList((from s in db.LecturerPerson
                                select new
                                {
                                    LecturerID = s.LecturerID,
                                    FullName = s.Name + " " + s.Surname
                                }),
            "LecturerID", "FullName", exams.LecturerID);
            return View(exams);
        }

        // POST: Exams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ExamsID,CourseID,LecturerID,ExamDate,ExamStart,ExamText")] Exams exams)
        {
            if (ModelState.IsValid)
            {
                db.Entry(exams).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CourseID = new SelectList(db.Course, "CourseID", "CourseName", exams.CourseID);
            ViewBag.LecturerID = new SelectList(db.LecturerPerson, "LecturerID", "Name", exams.LecturerID);
            return View(exams);
        }

        // GET: Exams/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exams exams = db.Exams.Find(id);
            if (exams == null)
            {
                return HttpNotFound();
            }
            return View(exams);
        }

        // POST: Exams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Exams exams = db.Exams.Find(id);
            db.Exams.Remove(exams);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
