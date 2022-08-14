using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MAKClaims.Models;
using Microsoft.AspNet.Identity;

namespace Claims.Controllers
{
    public class InsuredsController : Controller
    {
        private Claimsconfig db = new Claimsconfig();

        [Authorize(Roles ="Default")]
        public ActionResult Welcome() //splash page for when customers login 
        {
            var uid = User.Identity.GetUserId();
            Insured currentInsured = db.Insureds.Where(i => i.LoginID.Equals(uid, StringComparison.Ordinal)).FirstOrDefault();
            Adjustor tryAdjustor = db.Adjustors.Where(a => a.LoginID.Equals(uid, StringComparison.Ordinal)).FirstOrDefault();
            
            if(currentInsured != null)
            {
                ViewBag.Welcome = "Hello " + currentInsured.Name_First;
            }
            else if (tryAdjustor != null)
            {
                ViewBag.Welcome = "Hello " + tryAdjustor.Name;
            }
            else
            {
                var notInDb = db.AspNetUsers.Where(n => n.Id.Equals(uid, StringComparison.Ordinal)).FirstOrDefault();
                ViewBag.Welcome = "Hello " + notInDb.UserName;
            }
             

            return View();
        }

        // GET: Insureds
        public ActionResult Index()
        {
            return View(db.Insureds.ToList());
        }

        // GET: Insureds/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insured insured = db.Insureds.Find(id);
            if (insured == null)
            {
                return HttpNotFound();
            }
            return View(insured);
        }

        // GET: Insureds/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Insureds/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InsuredId,MailingAddress,State,Zip,PhoneNumber,Email,Name_First,Name_Last,City,Username,Password")] Insured insured)
        {
            if (ModelState.IsValid)
            {
                db.Insureds.Add(insured);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(insured);
        }

        // GET: Insureds/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insured insured = db.Insureds.Find(id);
            if (insured == null)
            {
                return HttpNotFound();
            }
            return View(insured);
        }

        // POST: Insureds/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InsuredId,MailingAddress,State,Zip,PhoneNumber,Email,Name_First,Name_Last,City,Username,Password")] Insured insured)
        {
            if (ModelState.IsValid)
            {
                db.Entry(insured).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(insured);
        }

        // GET: Insureds/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insured insured = db.Insureds.Find(id);
            if (insured == null)
            {
                return HttpNotFound();
            }
            return View(insured);
        }

        // POST: Insureds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Insured insured = db.Insureds.Find(id);
            db.Insureds.Remove(insured);
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
