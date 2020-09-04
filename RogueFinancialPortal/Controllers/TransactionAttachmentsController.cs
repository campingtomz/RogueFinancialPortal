using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using RogueFinancialPortal.Helpers;
using RogueFinancialPortal.Models;

namespace RogueFinancialPortal.Controllers
{
    public class TransactionAttachmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

     


        // POST: TransactionAttachments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TransactionIdFileNam")] TransactionAttachment transactionAttachment, string AttachmentDescription, HttpPostedFileBase file)
        {
            if (file == null)
            {
                TempData["Error"] = "You must Supply a vaild File";
                return RedirectToAction("Dashboard", "Tickets", new { id = transactionAttachment.TransactionId });
            }
            if (ModelState.IsValid)
            {
                if (FileUploadValidator.IsWebFriendlyImage(file) || FileUploadValidator.IsWebFriendlyFile(file))
                {
                    transactionAttachment.Created = DateTime.Now;
                    transactionAttachment.OwnerId = User.Identity.GetUserId();
                    transactionAttachment.Description = AttachmentDescription;

                    var fileName = FileStamp.MakeUnique(file.FileName);
                    var serverFolder = WebConfigurationManager.AppSettings["DefaultAttachmentFolder"];
                    file.SaveAs(Path.Combine(Server.MapPath(serverFolder), fileName));
                    transactionAttachment.FilePath = $"{serverFolder}{fileName}";
                    transactionAttachment.FileName = fileName;
                    db.TransactionAttachments.Add(transactionAttachment);
                    db.SaveChanges();
                    return RedirectToAction("Dashboard", "Transaction", new { id = transactionAttachment.TransactionId });
                }

            }

            //ViewBag.TicketId = new SelectList(db.Tickets, "Id", "SubmitterId", ticketAttahment.TicketId);
            //ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketAttahment.UserId);
            TempData["Error"] = "Model Invalid";
            return RedirectToAction("Dashboard", "Transaction", new { id = transactionAttachment.TransactionId });

        }

        // GET: TransactionAttachments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TransactionAttachment transactionAttachment = db.TransactionAttachments.Find(id);
            if (transactionAttachment == null)
            {
                return HttpNotFound();
            }
            return View(transactionAttachment);
        }

        // POST: TransactionAttachments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TransactionAttachment transactionAttachment = db.TransactionAttachments.Find(id);
            db.TransactionAttachments.Remove(transactionAttachment);
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
