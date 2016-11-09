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
    public class CoursesController : Controller
    {
        private EvidencijaDbEntities db = new EvidencijaDbEntities();

        // GET: Courses
        public ActionResult Index()
        {
            var course = db.Course.Include(c => c.LecturerPerson);
            return View(course.ToList());
        }

        // GET: Courses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Course.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // GET: Courses/Create
        public ActionResult Create()
        {
            //Samo ime predavača
            //ViewBag.LecturerID = new SelectList(db.LecturerPerson, "LecturerID", "Name");      

            //dohvaćanje imena i prezimena predavača
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


        // POST: Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CourseID,LecturerID,CourseName,CourseAbbreviation")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Course.Add(course);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LecturerID = new SelectList(db.LecturerPerson, "Name", "Surname", course.LecturerID);
            return View(course);
        }

        // GET: Courses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Course.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            //ViewBag.LecturerID = new SelectList(db.LecturerPerson, "LecturerID", "Name", course.LecturerID);
            //dohvaćanje imena i prezimena predavača
            ViewData["LecturerID"] =
            new SelectList((from s in db.LecturerPerson
                            select new
                            {
                                LecturerID = s.LecturerID,
                                FullName = s.Name + " " + s.Surname
                            }),
            "LecturerID", "FullName", course.LecturerID);
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CourseID,LecturerID,CourseName,CourseAbbreviation")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LecturerID = new SelectList(db.LecturerPerson, "LecturerID", "Name", course.LecturerID);
            return View(course);
        }

        // GET: Courses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Course.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = db.Course.Find(id);
            var lectures = db.Lectures.Include(l => l.Course).Include(l => l.LecturerPerson);
            var exams = db.Exams.Include(e => e.Course).Include(e => e.LecturerPerson);
            var colloquium = db.Colloquium.Include(c => c.Course).Include(c => c.LecturerPerson);
            foreach (var c in colloquium)
            {
                if (c.CourseID == course.CourseID)
                {
                    db.Colloquium.Remove(c);
                }
            }
            foreach (var e in exams)
            {
                if (e.CourseID == course.CourseID)
                {
                    db.Exams.Remove(e);
                }
            }
            foreach (var l in lectures)
            {
                if (l.CourseID == course.CourseID)
                {
                    db.Lectures.Remove(l);
                }
            }
            db.Course.Remove(course);
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
