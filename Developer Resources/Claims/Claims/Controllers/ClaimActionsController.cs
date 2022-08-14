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
    public class ClaimActionsController : Controller
    {
        private ClaimsEntities db = new ClaimsEntities();

        // GET: ClaimActions
        public ActionResult Index()
        {
            var claimActions = db.ClaimActions.Include(c => c.Action).Include(c => c.Adjustor).Include(c => c.Claim);
            return View(claimActions.ToList());
        }

        // GET: ClaimActions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClaimAction claimAction = db.ClaimActions.Find(id);
            if (claimAction == null)
            {
                return HttpNotFound();
            }
            return View(claimAction);
        }

        // GET: ClaimActions/Create
        public ActionResult Create()
        {
            ViewBag.ActionId = new SelectList(db.Actions, "ActionId", "ActionType");
            ViewBag.AdjustorId = new SelectList(db.Adjustors, "AdjustorId", "Name");
            ViewBag.ClaimId = new SelectList(db.Claims, "ClaimId", "ClaimId");
            return View();
        }

        // POST: ClaimActions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ClaimActionId,Date,DollarAmount,Note,ClaimId,ActionId,AdjustorId")] ClaimAction claimAction)
        {
            if (ModelState.IsValid)
            {
                db.ClaimActions.Add(claimAction);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ActionId = new SelectList(db.Actions, "ActionId", "ActionType", claimAction.ActionId);
            ViewBag.AdjustorId = new SelectList(db.Adjustors, "AdjustorId", "Name", claimAction.AdjustorId);
            ViewBag.ClaimId = new SelectList(db.Claims, "ClaimId", "ClaimId", claimAction.ClaimId);
            return View(claimAction);
        }

        // GET: ClaimActions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClaimAction claimAction = db.ClaimActions.Find(id);
            if (claimAction == null)
            {
                return HttpNotFound();
            }
            ViewBag.ActionId = new SelectList(db.Actions, "ActionId", "ActionType", claimAction.ActionId);
            ViewBag.AdjustorId = new SelectList(db.Adjustors, "AdjustorId", "Name", claimAction.AdjustorId);
            ViewBag.ClaimId = new SelectList(db.Claims, "ClaimId", "ClaimId", claimAction.ClaimId);
            return View(claimAction);
        }

        // POST: ClaimActions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ClaimActionId,Date,DollarAmount,Note,ClaimId,ActionId,AdjustorId")] ClaimAction claimAction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(claimAction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ActionId = new SelectList(db.Actions, "ActionId", "ActionType", claimAction.ActionId);
            ViewBag.AdjustorId = new SelectList(db.Adjustors, "AdjustorId", "Name", claimAction.AdjustorId);
            ViewBag.ClaimId = new SelectList(db.Claims, "ClaimId", "ClaimId", claimAction.ClaimId);
            return View(claimAction);
        }

        // GET: ClaimActions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClaimAction claimAction = db.ClaimActions.Find(id);
            if (claimAction == null)
            {
                return HttpNotFound();
            }
            return View(claimAction);
        }

        // POST: ClaimActions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ClaimAction claimAction = db.ClaimActions.Find(id);
            db.ClaimActions.Remove(claimAction);
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
