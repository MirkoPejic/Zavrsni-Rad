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
    public class LecturerPersonsController : Controller
    {
        private EvidencijaDbEntities db = new EvidencijaDbEntities();

        // GET: LecturerPersons
        public ActionResult Index()
        {
            return View(db.LecturerPerson.ToList());
        }

        // GET: LecturerPersons/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LecturerPerson lecturerPerson = db.LecturerPerson.Find(id);
            if (lecturerPerson == null)
            {
                return HttpNotFound();
            }
            return View(lecturerPerson);
        }

        // GET: LecturerPersons/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LecturerPersons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LecturerID,Name,Surname,Title")] LecturerPerson lecturerPerson)
        {
            if (ModelState.IsValid)
            {
                db.LecturerPerson.Add(lecturerPerson);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(lecturerPerson);
        }

        // GET: LecturerPersons/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LecturerPerson lecturerPerson = db.LecturerPerson.Find(id);
            if (lecturerPerson == null)
            {
                return HttpNotFound();
            }
            return View(lecturerPerson);
        }

        // POST: LecturerPersons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LecturerID,Name,Surname,Title")] LecturerPerson lecturerPerson)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lecturerPerson).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(lecturerPerson);
        }

        // GET: LecturerPersons/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LecturerPerson lecturerPerson = db.LecturerPerson.Find(id);
            if (lecturerPerson == null)
            {
                return HttpNotFound();
            }
            return View(lecturerPerson);
        }

        // POST: LecturerPersons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LecturerPerson lecturerPerson = db.LecturerPerson.Find(id); //1
            var course = db.Course.Include(c => c.LecturerPerson);
            var lectures = db.Lectures.Include(l => l.Course).Include(l => l.LecturerPerson);
            var exams = db.Exams.Include(e => e.Course).Include(e => e.LecturerPerson);
            var colloquium = db.Colloquium.Include(c => c.Course).Include(c => c.LecturerPerson);
            foreach (var c in colloquium)
            {
                if (c.LecturerID == lecturerPerson.LecturerID)
                {
                    db.Colloquium.Remove(c);
                }
            }
            foreach (var e in exams)
            {
                if (e.LecturerID == lecturerPerson.LecturerID)
                {
                    db.Exams.Remove(e);
                }
            }
            foreach ( var l in lectures) 
            {
                if (l.LecturerID == lecturerPerson.LecturerID)
                {
                    db.Lectures.Remove(l);
                }
            }
            foreach (var k in course)
            {
                if (k.LecturerID == lecturerPerson.LecturerID)
                {
                    db.Course.Remove(k);
                }
            }
            db.LecturerPerson.Remove(lecturerPerson); //2
            db.SaveChanges(); //3
            return RedirectToAction("Index"); //4
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
