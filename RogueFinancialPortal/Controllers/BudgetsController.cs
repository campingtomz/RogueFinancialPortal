using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using RogueFinancialPortal.Extensions;

using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RogueFinancialPortal.Models;
using RogueFinancialPortal.ViewModels;

namespace RogueFinancialPortal.Controllers
{
    [Authorize]
    public class BudgetsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Budgets
        public ActionResult Index()
        {
            var budgets = db.Budgets.Include(b => b.HouseHold).Include(b => b.Owner);
            return View(budgets.ToList());
        }

        // GET: Budgets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Budget budget = db.Budgets.Find(id);
            if (budget == null)
            {
                return HttpNotFound();
            }
            return View(budget);
        }

        // GET: Budgets/Create
        public ActionResult Create()
        {
            var BankAccounts = db.HouseHolds.Find(User.Identity.GetHouseHoldId()).BankAccounts.ToList();
            if (BankAccounts.Count > 0 || BankAccounts != null)
            {
                ViewBag.AccountName = new SelectList(BankAccounts, "Id", "BankAccountName");
                return View();
            }
            return View();
        }

        // POST: Budgets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BudgetName,Description")] BudgetWizardVM budget, int AccountName)
        {
            if (ModelState.IsValid)
            {
                Budget newBudget = new Budget();
                newBudget.BudgetName = budget.BudgetName;
                newBudget.Description = budget.Description;
                newBudget.BankAccountId = AccountName;

                db.Budgets.Add(newBudget);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var BankAccounts = db.HouseHolds.Find(User.Identity.GetHouseHoldId()).BankAccounts.ToList();
            ViewBag.AccountName = new SelectList(BankAccounts, "Id", "BankAccountName");

            return View(budget);
        }

        // GET: Budgets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Budget budget = db.Budgets.Find(id);
            if (budget == null)
            {
                return HttpNotFound();
            }
            ViewBag.HouseHoldId = new SelectList(db.HouseHolds, "Id", "OwnerId", budget.HouseHoldId);
            ViewBag.OwnerId = new SelectList(db.Users, "Id", "FirstName", budget.OwnerId);
            return View(budget);
        }

        // POST: Budgets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,HouseHoldId,OwnerId,BudgetName,Created,CurrentAmount")] Budget budget)
        {
            if (ModelState.IsValid)
            {
                db.Entry(budget).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HouseHoldId = new SelectList(db.HouseHolds, "Id", "OwnerId", budget.HouseHoldId);
            ViewBag.OwnerId = new SelectList(db.Users, "Id", "FirstName", budget.OwnerId);
            return View(budget);
        }

        // GET: Budgets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Budget budget = db.Budgets.Find(id);
            if (budget == null)
            {
                return HttpNotFound();
            }
            return View(budget);
        }

        // POST: Budgets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Budget budget = db.Budgets.Find(id);
            db.Budgets.Remove(budget);
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
