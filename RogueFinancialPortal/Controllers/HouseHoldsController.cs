using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using RogueFinancialPortal.Extensions;
using RogueFinancialPortal.Helpers;
using RogueFinancialPortal.Models;
using RogueFinancialPortal.ViewModels;

namespace RogueFinancialPortal.Controllers
{
    [Authorize]
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



        [Authorize(Roles = "Head")]
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
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<ActionResult>  HouseHoldWizard(List<BankAccountWizardVM> BankAccounts, string Greeting, string HouseHoldName)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            //if (user.HouseHoldId == null)
            //{
                HouseHold newHouseHold = new HouseHold(HouseHoldName, Greeting);
                db.HouseHolds.Add(newHouseHold);
                db.SaveChanges();

                user.HouseHoldId = newHouseHold.Id;
                roleHelper.UpdateUserRole(user.Id, "Head");
                db.SaveChanges();
                await AuthorizeExtensions.RefreshAuthentication(HttpContext, user);

                if (BankAccounts != null)
                {
                    foreach (var bankAccount in BankAccounts)
                    {
                        BankAccount newBankAccount = new BankAccount(bankAccount.StartingBalance, bankAccount.WarningBalance, bankAccount.BankAccountName);
                        db.BankAccounts.Add(newBankAccount);
                        db.SaveChanges();
                        if (bankAccount.Budgets != null)
                        {
                            foreach (var budget in bankAccount.Budgets)
                            {
                                Budget newBudget = new Budget();
                                newBudget.BudgetName = budget.BudgetName;
                                newBudget.Description = budget.Description;
                                newBudget.BankAccountId = newBankAccount.Id;

                                db.Budgets.Add(newBudget);
                                db.SaveChanges();

                                if (budget.BudgetItems != null)
                                {
                                    foreach (var budgetItem in budget.BudgetItems)
                                    {
                                        BudgetItem newBudgetItem = new BudgetItem();
                                        newBudgetItem.ItemName = budgetItem.ItemName;
                                        newBudgetItem.Description = budgetItem.Description;
                                        newBudgetItem.TargetAmount = budgetItem.TargetAmount;
                                        newBudgetItem.BudgetId = newBudget.Id;


                                        db.BudgetItems.Add(newBudgetItem);
                                        db.SaveChanges();
                                    }
                                }
                            }
                        }

                    //}
                }
            }

            return RedirectToAction("Index", "Home");

        }
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

        public ActionResult LeaveAsync()
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var role = roleHelper.ListUserRoles(userId).FirstOrDefault();
            switch (role)
            {
                case "Head":
                    var memberCount = db.Users.Where(u => u.HouseHoldId == user.HouseHoldId).Count() - 1;
                    if (memberCount >= 1)
                    {
                        TempData["Message"] = $"You are unable to leave the Household! There are still <b>{memberCount}<b> other members in the household, you must select one to replace you as head of Household";
                        ViewBag.Members = new SelectList(db.Users.Where(u => u.HouseHoldId == user.HouseHoldId).ToList(), "Id", "FullName");
                        ViewBag.Ishead = true;
                    }
                    else
                    {
                        TempData["Message"] = $"You are Leaving the HouseHold. The HouseHold and all data will be Delete! Are you sure you want to leave?";
                        ViewBag.Ishead = false;
                    }
                    return View();
                case "Member":
                    TempData["Message"] = $"You are Leaving the HouseHold. Are you sure you want to leave?";
                    ViewBag.Ishead = false;
                    return View();

                default:
                    return RedirectToAction("index", "Home");

            }
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LeaveAsync(string newHeadUserId)
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var household = db.HouseHolds.Find(user.HouseHoldId);
            var role = roleHelper.ListUserRoles(userId).FirstOrDefault();
            switch (role)
            {
                case "Head":
                    if (newHeadUserId != null)
                    {
                        household.OwnerId = newHeadUserId;
                        roleHelper.UpdateUserRole(newHeadUserId, "Head");
                        var NewUserHead = db.Users.Find(newHeadUserId);
                        await AuthorizeExtensions.RefreshAuthentication(HttpContext, NewUserHead);
                    }
                    else
                    {
                        household.IsDeleted = true;
                        user.HouseHoldId = null;
                    }

                    //var household = db.HouseHolds.Find(user.HouseHoldId);
                    //db.HouseHolds.Remove(household);
                    db.SaveChanges();

                    roleHelper.UpdateUserRole(userId, "Default");
                    await AuthorizeExtensions.RefreshAuthentication(HttpContext, user);
                    return RedirectToAction("Index", "Home");

                case "Member":
                    user.HouseHoldId = null;

                    db.SaveChanges();
                    roleHelper.UpdateUserRole(userId, "Default");
                    await AuthorizeExtensions.RefreshAuthentication(HttpContext, user);
                    return RedirectToAction("Index", "Home");
                default:
                    return RedirectToAction("index", "Home");


            }
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
