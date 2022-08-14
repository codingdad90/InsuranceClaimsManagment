using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MAKClaims.Models;

namespace MAKClaims.Controllers
{
    public class AttachmentsController : Controller
    {

        private Claimsconfig db = new Claimsconfig();
        // GET: Attachments
        public ActionResult Index()
        {
            //var attachments = db.Attachments.Include(a => a.ClaimAction);
            //return View(attachments.ToList());

            return View();
        }

        //[HttpPost]
        //public FileResult ViewFile(int id)
        //{
        //    Attachment attachment = db.Attachments.ToList().Find(a => a.AttachmentId == id);
        //    //string ext = Path.GetExtension(attachment.AttachmentName);
        //    return File(attachment.Attachment1, attachment.AttachmentName);
        //}
        [HttpPost]
        public FileResult Download(int id)
        {
            Attachment attachment = db.Attachments.ToList().Find(a => a.AttachmentId == id);
            //string ext = Path.GetExtension(attachment.AttachmentName);
            return File(attachment.Attachment1, attachment.AttachmentName);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload([Bind(Include = "ClaimId,Attachment")] int cid,  HttpPostedFileBase upload)
        {
            if (upload != null && upload.ContentLength > 0)
            {
                var file = new Attachment
                {
                    AttachmentName = System.IO.Path.GetFileName(upload.FileName),
                    ClaimId = cid,


                    //Path.Combine(Server.MapPath("~/UploadedFiles"),
                    //    Path.GetFileName(upload.FileName))

                };
                using (var reader = new System.IO.BinaryReader(upload.InputStream))
                {
                    file.Attachment1 = reader.ReadBytes(upload.ContentLength);
                    //claim.Attachment = reader.ReadBytes(upload.ContentLength);
                }
                //claim.Attachment = new byte[upload.ContentLength];
                //byte[] myfile = new byte[upload.InputStream.Length];
               // upload.InputStream.Read(claim.Attachment, 0, upload.ContentLength);
                db.Attachments.Add(file);
                db.SaveChanges();
                return RedirectToAction("ClaimDetails", "ManualViews", new { id = cid });
            }

            return View();


        }
        //[HttpPost]
        //public ActionResult Upload(HttpPostedFileBase file)
        //{
        //    if (file != null && file.ContentLength > 0)
        //        try
        //        {  //Server.MapPath takes the absolte path of folder 'Uploads'
        //            string path = Path.Combine(Server.MapPath("~/UploadedFiles"),
        //                                       Path.GetFileName(file.FileName));
        //            var attach = new Attachment();
        //            using (var binaryReader = new BinaryReader(Request.Files[0].InputStream))
        //            {

        //            }

        //            //Save file using Path+fileName take from above string
        //            file.SaveAs(path);



        //            ViewBag.Message = "File uploaded successfully";
        //        }
        //        catch (Exception ex)
        //        {
        //            ViewBag.Message = "ERROR:" + ex.Message.ToString();
        //        }
        //    else
        //    {
        //        ViewBag.Message = "You have not specified a file.";
        //    }
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult Upload(HttpPostedFileBase[] files)
        //{
        //    foreach (HttpPostedFileBase file in files)
        //    {
        //        if (file != null)
        //        {
        //            var inputFileName = Path.GetFileName(file.FileName);
        //            var serverSavePath = Path.Combine(Server.MapPath("~/UploadedFiles/") + inputFileName);
        //            file.SaveAs(serverSavePath);
        //            ViewBag.UploadStatus = files.Count().ToString() + " Files Uploaded Successfully.";
        //        }
        //    }
        //    return View();
        //}


        //[HttpPost]
        //public ActionResult UploadFiles(IEnumerable<HttpPostedFileBase> files)
        //{
        //    foreach (var file in files)
        //    {
        //        string filePath = Guid.NewGuid() + Path.GetExtension(file.FileName);
        //        file.SaveAs(Path.Combine(Server.MapPath("~/UploadedFiles"), filePath));
        //        //Here you can write code for save this information in your database if you want
        //    }
        //    return Json("file uploaded successfully");
        //}

        // GET: Attachments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attachment attachment = db.Attachments.Find(id);
            if (attachment == null)
            {
                return HttpNotFound();
            }
            return View(attachment);
        }

        // GET: Attachments/Create
        public ActionResult Create()
        {
            ViewBag.ClaimActionId = new SelectList(db.ClaimActions, "ClaimActionId", "Note");
            return View();
        }

        // POST: Attachments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AttachmentId,Attachment1,ClaimId")] Attachment attachment)
        {
            if (ModelState.IsValid)
            {
                db.Attachments.Add(attachment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClaimActionId = new SelectList(db.ClaimActions, "ClaimActionId", "Note", attachment.ClaimId);
            return View(attachment);
        }

        // GET: Attachments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attachment attachment = db.Attachments.Find(id);
            if (attachment == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClaimActionId = new SelectList(db.ClaimActions, "ClaimActionId", "Note", attachment.ClaimId);
            return View(attachment);
        }

        // POST: Attachments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AttachmentId,Attachment1,ClaimActionId")] Attachment attachment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(attachment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClaimActionId = new SelectList(db.ClaimActions, "ClaimActionId", "Note", attachment.ClaimId);
            return View(attachment);
        }

        // GET: Attachments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attachment attachment = db.Attachments.Find(id);
            if (attachment == null)
            {
                return HttpNotFound();
            }
            return View(attachment);
        }

        // POST: Attachments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Attachment attachment = db.Attachments.Find(id);
            db.Attachments.Remove(attachment);
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
