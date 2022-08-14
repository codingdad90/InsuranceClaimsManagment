using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MAKClaims.Models;

namespace Claims.Controllers
{
    public class ClaimsController : Controller
    {
        private Claimsconfig db = new Claimsconfig();

        // GET: Claims
        public ActionResult Index()
        {
            var claims = db.Claims.Include(c => c.Adjustor).Include(c => c.Property);
            return View(claims.ToList());
        }

        // GET: Claims/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Claim claim = db.Claims.Find(id);
            if (claim == null)
            {
                return HttpNotFound();
            }
            return View(claim);
        }

        // GET: Claims/Create
        public ActionResult Create()
        {
            ViewBag.AdjustorId = new SelectList(db.Adjustors, "AdjustorId", "Name");
            ViewBag.PropertyId = new SelectList(db.Properties, "PropertyId", "Address");
            return View();
        }

        // POST: Claims/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ClaimId,AdjustorId,PropertyId,DateOfLoss,Attachment,Reserves,Deductable,AmountPaid,Status")] Claim claim)
        {
            if (ModelState.IsValid)
            {               
                db.Claims.Add(claim);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AdjustorId = new SelectList(db.Adjustors, "AdjustorId", "Name", claim.AdjustorId);
            ViewBag.PropertyId = new SelectList(db.Properties, "PropertyId", "Address", claim.PropertyId);
            return View(claim);
        }

        // GET: Claims/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Claim claim = db.Claims.Find(id);
            if (claim == null)
            {
                return HttpNotFound();
            }
            ViewBag.AdjustorId = new SelectList(db.Adjustors, "AdjustorId", "Name", claim.AdjustorId);
            ViewBag.PropertyId = new SelectList(db.Properties, "PropertyId", "Address", claim.PropertyId);
            return View(claim);
        }

        // POST: Claims/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ClaimId,AdjustorId,PropertyId,DateOfLoss,Attachment,Reserves,Deductable,AmountPaid,Status")] Claim claim)
        {
            if (ModelState.IsValid)
            {
                db.Entry(claim).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AdjustorId = new SelectList(db.Adjustors, "AdjustorId", "Name", claim.AdjustorId);
            ViewBag.PropertyId = new SelectList(db.Properties, "PropertyId", "Address", claim.PropertyId);
            return View(claim);
        }

        // GET: Claims/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Claim claim = db.Claims.Find(id);
            if (claim == null)
            {
                return HttpNotFound();
            }
            return View(claim);
        }

        // POST: Claims/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Claim claim = db.Claims.Find(id);
            db.Claims.Remove(claim);
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
