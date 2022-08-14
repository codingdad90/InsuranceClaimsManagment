using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Claims.Models;

namespace Claims.Controllers
{
    public class AdjustorsController : Controller
    {
        private ClaimsEntities db = new ClaimsEntities();

        // GET: Adjustors
        public ActionResult Index()
        {
            var adjustors = db.Adjustors.Include(a => a.Manager);
            return View(adjustors.ToList());
        }

        // GET: Adjustors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Adjustor adjustor = db.Adjustors.Find(id);
            if (adjustor == null)
            {
                return HttpNotFound();
            }
            return View(adjustor);
        }

        // GET: Adjustors/Create
        public ActionResult Create()
        {
            ViewBag.ManagerId = new SelectList(db.Managers, "ManagerId", "Name");
            return View();
        }

        // POST: Adjustors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AdjustorId,Name,PhoneNumber,Email,IsManager,Password,ManagerId")] Adjustor adjustor)
        {
            if (ModelState.IsValid)
            {
                db.Adjustors.Add(adjustor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ManagerId = new SelectList(db.Managers, "ManagerId", "Name", adjustor.ManagerId);
            return View(adjustor);
        }

        // GET: Adjustors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Adjustor adjustor = db.Adjustors.Find(id);
            if (adjustor == null)
            {
                return HttpNotFound();
            }
            ViewBag.ManagerId = new SelectList(db.Managers, "ManagerId", "Name", adjustor.ManagerId);
            return View(adjustor);
        }

        // POST: Adjustors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AdjustorId,Name,PhoneNumber,Email,IsManager,Password,ManagerId")] Adjustor adjustor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(adjustor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ManagerId = new SelectList(db.Managers, "ManagerId", "Name", adjustor.ManagerId);
            return View(adjustor);
        }

        // GET: Adjustors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Adjustor adjustor = db.Adjustors.Find(id);
            if (adjustor == null)
            {
                return HttpNotFound();
            }
            return View(adjustor);
        }

        // POST: Adjustors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Adjustor adjustor = db.Adjustors.Find(id);
            db.Adjustors.Remove(adjustor);
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
