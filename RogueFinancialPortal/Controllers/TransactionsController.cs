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
using RogueFinancialPortal.Extensions;
using RogueFinancialPortal.Helpers;
using RogueFinancialPortal.Models;

namespace RogueFinancialPortal.Controllers
{
    [Authorize]
    public class TransactionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Transactions

        // GET: Transactions/Create
        public ActionResult Create()
        {
            ViewBag.BudgetItemId = new SelectList(db.BudgetItems, "Id", "OwnerId");
            ViewBag.OwnerId = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,OwnerId,AccountId,BudgetItemId,TransactionType,Created,Amount,Memo,IsDeleted,AccountType,AvatarPath")] Transaction transaction, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                AddAttachment(transaction,file);
                db.Transactions.Add(transaction);
                db.SaveChanges();
                transaction.UpdateBalance();
                return RedirectToAction("Index");
            }

            ViewBag.BudgetItemId = new SelectList(db.BudgetItems, "Id", "OwnerId", transaction.BudgetItemId);
            ViewBag.OwnerId = new SelectList(db.Users, "Id", "FirstName", transaction.OwnerId);
            return View(transaction);
        }

        // GET: Transactions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }

            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,OwnerId,AccountId,BudgetItemId,TransactionType,Created,Amount,Memo,IsDeleted,AccountType,AvatarPath")] Transaction transaction, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                var oldTransaction = db.Transactions.AsNoTracking().FirstOrDefault(t => t.Id == transaction.Id);
                if(file != null)
                {
                    AddAttachment(transaction, file);
                }
                db.Entry(transaction).State = EntityState.Modified;

                db.SaveChanges();
                var newTransaction = db.Transactions.AsNoTracking().FirstOrDefault(t => t.Id == transaction.Id);
                //newTransaction.EditTransaction(oldTransaction);

                return RedirectToAction("Index");
            }

            return View(transaction);
        }
        public  void AddAttachment( Transaction transaction, HttpPostedFileBase file)
        {
            if (FileUploadValidator.IsWebFriendlyImage(file) || FileUploadValidator.IsWebFriendlyFile(file))
            {
                var fileName = FileStamp.MakeUnique(file.FileName);
                var serverFolder = WebConfigurationManager.AppSettings["DefaultAttachmentFolder"];
                file.SaveAs(Path.Combine(Server.MapPath(serverFolder), fileName));
                transaction.FilePath = $"{serverFolder}{fileName}";
                db.SaveChanges();
            }
        }
        // GET: Transactions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Transaction transaction = db.Transactions.Find(id);
            transaction.DeleteTransaction();
            db.Transactions.Remove(transaction);
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
