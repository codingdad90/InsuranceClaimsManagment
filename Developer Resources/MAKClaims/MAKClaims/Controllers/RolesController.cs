using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MAKClaims.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MAKClaims.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        
        private ApplicationDbContext dbc = new ApplicationDbContext();
        // GET: Roles
        public ActionResult Index()
        {
            var r =  dbc.Roles.ToList();
            
            return View(r);
        }
        //GET: Create
        public ActionResult Create()
        {


            return View();
        }
        //POST: Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                dbc.Roles.Add(new IdentityRole()
                {
                    Name = collection["RoleName"]
                });
                dbc.SaveChanges();
                ViewBag.ResultMessage = "Role created successfully !";
                return View("Create");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(string RoleName)
        {
            var thisRole = dbc.Roles.Where(r => r.Name.Equals(RoleName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            dbc.Roles.Remove(thisRole);
            dbc.SaveChanges();
            return RedirectToAction("Create");
        }

        //
        // GET: /Roles/Edit/5
        public ActionResult Edit(string roleName)
        {
            var thisRole = dbc.Roles.Where(r => r.Name.Equals(roleName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            return View(thisRole);
        }

        //
        // POST: /Roles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Microsoft.AspNet.Identity.EntityFramework.IdentityRole role)
        {
            try
            {
                dbc.Entry(role).State = System.Data.Entity.EntityState.Modified;
                dbc.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //ManageUsers
        public ActionResult ManageUserRoles()
        {
            var list = dbc.Roles.OrderBy(r => r.Name).ToList().Select(rr =>         
            new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;
            return View();
        }


        public ActionResult ManageUsers()
        {
            // populate roles for the view dropdown
            var list = dbc.Roles.OrderBy(r => r.Name).ToList().Select(rr =>
            new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<ActionResult> RoleAddToUser(string UserName, string RoleName)
        {
            ApplicationUser user = dbc.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            ApplicationUser userEmail = dbc.Users.Where(u => u.Email.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            if (UserName.Contains("@"))
            {
                user = userEmail;
            }
            

            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(dbc));

            if (user != null)
            {
                await manager.AddToRoleAsync(user.Id, RoleName);
                ViewBag.ResultMessage = "Role created successfully!";
            }
            else
            {
                ViewBag.ResultMessage = "ID not found for this user!";
            }

            var list = dbc.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;
            return await System.Threading.Tasks.Task.FromResult(View("ManageUserRoles"));




        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<ActionResult> GetRoles(string UserName)
        {
            if (!string.IsNullOrWhiteSpace(UserName))
            {
                ApplicationUser user = dbc.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                ApplicationUser userEmail = dbc.Users.Where(u => u.Email.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                if (UserName.Contains("@"))
                {
                    user = userEmail;
                }

                var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(dbc));
                if (user == null)
                {
                    ViewBag.ResultMessage2 = "No user id found!";
                }
                else
                {
                    ViewBag.RolesForThisUser = await manager.GetRolesAsync(user.Id);
                }

                // Populate roles for the view dropdown
                var list = dbc.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
                ViewBag.Roles = list;
            }

            return View("ManageUserRoles");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<ActionResult> DeleteRoleForUser(string UserName, string RoleName)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(dbc));

            ApplicationUser user = dbc.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            ApplicationUser userEmail = dbc.Users.Where(u => u.Email.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            if (UserName.Contains("@"))
            {
                user = userEmail;
            }

            if (user != null)
            {
                if (await manager.IsInRoleAsync(user.Id, RoleName))
                {
                    await manager.RemoveFromRoleAsync(user.Id, RoleName);
                    ViewBag.ResultMessage = "Role removed from this user successfully !";
                }
                else
                {
                    ViewBag.ResultMessage = "This user doesn't belong to selected role.";
                }
            }
            else
            {
                ViewBag.ResultMessage = "There is no user with this ID!";
            }

            // prepopulat roles for the view dropdown
            var list = dbc.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;

            return View("ManageUserRoles");
        }
    }
}