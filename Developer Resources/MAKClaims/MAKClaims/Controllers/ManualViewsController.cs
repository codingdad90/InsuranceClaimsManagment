using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MAKClaims.Models;
using Microsoft.AspNet.Identity;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.SqlClient;


namespace ManualViews.Controllers
{
    public class ManualViewsController : Controller
    {
        private Claimsconfig db = new Claimsconfig();

        // GET: Claims
        //public ActionResult Adjusterhome()
        //{
        //    var claims = db.Claims.Include(c => c.Adjustor).Include(c => c.Property);
        //    return View(claims.ToList());
        //}
        public ActionResult Adjusterhome(string sortOrder)
        {
            var uid = User.Identity.GetUserId();
            Adjustor currentAdjuster = db.Adjustors.Where(a => a.LoginID.Equals(uid, StringComparison.Ordinal)).FirstOrDefault();
            ViewBag.ID = 6;
            if (uid != null)
            {
                var claims = db.Claims.Include(c => c.Adjustor).Include(c => c.Property).Where(c => c.AdjustorId.Equals(currentAdjuster.AdjustorId));
                claims = claims.Where(c => c.Status.Equals(true));
                ViewBag.ClaimNumsortParm = String.IsNullOrEmpty(sortOrder) ? "claimnum_desc" : "";
                ViewBag.NameSortParm = sortOrder == "Name" ? "name_desc" : "Name";
                ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
                ViewBag.ReservesSortParm = sortOrder == "Reserves" ? "reserves_desc" : "Reserves";

                switch (sortOrder)
                {
                    case "claimnum_desc":
                        claims = claims.OrderByDescending(c => c.ClaimId);
                        break;
                    case "Name":
                        claims = claims.OrderBy(c => c.Property.Insured.Name_Last);
                        break;
                    case "name_desc":
                        claims = claims.OrderByDescending(c => c.Property.Insured.Name_Last);
                        break;
                    case "Date":
                        claims = claims.OrderBy(c => c.DateOfLoss);
                        break;
                    case "date_desc":
                        claims = claims.OrderByDescending(c => c.DateOfLoss);
                        break;
                    case "Reserves":
                        claims = claims.OrderBy(c => c.Reserves);
                        break;
                    case "reserves_desc":
                        claims = claims.OrderByDescending(c => c.Reserves);
                        break;

                    default:
                        claims = claims.OrderBy(c => c.ClaimId);
                        break;
                }
                return View(claims.ToList());

            }
            //This should be replaced before we finish but we are keeping it to view all claims easily for debug
            else
            {
                var claims = db.Claims.Include(c => c.Adjustor).Include(c => c.Property);
                ViewBag.ClaimNumsortParm = String.IsNullOrEmpty(sortOrder) ? "claimnum_desc" : "";
                ViewBag.NameSortParm = sortOrder == "Name" ? "name_desc" : "Name";
                ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
                ViewBag.ReservesSortParm = sortOrder == "Reserves" ? "reserves_desc" : "Reserves";

                switch (sortOrder)
                {
                    case "claimnum_desc":
                        claims = claims.OrderByDescending(c => c.ClaimId);
                        break;
                    case "Name":
                        claims = claims.OrderBy(c => c.Property.Insured.Name_Last);
                        break;
                    case "name_desc":
                        claims = claims.OrderByDescending(c => c.Property.Insured.Name_Last);
                        break;
                    case "Date":
                        claims = claims.OrderBy(c => c.DateOfLoss);
                        break;
                    case "date_desc":
                        claims = claims.OrderByDescending(c => c.DateOfLoss);
                        break;
                    case "Reserves":
                        claims = claims.OrderBy(c => c.Reserves);
                        break;
                    case "reserves_desc":
                        claims = claims.OrderByDescending(c => c.Reserves);
                        break;

                    default:
                        claims = claims.OrderBy(c => c.ClaimId);
                        break;
                }
                return View(claims.ToList());
            }
        }


        public ActionResult Tasks()
        {

            if (this.User.IsInRole("Manager"))
            {
                var todos = db.ClaimActions.Include(c => c.Adjustor).Include(c => c.Claim).Where(c => c.ActionId.Equals(1));

                todos = todos.Where(c => c.Complete.Equals(null));




                var todoadj = db.Adjustors.Where(a => todos.Any(c => a.AdjustorId.Equals(c.AdjustorId))).Distinct().ToList();

                ViewBag.AdjustorId = new SelectList(todoadj, "AdjustorId", "Name");
                return View(todos.ToList().OrderBy(c => c.Date));

            }


            else
            {
                if (this.User.IsInRole("Adjustor"))
                {
                    var uid = User.Identity.GetUserId();
                    Adjustor currentAdjuster = db.Adjustors.Where(a => a.LoginID.Equals(uid, StringComparison.Ordinal)).FirstOrDefault();
                    var todos = db.ClaimActions.Include(c => c.Adjustor).Include(c => c.Claim).Where(c => c.AdjustorId.Equals(currentAdjuster.AdjustorId)).Where(c => c.ActionId == 1);

                    todos = todos.Where(c => c.Complete.Equals(null));


                    var todoadj = db.Adjustors.Where(a => todos.Any(c => a.AdjustorId.Equals(c.AdjustorId))).Distinct().ToList();

                    ViewBag.AdjustorId = new SelectList(todoadj, "AdjustorId", "Name");
                    if (uid != null)
                    {


                        return View(todos.ToList().OrderBy(c => c.Date));
                    }
                    return View();
                }
                return View();
            }
        }
        public ActionResult CompleteTasks()
        {

            if (this.User.IsInRole("Manager"))
            {
                var todos = db.ClaimActions.Include(c => c.Adjustor).Include(c => c.Claim).Where(c => c.ActionId.Equals(1));
                todos = todos.Include(c => c.Claim.Property);


                todos = todos.Where(c => c.Complete.Equals(true));
                //var todoadj = db.Adjustors.Where(a => todos.Any(c => a.AdjustorId.Equals(c.AdjustorId))).Distinct().ToList();

                //ViewBag.AdjustorId = new SelectList(todoadj, "AdjustorId", "Name");
                return View(todos.ToList().OrderBy(c => c.Date));

            }


            else
            {
                if (this.User.IsInRole("Adjustor"))
                {
                    var uid = User.Identity.GetUserId();
                    Adjustor currentAdjuster = db.Adjustors.Where(a => a.LoginID.Equals(uid, StringComparison.Ordinal)).FirstOrDefault();
                    var todos = db.ClaimActions.Include(c => c.Adjustor).Include(c => c.Claim).Where(c => c.AdjustorId.Equals(currentAdjuster.AdjustorId)).Where(c => c.ActionId == 1);

                    todos = todos.Where(c => c.Complete.Equals(true));


                    var todoadj = db.Adjustors.Where(a => todos.Any(c => a.AdjustorId.Equals(c.AdjustorId))).Distinct().ToList();

                    ViewBag.AdjustorId = new SelectList(todoadj, "AdjustorId", "Name");
                    if (uid != null)
                    {


                        return View(todos.ToList().OrderBy(c => c.Date));
                    }
                    return View();
                }
                return View();
            }
        }
        public ActionResult _IndexByTag(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            {
                if (this.User.IsInRole("Manager"))
                {
                    var todos = db.ClaimActions.Include(c => c.Adjustor).Include(c => c.Claim).Where(c => c.ActionId.Equals(1));
                    todos = todos.Include(c => c.Claim.Property);
                    todos = todos.Where(c => c.Complete.Equals(null));
                    todos = todos.Where(c => c.AdjustorId.Equals(id));
                    return PartialView("_Tasks", todos.ToList().OrderBy(c => c.Date));

                }


                else
                {
                    if (this.User.IsInRole("Adjustor"))
                    {
                        var uid = User.Identity.GetUserId();
                        Adjustor currentAdjuster = db.Adjustors.Where(a => a.LoginID.Equals(uid, StringComparison.Ordinal)).FirstOrDefault();

                        if (uid != null)
                        {
                            var todos = db.ClaimActions.Include(c => c.Adjustor).Include(c => c.Claim).Where(c => c.AdjustorId.Equals(currentAdjuster.AdjustorId)).Where(c => c.ActionId == 1);
                            todos = todos.Include(c => c.Claim.Property);
                            todos = todos.Where(c => c.AdjustorId.Equals(id));
                            return PartialView("_Tasks", todos.ToList().OrderBy(c => c.Date));
                        }
                        return View();
                    }
                    return View();
                }
            }

        }
        public ActionResult _IndexByName(string parm)
        {
            db.Configuration.ProxyCreationEnabled = false;
            {
                if (this.User.IsInRole("Manager"))
                {
                    var todos = db.ClaimActions.Include(c => c.Adjustor).Include(c => c.Claim).Where(c => c.ActionId.Equals(1));
                    todos = todos.Include(c => c.Claim.Property);
                    todos = todos.Where(c => c.Complete.Equals(null));
                    todos = todos.Where(c => c.Claim.Property.Address.Contains(parm));
                    return PartialView("_Tasks", todos.ToList().OrderBy(c => c.Date));

                }


                else
                {
                    if (this.User.IsInRole("Adjustor"))
                    {
                        var uid = User.Identity.GetUserId();
                        Adjustor currentAdjuster = db.Adjustors.Where(a => a.LoginID.Equals(uid, StringComparison.Ordinal)).FirstOrDefault();

                        if (uid != null)
                        {
                            var todos = db.ClaimActions.Include(c => c.Adjustor).Include(c => c.Claim).Where(c => c.AdjustorId.Equals(currentAdjuster.AdjustorId)).Where(c => c.ActionId == 1);
                            todos = todos.Include(c => c.Claim.Property);
                            todos = todos.Where(c => c.Complete.Equals(null));
                            todos = todos.Where(c => c.Claim.Property.Address.Contains(parm));
                            return PartialView("_Tasks", todos.ToList().OrderBy(c => c.Date));
                        }
                        return View();
                    }
                    return View();
                }
            }


        }
        public ActionResult _IndexByTag2(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            {
                if (this.User.IsInRole("Manager"))
                {
                    var todos = db.ClaimActions.Include(c => c.Adjustor).Include(c => c.Claim).Where(c => c.ActionId.Equals(1));
                    todos = todos.Where(c => c.Complete.Equals(true));
                    todos = todos.Where(c => c.AdjustorId.Equals(id));
                    return PartialView("_Tasks", todos.ToList().OrderBy(c => c.Date));

                }


                else
                {
                    if (this.User.IsInRole("Adjustor"))
                    {
                        var uid = User.Identity.GetUserId();
                        Adjustor currentAdjuster = db.Adjustors.Where(a => a.LoginID.Equals(uid, StringComparison.Ordinal)).FirstOrDefault();

                        if (uid != null)
                        {
                            var todos = db.ClaimActions.Include(c => c.Adjustor).Include(c => c.Claim).Where(c => c.AdjustorId.Equals(currentAdjuster.AdjustorId)).Where(c => c.ActionId == 1);
                            todos = todos.Where(c => c.Complete.Equals(true));
                            todos = todos.Where(c => c.AdjustorId.Equals(id));
                            return PartialView("_Tasks", todos.ToList().OrderBy(c => c.Date));
                        }
                        return View();
                    }
                    return View();
                }
            }

        }
        public ActionResult _IndexByName2(string parm)
        {
            db.Configuration.ProxyCreationEnabled = false;
            {
                if (this.User.IsInRole("Manager"))
                {
                    var todos = db.ClaimActions.Include(c => c.Adjustor).Include(c => c.Claim).Where(c => c.ActionId.Equals(1));
                    todos = todos.Where(c => c.Complete.Equals(true));
                    todos = todos.Where(c => c.Claim.Property.Address.Contains(parm));
                    return PartialView("_Tasks", todos.ToList().OrderBy(c => c.Date));

                }


                else
                {
                    if (this.User.IsInRole("Adjustor"))
                    {
                        var uid = User.Identity.GetUserId();
                        Adjustor currentAdjuster = db.Adjustors.Where(a => a.LoginID.Equals(uid, StringComparison.Ordinal)).FirstOrDefault();

                        if (uid != null)
                        {
                            var todos = db.ClaimActions.Include(c => c.Adjustor).Include(c => c.Claim).Where(c => c.AdjustorId.Equals(currentAdjuster.AdjustorId)).Where(c => c.ActionId == 1);
                            todos = todos.Where(c => c.Complete.Equals(true));
                            todos = todos.Where(c => c.Claim.Property.Address.Contains(parm));
                            return PartialView("_Tasks", todos.ToList().OrderBy(c => c.Date));
                        }
                        return View();
                    }
                    return View();
                }
            }


        }
        public ActionResult ClaimActionedit(int? id)
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
            
            var currentClaimAction = db.ClaimActions.Include(ca => ca.Claim).Include(ca => ca.Adjustor).Where(ca => ca.ClaimActionId.Equals(id));
            var claim = db.Claims.Include(c => c.Adjustor).Include(c=>c.ClaimActions);
            claim = claim.Where(c => c.ClaimId.Equals(claimAction.ClaimId));


            ViewBag.ActionId = new SelectList(db.Actions.Where(a=>a.ActionId.Equals(1)), "ActionId", "ActionType", claimAction.ActionId);
            ViewBag.AdjustorId = new SelectList(db.Adjustors, "AdjustorId", "Name", claimAction.AdjustorId);
            ViewBag.ClaimId = new SelectList(claim, "ClaimId", "ClaimId", claimAction.ClaimId);
            return View(claimAction);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ClaimActionedit([Bind(Include = "ClaimActionId,Date,DollarAmount,Note,ClaimId,ActionId,AdjustorId,Complete")] ClaimAction claimAction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(claimAction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Tasks", "ManualViews");
            }
            ViewBag.ActionId = new SelectList(db.Actions, "ActionId", "ActionType", claimAction.ActionId);
            ViewBag.AdjustorId = new SelectList(db.Adjustors, "AdjustorId", "Name", claimAction.AdjustorId);
            ViewBag.ClaimId = new SelectList(db.Claims, "ClaimId", "ClaimId", claimAction.ClaimId);
            return View(claimAction);
        }


        [Authorize(Roles = "Manager")]
        public ActionResult AddTask(int? id)

        {
            var uid = User.Identity.GetUserId();

            Manager managerMan = db.Managers.Where(man => man.LoginID.Equals(uid, StringComparison.Ordinal)).FirstOrDefault();

            var manager = managerMan;
            var allAdjustors = db.Adjustors.Include(m => m.Manager).Include(c => c.Claims);

            var mansAdjustors = from a in allAdjustors
                                where a.ManagerId == manager.ManagerId
                                select a;

            var listWithoutMan = from b in mansAdjustors
                                 where b.IsManager == false
                                 select b;

            var adjIds = from ai in listWithoutMan
                         select ai.AdjustorId;
            var maclaims = db.Claims.Include(c => c.Adjustor).Where(c => mansAdjustors.Any(i => c.AdjustorId.Equals(i.AdjustorId)));
            ViewBag.ActionId = new SelectList(db.Actions, "ActionId", "ActionType");
            ViewBag.AdjustorId = new SelectList(mansAdjustors, "AdjustorId", "Name");
            ViewBag.ClaimId = new SelectList(maclaims, "ClaimId", "ClaimId");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddTask([Bind(Include = "ClaimActionId,Date,DollarAmount,Note,ClaimId,ActionId,AdjustorId")] ClaimAction claimAction)
        {
            if (ModelState.IsValid)
            {
                db.ClaimActions.Add(claimAction);
                db.SaveChanges();
                return RedirectToAction("ManagerHome", "Managers", new { area = "" });
            }
            var uid = User.Identity.GetUserId();

            Manager managerMan = db.Managers.Where(man => man.LoginID.Equals(uid, StringComparison.Ordinal)).FirstOrDefault();



            var manager = managerMan;
            var allAdjustors = db.Adjustors.Include(m => m.Manager).Include(c => c.Claims);

            var mansAdjustors = from a in allAdjustors
                                where a.ManagerId == manager.ManagerId
                                select a;

            var listWithoutMan = from b in mansAdjustors
                                 where b.IsManager == false
                                 select b;

            var adjIds = from ai in listWithoutMan
                         select ai.AdjustorId;
            var maclaims = db.Claims.Include(c => c.Adjustor).Where(c => mansAdjustors.Any(i => c.AdjustorId.Equals(i.AdjustorId)));
            ViewBag.ActionId = new SelectList(db.Actions, "ActionId", "ActionType", claimAction.ActionId);
            ViewBag.AdjustorId = new SelectList(allAdjustors, "AdjustorId", "Name", claimAction.AdjustorId);
            ViewBag.ClaimId = new SelectList(maclaims, "ClaimId", "ClaimId", claimAction.ClaimId);
            return View(claimAction);
        }

        public ActionResult AddClaimAction(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Claim theClaim = db.Claims.Find(id);
            if (theClaim == null)
            {
                return HttpNotFound();
            }
            var uid = User.Identity.GetUserId();
            var currentAdjuster = db.Adjustors.Where(a => a.LoginID.Equals(uid, StringComparison.Ordinal));
            var currentClaim = db.Claims.Where(c => c.ClaimId.Equals((int)id)).ToList();
            //List<Claim> currentClaim = db.Claims.Where(c => c.ClaimId.Equals(id));


            ViewBag.ActionId = new SelectList(db.Actions, "ActionId", "ActionType");
            ViewBag.AdjustorId = new SelectList(currentAdjuster, "AdjustorId", "Name");
            ViewBag.ClaimId = new SelectList(currentClaim, "ClaimId", "ClaimId");

            //ViewBag.ActionId = new SelectList(db.Actions, "ActionId", "ActionType");
            //ViewBag.AdjustorId = new SelectList(db.Adjustors, "AdjustorId", "Name");
            //ViewBag.ClaimId = new SelectList(db.Claims, "ClaimId", "ClaimId");
            return View();
        }

        // POST: ClaimActions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddClaimAction([Bind(Include = "ClaimActionId,Date,DollarAmount,Note,ClaimId,ActionId,AdjustorId")] ClaimAction claimAction)
        {
            if (ModelState.IsValid)
            {
                db.ClaimActions.Add(claimAction);
                db.SaveChanges();
                return RedirectToAction("Adjusterhome", "ManualViews");
            }

            var uid = User.Identity.GetUserId();
            Adjustor currentAdjuster = db.Adjustors.Where(a => a.LoginID.Equals(uid, StringComparison.Ordinal)).FirstOrDefault();
            //var theClaim = db.Claims.Where(c => c.ClaimId.Equals(id)).FirstOrDefault();
            ViewBag.ActionId = new SelectList(db.Actions, "ActionId", "ActionType");
            ViewBag.AdjustorId = new SelectList(db.Adjustors.Where(a => a.LoginID.Equals(uid, StringComparison.Ordinal)), "AdjustorId", "Name");
            ViewBag.ClaimId = new SelectList(db.Claims, "ClaimId", "ClaimId", claimAction.ClaimId);
            return View();
        }
        public ActionResult NewClaim()
        {
            var combinedprop = db.Properties
                .AsEnumerable()
                .Select(s => new
                {
                    PropertyId = s.PropertyId,
                    FullAddress = string.Format("{0}  {1}, {2}", s.Address, s.City, s.State),
                    State = s.State,
                    City = s.City

                }).ToList().OrderBy(c => c.State).ThenBy(c => c.City);
            var sortedInsureds = db.Insureds.ToList().OrderBy(i => i.Name_Last);


            ViewBag.AdjustorId = new SelectList(db.Adjustors.OrderBy(x => x.Name), "AdjustorId", "Name");
            ViewBag.PropertyId = new SelectList(combinedprop, "PropertyId", "FullAddress");
            ViewBag.InsuredId = new SelectList(sortedInsureds, "InsuredId", "FullName");
            return View(new Claim());
        }

        // POST: Claims/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewClaim([Bind(Include = "ClaimId,AdjustorId,PropertyId,DateOfLoss,Attachment,Reserves,Deductable,AmountPaid,Status")] Claim claim, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    var file = new Attachment
                    {
                        AttachmentName = System.IO.Path.GetFileName(upload.FileName),
                        ClaimId = claim.ClaimId,


                    };
                    using (var reader = new System.IO.BinaryReader(upload.InputStream))
                    {
                        file.Attachment1 = reader.ReadBytes(upload.ContentLength);
                        claim.Attachment = reader.ReadBytes(upload.ContentLength);
                    }
                    claim.Attachment = new byte[upload.ContentLength];
                    //byte[] myfile = new byte[upload.InputStream.Length];
                    upload.InputStream.Read(claim.Attachment, 0, upload.ContentLength);
                    db.Attachments.Add(file);

                }

                db.Claims.Add(claim);
                db.SaveChanges();
                if (this.User.IsInRole("Manager"))
                {
                    return RedirectToAction("Managerhome", "Managers");
                }
                else
                {
                    return RedirectToAction("Adjusterhome", "ManualViews");
                }

            }
            var combinedprop = db.Properties
                .AsEnumerable()
                .Select(s => new
                {
                    PropertyId = s.PropertyId,
                    FullAddress = string.Format("{0}  {1}, {2}", s.Address, s.City, s.State)
                }).ToList();


            ViewBag.AdjustorId = new SelectList(db.Adjustors, "AdjustorId", "Name");
            ViewBag.PropertyId = new SelectList(combinedprop, "PropertyId", "FullAddress");
            return View(claim);
        }
        [HttpPost]
        public ActionResult SaveProperty([Bind(Include = "PropertyId,InsuredId,Address,State,Zip,City")] Property property, FormCollection data)
        {
            var propertyid = Int32.Parse(data["PropertyId"]);
            var address = data["Address"];
            var city = data["City"];
            var state = data["State"];
            var zip = Int32.Parse(data["Zip"]);

            property = new Property() { PropertyId = propertyid, Address = address, City = city, State = state, Zip = zip };
            db.Properties.Add(property);
            db.SaveChanges();
            return RedirectToAction("NewClaim");
        }

        public ActionResult NewProperty()
        {
            var vInsured = db.Insureds.OrderBy(i => i.Name_Last).ToList();
            ViewBag.InsuredId = new SelectList(vInsured, "InsuredId", "FullName");
            return View();
        }

        // POST: Properties/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewProperty([Bind(Include = "PropertyId,InsuredId,Address,State,Zip,City")] Property property)
        {
            if (ModelState.IsValid)
            {
                db.Properties.Add(property);
                db.SaveChanges();
                return RedirectToAction("Employee", "ManualViews", new { area = "" });
            }

            ViewBag.InsuredId = new SelectList(db.Insureds, "InsuredId", "MailingAddress", property.InsuredId);
            return View(property);
        }
        public ActionResult NewPropertyModal()
        {
            ViewBag.InsuredId = new SelectList(db.Insureds, "InsuredId", "MailingAddress");
            return PartialView();
        }

        // POST: Properties/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewPropertyModal([Bind(Include = "PropertyId,InsuredId,Address,State,Zip,City")] Property property)
        {
            if (ModelState.IsValid)
            {
                db.Properties.Add(property);
                db.SaveChanges();
                return RedirectToAction("Employee", "ManualViews", new { area = "" });
            }

            ViewBag.InsuredId = new SelectList(db.Insureds, "InsuredId", "MailingAddress", property.InsuredId);
            return PartialView(property);
        }


        public ActionResult ClaimDetails(int? id)

        {

            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Claim claim = db.Claims.Find(id);
            Property property = db.Properties.Find(claim.PropertyId);
            var iName = property.Insured.FullName;
            ViewBag.IName = iName;
            var attachment = new List<Attachment>();
            attachment = db.Attachments.ToList();
            var myAttachments = attachment.Where(a => a.ClaimId.Equals(claim.ClaimId));
            ViewBag.FileNames = myAttachments;
            var claimActions = claim.ClaimActions.ToList().OrderByDescending(c => c.Date);
            ViewBag.Claim = claim;
            ViewBag.AdjusterName = claim.Adjustor.Name;
            ViewBag.ClaimActions = claimActions;
            var assignAdjusterlist = db.Adjustors.Where(a => a.AdjustorId.Equals(claim.AdjustorId)).ToList();
            var uid = User.Identity.GetUserId();
            var currentAdjuster = db.Adjustors.Where(a => a.LoginID.Equals(uid, StringComparison.Ordinal));
            var currentClaim = db.Claims.Where(c => c.ClaimId.Equals((int)id)).ToList();




            ViewBag.ActionId = new SelectList(db.Actions, "ActionId", "ActionType");
            ViewBag.AdjustorId = new SelectList(currentAdjuster, "AdjustorId", "Name");
            ViewBag.AssignAd = new SelectList(assignAdjusterlist, "AdjustorId", "Name");
            ViewBag.AllAd = new SelectList(db.Adjustors.OrderBy(a => a.Name), "AdjustorId", "Name");
            ViewBag.ClaimId = new SelectList(currentClaim, "ClaimId", "ClaimId");


            if (claim == null)
            {
                return HttpNotFound();
            }
            return View();
        }
        public ActionResult ClaimDetails2(int? id)

        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Claim claim = db.Claims.Find(id);
            var claimActions = claim.ClaimActions.ToList().OrderByDescending(c => c.Date);
            ViewBag.Claim = claim;
            ViewBag.AdjusterName = claim.Adjustor.Name;
            ViewBag.ClaimActions = claimActions;
            var assignAdjusterlist = db.Adjustors.Where(a => a.AdjustorId.Equals(claim.AdjustorId)).ToList();
            var uid = User.Identity.GetUserId();
            var currentAdjuster = db.Adjustors.Where(a => a.LoginID.Equals(uid, StringComparison.Ordinal));
            var currentClaim = db.Claims.Where(c => c.ClaimId.Equals((int)id)).ToList();




            ViewBag.ActionId = new SelectList(db.Actions, "ActionId", "ActionType");
            //ViewBag.AdjustorId = new SelectList(currentAdjuster, "AdjustorId", "Name");
            ViewBag.AssignAd = new SelectList(assignAdjusterlist, "AdjustorId", "Name");
            ViewBag.AdjustorId = new SelectList(db.Adjustors.OrderBy(a => a.Name), "AdjustorId", "Name");
            ViewBag.ClaimId = new SelectList(currentClaim, "ClaimId", "ClaimId");


            if (claim == null)
            {
                return HttpNotFound();
            }
            return View();
        }


        [HttpPost]
        public ActionResult AddClaimActionModal1(ClaimAction claimAction)
        {
            db.ClaimActions.Add(claimAction);
            db.SaveChanges();
            return RedirectToAction("Employee", "ManualViews");

        }

        //Not currently working, saving for possibly after project is due to figure it out
        [HttpPost]
        public ActionResult AddClaimActionModal(ClaimAction claimAction, FormCollection fc)
        {
            var date = claimAction.Date;
            var actionId = claimAction.ActionId;
            var dollarAmount = claimAction.DollarAmount;
            var note = claimAction.Note;
            var claimId = claimAction.ClaimId;
            var assignAd = fc["AssignAd"];
            var allAd = fc["AllAd"];

            var assignAdf = fc["AssignAd"];

            if (assignAd == null)
            {
                if (allAd == null)
                {
                    db.ClaimActions.Add(claimAction);
                    db.SaveChanges();
                    return RedirectToAction("Employee", "ManualViews");
                }
                else
                {
                    var allAdf = Convert.ToInt32(fc["AllAd"]);
                    claimAction = new ClaimAction() { Date = date, DollarAmount = dollarAmount, Note = note, ClaimId = claimId, ActionId = actionId, AdjustorId = allAdf };
                    db.ClaimActions.Add(claimAction);
                    db.SaveChanges();
                    return RedirectToAction("Employee", "ManualViews");
                }

            }
            else
            {
                var assignf = Convert.ToInt32(fc["AssignAd"]);
                claimAction = new ClaimAction() { Date = date, DollarAmount = dollarAmount, Note = note, ClaimId = claimId, ActionId = actionId, AdjustorId = assignf };
                db.ClaimActions.Add(claimAction);
                db.SaveChanges();
                return RedirectToAction("Employee", "ManualViews");

            }
        }


        [Authorize(Roles = "Admin, Superuser")]
        public ActionResult Admin()
        {
            return View();
        }

        [Authorize(Roles = "Manager, Admin, Adjustor, Superuser")]
        public ActionResult Employee()
        {
            var uRollM = User.IsInRole("Manager");
            var uRollAdmin = User.IsInRole("Admin");
            if (uRollM != false)
            {
                return RedirectToAction("ManagerHome", "Managers");
            }
            else if (uRollAdmin != false)
            {
                return RedirectToAction("Admin", "ManualViews");
            }
            else
            {
                return RedirectToAction("Adjusterhome", "ManualViews");
            }


        }





        //// GET: Claims/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Claim claim = db.Claims.Find(id);
        //    if (claim == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(claim);
        //}

        //// GET: Claims/Create
        //public ActionResult Create()
        //{
        //    ViewBag.AdjustorId = new SelectList(db.Adjustors, "AdjustorId", "Name");
        //    ViewBag.PropertyId = new SelectList(db.Properties, "PropertyId", "Address");
        //    return View();
        //}

        //// POST: Claims/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "ClaimId,AdjustorId,PropertyId,DateOfLoss,Attachment,Reserves,Deductable,AmountPaid,Status")] Claim claim)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Claims.Add(claim);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.AdjustorId = new SelectList(db.Adjustors, "AdjustorId", "Name", claim.AdjustorId);
        //    ViewBag.PropertyId = new SelectList(db.Properties, "PropertyId", "Address", claim.PropertyId);
        //    return View(claim);
        //}

        //// GET: Claims/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Claim claim = db.Claims.Find(id);
        //    if (claim == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.AdjustorId = new SelectList(db.Adjustors, "AdjustorId", "Name", claim.AdjustorId);
        //    ViewBag.PropertyId = new SelectList(db.Properties, "PropertyId", "Address", claim.PropertyId);
        //    return View(claim);
        //}

        //// POST: Claims/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "ClaimId,AdjustorId,PropertyId,DateOfLoss,Attachment,Reserves,Deductable,AmountPaid,Status")] Claim claim)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(claim).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.AdjustorId = new SelectList(db.Adjustors, "AdjustorId", "Name", claim.AdjustorId);
        //    ViewBag.PropertyId = new SelectList(db.Properties, "PropertyId", "Address", claim.PropertyId);
        //    return View(claim);
        //}

        //// GET: Claims/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Claim claim = db.Claims.Find(id);
        //    if (claim == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(claim);
        //}

        //// POST: Claims/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Claim claim = db.Claims.Find(id);
        //    db.Claims.Remove(claim);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
