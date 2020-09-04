using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using RogueFinancialPortal.Extensions;
using RogueFinancialPortal.Helpers;
using RogueFinancialPortal.Models;
using RogueFinancialPortal.ViewModels;

namespace RogueFinancialPortal.Controllers
{
    public class HouseHoldsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRoleHelper roleHelper = new UserRoleHelper();
        // GET: HouseHolds
        public ActionResult Index()
        {
            var houseHolds = db.HouseHolds.Include(h => h.Owner);
            return View(houseHolds.ToList());
        }

        // GET: HouseHolds/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HouseHold houseHold = db.HouseHolds.Find(id);
            if (houseHold == null)
            {
                return HttpNotFound();
            }
            return View(houseHold);
        }



        //[Authorize(Roles = "Head")]
        [HttpGet]
        public ActionResult HouseHoldWizard()
        {
            //var model = new HouseHoldCreationWizardVM();
            //model.HouseHoldId = (int)User.Identity.GetHouseHoldId();
            //if(model.HouseHoldId == null)
            //{
            //    return RedirectToAction("Create");
            //}
            return View();
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        ////public ActionResult HouseHoldWizard(HouseHoldCreationWizardVM houseHold)
        ////{
        //    foreach (var bankAccount in houseHold.BankAccounts.ToList())
        //    {
        //        var newBankAccount = new BankAccount(bankAccount.StartingBalance, bankAccount.WarningBalance);
        //        db.BankAccounts.Add(newBankAccount);
               
        //    }
        //    foreach(var budget in houseHold.Budgets)
        //    {
        //        var newBudget = new Budget();
        //        db.SaveChanges();
        //        foreach(var budgetItems in budget.BudgetItems)
        //        {
        //            var budgetItem = new BudgetItem();
        //            budgetItem.BudgetId = budgetItems.BudgetId;
        //            budgetItem.TargetAmount = budgetItems.TargetAmount;
        //            db.BudgetItems.Add(budgetItem);

        //        }
        //        db.SaveChanges();
        //    }
        //    return View();

        //}
        // POST: HouseHolds/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598. 
        // GET: HouseHolds/Create
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Default")]

        public async System.Threading.Tasks.Task<ActionResult> Create([Bind(Include = "Id,HouseHoldName,Greeting")] HouseHold houseHold)
        {
            if (ModelState.IsValid)
            {
                houseHold.Created = DateTime.Now;
                db.HouseHolds.Add(houseHold);
                db.SaveChanges();
                var user = db.Users.Find(User.Identity.GetUserId());
                user.HouseHoldId = houseHold.Id;
                roleHelper.UpdateUserRole(user.Id, "Head");
                db.SaveChanges();
                await AuthorizeExtensions.RefreshAuthentication(HttpContext, user);
                return RedirectToAction("HouseHoldWizard");
            }

            return View(houseHold);
        }
        

        // GET: HouseHolds/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HouseHold houseHold = db.HouseHolds.Find(id);
            if (houseHold == null)
            {
                return HttpNotFound();
            }
            ViewBag.OwnerId = new SelectList(db.Users, "Id", "FirstName", houseHold.OwnerId);
            return View(houseHold);
        }

        // POST: HouseHolds/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,OwnerId,HouseHoldName,Greeting,Created,IsDeleted")] HouseHold houseHold)
        {
            if (ModelState.IsValid)
            {
                db.Entry(houseHold).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OwnerId = new SelectList(db.Users, "Id", "FirstName", houseHold.OwnerId);
            return View(houseHold);
        }

        // GET: HouseHolds/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HouseHold houseHold = db.HouseHolds.Find(id);
            if (houseHold == null)
            {
                return HttpNotFound();
            }
            return View(houseHold);
        }

        // POST: HouseHolds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HouseHold houseHold = db.HouseHolds.Find(id);
            db.HouseHolds.Remove(houseHold);
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
