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
    public class ColloquiumsController : Controller
    {
        private EvidencijaDbEntities db = new EvidencijaDbEntities();

        // GET: Colloquiums
        public ActionResult Index()
        {
            var colloquium = db.Colloquium.Include(c => c.Course).Include(c => c.LecturerPerson);
            return View(colloquium.ToList().OrderBy(o => o.ColloquiumDate));
        }

        // GET: Colloquiums/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Colloquium colloquium = db.Colloquium.Find(id);
            if (colloquium == null)
            {
                return HttpNotFound();
            }
            return View(colloquium);
        }

        // GET: Colloquiums/Create
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

        // POST: Colloquiums/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ColloquiumID,CourseID,LecturerID,ColloquiumDate,ColloquiumStart,ColloquiumText")] Colloquium colloquium)
        {
            if (ModelState.IsValid)
            {
                db.Colloquium.Add(colloquium);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CourseID = new SelectList(db.Course, "CourseID", "CourseName", colloquium.CourseID);
            ViewBag.LecturerID = new SelectList(db.LecturerPerson, "LecturerID", "Name", colloquium.LecturerID);
            return View(colloquium);
        }

        // GET: Colloquiums/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Colloquium colloquium = db.Colloquium.Find(id);
            if (colloquium == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseID = new SelectList(db.Course, "CourseID", "CourseName", colloquium.CourseID);
            //ViewBag.LecturerID = new SelectList(db.LecturerPerson, "LecturerID", "Name", colloquium.LecturerID);

            ViewData["LecturerID"] =
            new SelectList((from s in db.LecturerPerson
                            select new
                            {
                                LecturerID = s.LecturerID,
                                FullName = s.Name + " " + s.Surname
                            }),
            "LecturerID", "FullName", colloquium.LecturerID);
            return View(colloquium);
        }

        // POST: Colloquiums/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ColloquiumID,CourseID,LecturerID,ColloquiumDate,ColloquiumStart,ColloquiumText")] Colloquium colloquium)
        {
            if (ModelState.IsValid)
            {
                db.Entry(colloquium).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CourseID = new SelectList(db.Course, "CourseID", "CourseName", colloquium.CourseID);
            ViewBag.LecturerID = new SelectList(db.LecturerPerson, "LecturerID", "Name", colloquium.LecturerID);
            return View(colloquium);
        }

        // GET: Colloquiums/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Colloquium colloquium = db.Colloquium.Find(id);
            if (colloquium == null)
            {
                return HttpNotFound();
            }
            return View(colloquium);
        }

        // POST: Colloquiums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Colloquium colloquium = db.Colloquium.Find(id);
            db.Colloquium.Remove(colloquium);
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
