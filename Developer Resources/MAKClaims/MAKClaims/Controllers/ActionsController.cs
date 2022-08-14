using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MAKClaims.Models;

namespace MAKClaims.Controllers
{
    public class ActionsController : Controller
    {
        private Claimsconfig db = new Claimsconfig();

        // GET: Actions
        public ActionResult Index()
        {
            return View(db.Actions.ToList());
        }

        // GET: Actions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MAKClaims.Models.Action action = db.Actions.Find(id);
            if (action == null)
            {
                return HttpNotFound();
            }
            return View(action);
        }

        // GET: Actions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Actions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ActionId,ActionType")] Models.Action action, FormCollection fc )
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            var actionType = fc["ActionType"];
            action = new Models.Action() { ActionType = actionType };
            
            
                db.Actions.Add(action);
                db.SaveChanges();
                return RedirectToAction("Index");
            

            //return View(action);
        }

        // GET: Actions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MAKClaims.Models.Action action = db.Actions.Find(id);
            if (action == null)
            {
                return HttpNotFound();
            }
            return View(action);
        }

        // POST: Actions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ActionId,ActionType")] MAKClaims.Models.Action action)
        {
            if (ModelState.IsValid)
            {
                db.Entry(action).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(action);
        }

        // GET: Actions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MAKClaims.Models.Action action = db.Actions.Find(id);
            if (action == null)
            {
                return HttpNotFound();
            }
            return View(action);
        }

        // POST: Actions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MAKClaims.Models.Action action = db.Actions.Find(id);
            db.Actions.Remove(action);
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
