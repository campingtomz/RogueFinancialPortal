using Microsoft.AspNet.Identity;
using RogueFinancialPortal.Enums;
using RogueFinancialPortal.Extensions;
using RogueFinancialPortal.Helpers;
using RogueFinancialPortal.Models;
using RogueFinancialPortal.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace RogueFinancialPortal.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        UserHelper userHelper = new UserHelper();
        UserRoleHelper roleHelper = new UserRoleHelper();
        public ActionResult Index()
        {
            var user = userHelper.getUser(User.Identity.GetUserId());
            if (roleHelper.ListUserRoles(user.Id).FirstOrDefault() == "Default") {
                return RedirectToAction("DefaultIndex");
            }
            else
            {
                var householdId = (int)User.Identity.GetHouseHoldId();

                ViewBag.BudgetsList = new SelectList(db.Budgets.Where(b => b.HouseHoldId == householdId).Where(b => b.IsDeleted == false), "Id", "BudgetName");
                ViewBag.AccountList = new SelectList(db.BankAccounts.Where(b => b.HouseHoldId == householdId).Where(b => b.IsDeleted == false), "Id", "BankAccountName");
                ViewBag.BudgetsItemsList = new SelectList(db.BudgetItems.Where(b => b.Budget.HouseHoldId == householdId).Where(b => b.IsDeleted == false), "Id", "ItemName");


                ViewBag.TransactionType = new SelectList(Enum.GetValues(typeof(TransactionType)), TransactionType.Withdrawal);
                ViewBag.AccountType = new SelectList(Enum.GetValues(typeof(AccountType)), AccountType.Checking);

                return View();
            }
        }
        public ActionResult DefaultIndex()
        {
            var householdId = (int)User.Identity.GetHouseHoldId();
            var model = new List<InvitationVM>();
            var userEmail = userHelper.GetEmail(User.Identity.GetUserId());
            foreach (var invitation in db.Invitations.Where(u => u.RecipientEmail == userEmail).Where(u=>u.IsValid == true).ToList())
            {
                InvitationVM invitationVM = new InvitationVM();
                invitationVM.Id = invitation.Id;
                invitationVM.HouseHoldName = db.HouseHolds.Find(invitation.HouseHoldId).HouseHoldName;
                invitationVM.Code = invitation.Code;
                model.Add(invitationVM);
            }

            return View(model);
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Contact()
        {
            EmailModel model = new EmailModel();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(EmailModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var body = "<p>Email From: <bold>{0}</bold> ({1})</p><p> Message:</p><p>{ 2}</p> ";
                var from = "MyPortfolio<example@email.com>";
                    model.Body = "This is a message from your portfolio site. The name and the email of the contacting person is above.";
                var email = new MailMessage(from, ConfigurationManager.AppSettings["emailto"])
                {
                    Subject = "Rogue Financial Portal Contact Email",
                    Body = string.Format(body, model.FromName, model.FromEmail,
                model.Body),
                    IsBodyHtml = true
                };
                    var svc = new EmailService();
                    await svc.SendAsync(email);
                    return View(new EmailModel());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await Task.FromResult(0);
                }
            }
            return View(model);
        }
    }
}