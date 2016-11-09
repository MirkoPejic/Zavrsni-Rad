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
    public class LecturesController : Controller
    {
        private EvidencijaDbEntities db = new EvidencijaDbEntities();

        // GET: Lectures
        public ActionResult Index()
        {
            var lectures = db.Lectures.Include(l => l.Course).Include(l => l.LecturerPerson);
            return View(lectures.ToList().OrderBy(o => o.LectureDate));
        }

        // GET: Lectures/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lectures lectures = db.Lectures.Find(id);
            if (lectures == null)
            {
                return HttpNotFound();
            }
            return View(lectures);
        }

        // GET: Lectures/Create
        public ActionResult Create()
        {
            ViewBag.CourseID = new SelectList(db.Course, "CourseID", "CourseName");
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

        // POST: Lectures/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LecturesID,LecturerID,CourseID,LectureDate,LectureStart,LectureEnd,LectureText")] Lectures lectures)
        {
            if (ModelState.IsValid)
            {
                db.Lectures.Add(lectures);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CourseID = new SelectList(db.Course, "CourseID", "CourseName", lectures.CourseID);
            ViewBag.LecturerID = new SelectList(db.LecturerPerson, "LecturerID", "Name", lectures.LecturerID);
            return View(lectures);
        }

        // GET: Lectures/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lectures lectures = db.Lectures.Find(id);
            if (lectures == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseID = new SelectList(db.Course, "CourseID", "CourseName", lectures.CourseID);
            //ViewBag.LecturerID = new SelectList(db.LecturerPerson, "LecturerID", "Name", lectures.LecturerID);
            //dohvaćanje imena i prezimena predavača
            ViewData["LecturerID"] =
            new SelectList((from s in db.LecturerPerson
                            select new
                            {
                                LecturerID = s.LecturerID,
                                FullName = s.Name + " " + s.Surname
                            }),
            "LecturerID", "FullName", lectures.CourseID);
            return View(lectures);
        }

        // POST: Lectures/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LecturesID,LecturerID,CourseID,LectureDate,LectureStart,LectureEnd,LectureText")] Lectures lectures)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lectures).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CourseID = new SelectList(db.Course, "CourseID", "CourseName", lectures.CourseID);
            ViewBag.LecturerID = new SelectList(db.LecturerPerson, "LecturerID", "Name", lectures.LecturerID);
            return View(lectures);
        }

        // GET: Lectures/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lectures lectures = db.Lectures.Find(id);
            if (lectures == null)
            {
                return HttpNotFound();
            }
            return View(lectures);
        }

        // POST: Lectures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Lectures lectures = db.Lectures.Find(id);

            db.Lectures.Remove(lectures);
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
