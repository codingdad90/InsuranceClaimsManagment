using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MAKClaims.Models;
using MAKClaims.HelperClasses;
using Microsoft.AspNet.Identity;
using Microsoft.Ajax.Utilities;
using Microsoft.Ajax;
using MAKClaims;

namespace Claims.Controllers
{
    [Authorize(Roles = "Manager, Admin, Superuser")]
    public class ManagersController : Controller
    {
        private Claimsconfig db = new Claimsconfig();


        
        public ActionResult ManagerHome(int? id, string sortOrder)
        {
            

            var uid = User.Identity.GetUserId();
            //var uroll = User.IsInRole("Admin");

            Manager managerMan = db.Managers.Where(man => man.LoginID.Equals(uid, StringComparison.Ordinal)).FirstOrDefault();
           
            if (id != null)
            {
                managerMan = db.Managers.Find(id);
            }

            var manager = managerMan;



            var allAdjustors = db.Adjustors.Include(m => m.Manager).Include(c => c.Claims).ToArray();

            var mansAdjustors = from a in allAdjustors
                         where a.ManagerId ==  manager.ManagerId            
                         select a;

            var listWithoutMan = from b in mansAdjustors
                    where b.IsManager == false
                    select b;
           
            var adjIds = from ai in listWithoutMan
                         select ai.AdjustorId;

            ViewBag.ReserveSort = String.IsNullOrEmpty(sortOrder) ? "reserves_desc" : "";
            ViewBag.ClaimTotalSort = sortOrder == "ClaimTotal" ? "claimtot_desc" : "ClaimTotal";
            ViewBag.DeductableSort = sortOrder == "Deductable" ? "deduc_desc" : "Deductable";
            ViewBag.AdjustorNameSort = sortOrder == "Name" ? "adjustorname_desc" : "Name";
            ViewBag.AmountPaidSort = sortOrder == "Paid" ? "amountpaid_desc" : "Paid";
            ViewBag.ID = 5; //using this to diplay or not display links in menu


            switch (sortOrder)
            {
                case "reserves_desc":
                    listWithoutMan = listWithoutMan.OrderByDescending(r => r.TotalReserve);
                    break;
                case "ClaimTotal":
                    listWithoutMan = listWithoutMan.OrderBy(c => c.TotalClaim);
                    break;
                case "claimtot_desc":
                    listWithoutMan = listWithoutMan.OrderByDescending(c => c.TotalClaim);
                    break;
                case "Deductable":
                    listWithoutMan = listWithoutMan.OrderBy(d => d.TotalDeductable);
                    break;
                case "deduc_desc":
                    listWithoutMan = listWithoutMan.OrderByDescending(d => d.TotalDeductable);
                    break;
                case "Name":
                    listWithoutMan = listWithoutMan.OrderBy(n => n.Name);
                    break;
                case "adjustorname_desc":
                    listWithoutMan = listWithoutMan.OrderByDescending(n => n.Name);
                    break;
                case "Paid":
                    listWithoutMan = listWithoutMan.OrderBy(a => a.TotalAmoutPaid);
                    break;
                case "amountpaid_desc":
                    listWithoutMan = listWithoutMan.OrderByDescending(a => a.TotalAmoutPaid);
                    break;
            }


            var greeting = "Hello ";
            if (DateTime.Now.Hour < 12)
            {
                 greeting = "Good Morning ";
            }
            else if (DateTime.Now.Hour < 17)
            {
                greeting = "Good Afternoon ";
            }
            else if (DateTime.Now.Hour < 24)
            {
                greeting = "Good Evening ";
            }
            else
            {
                
            }
            ViewData["Welcome"] = greeting + manager.Name;

            return View(listWithoutMan.ToList());
        }
      
        public ActionResult AdjustorView(int? id, string sortOrder)
        {
            var uid = User.Identity.GetUserId();
            var adj = db.Adjustors.Find(id);           
            var claims = db.Claims.Include(ca => ca.ClaimActions).ToArray();

            var adjClaims = from ac in claims
                            where ac.AdjustorId == adj.AdjustorId
                            select ac;
       
            var adjClaimsActive = from aso in adjClaims
                                 where aso.Status == true
                                 select aso;

            //var checkActive = TempData["Active"];

            //if (checkActive.ToString() == "Active")
            //{
            //    adjClaims = adjClaimsStatO;
            //}
            if (uid.Equals(adj.AdjustorId))
            {
                ViewBag.ID = 6; //using this to diplay or not display links in menu
            }
            //else
            //{
            //    ViewBag.ID = 7;
            //}
            




            ViewBag.Name = adj.Name;

            ViewBag.ReserveSort = String.IsNullOrEmpty(sortOrder) ? "reserves_desc" : "";
            ViewBag.AmountPaidSort = sortOrder == "Paid" ? "amountpaid_desc" : "Paid";
            ViewBag.DeductableSort = sortOrder == "Deductable" ? "deduc_desc" : "Deductable";
            ViewBag.StatusActive = sortOrder == "Status" ? "stat_notactive" : "Status";
            ViewBag.DateLossSort = sortOrder == "Date" ? "date_dec" : "Date";
            
            ViewBag.Results = adjClaims;
            switch (sortOrder)
            {
                case "reserves_desc":
                    adjClaimsActive = adjClaimsActive.OrderByDescending(r => r.Reserves);
                    break;
                case "Paid":
                    adjClaimsActive = adjClaimsActive.OrderBy(p => p.AmountPaid);
                    break;
                case "amountpaid_desc":
                    adjClaimsActive = adjClaimsActive.OrderByDescending(ap => ap.AmountPaid);
                    break;
                case "Deductable":
                    adjClaimsActive = adjClaimsActive.OrderBy(d => d.Deductable);
                    break;
                case "deduc_desc":
                    adjClaimsActive = adjClaimsActive.OrderByDescending(d => d.Deductable);
                    break;
                case "Status":
                    //TempData["Active"] = "Active";
                    adjClaimsActive = adjClaims;
                    break;
                case "stat_notactive":
                    adjClaims = adjClaimsActive;
                    break;
                case "Date":
                    adjClaimsActive = adjClaimsActive.OrderBy(dt => dt.DateOfLoss);
                    break;
                case "date_dec":
                    adjClaimsActive = adjClaimsActive.OrderByDescending(dt => dt.DateOfLoss);
                    break;
            }

            

            return View(adjClaimsActive.ToList());          
        }

        


        //GET: Managers
        public ActionResult Index()
        {
            
            var managerList = from m in db.Managers
                              where m.Active == true
                              select m;


            return View(managerList.ToList());
        }

        //GET: ManagersView
        public ActionResult ManagerView(int? id)
        {


            if (id == null)
            {
                id = 10001;
            }
            Manager manager = db.Managers.Find(id);
            var empolyeeList = db.Managers.Include(a => a.Adjustors);
            var managerEmploy = from e in empolyeeList
                                where e.ManagerId == manager.ManagerId
                                select e;



            return View(managerEmploy.ToList());
        }



        // GET: Managers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manager manager = db.Managers.Find(id);
            if (manager == null)
            {
                return HttpNotFound();
            }
            return View(manager);
        }

        // GET: Managers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Managers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ManagerId,Name,PhoneNumber,Email,Active")] Manager manager)
        {
            if (ModelState.IsValid)
            {
                db.Managers.Add(manager);
                db.SaveChanges();
                return RedirectToAction("ManagerHome", "Managers", new { area = "" });
            }

            return View(manager);
        }

        // GET: Managers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manager manager = db.Managers.Find(id);
            if (manager == null)
            {
                return HttpNotFound();
            }
            return View(manager);
        }

        // POST: Managers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ManagerId,Name,PhoneNumber,Email,Active")] Manager manager)
        {
            if (ModelState.IsValid)
            {
                db.Entry(manager).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(manager);
        }

        // GET: Managers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manager manager = db.Managers.Find(id);
            if (manager == null)
            {
                return HttpNotFound();
            }
            return View(manager);
        }

        // POST: Managers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Manager manager = db.Managers.Find(id);
            db.Managers.Remove(manager);
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
        public ActionResult EditClaim(int? id)
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
        public ActionResult EditClaim([Bind(Include = "ClaimId,AdjustorId,PropertyId,DateOfLoss,Attachment,Reserves,Deductable,AmountPaid,Status")] Claim claim)
        {
            if (ModelState.IsValid)
            {
                db.Entry(claim).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AdjustorView", "Managers", new { id = claim.AdjustorId });
            }
            ViewBag.AdjustorId = new SelectList(db.Adjustors, "AdjustorId", "Name", claim.AdjustorId);
            ViewBag.PropertyId = new SelectList(db.Properties, "PropertyId", "Address", claim.PropertyId);
            return View(claim);
        }
    }
}
