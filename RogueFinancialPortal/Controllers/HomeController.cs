using RogueFinancialPortal.Enums;
using RogueFinancialPortal.Extensions;
using RogueFinancialPortal.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
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

        public ActionResult Index()
        {
            var householdId = (int)User.Identity.GetHouseHoldId();

            ViewBag.BudgetsList = new SelectList(db.Budgets.Where(b => b.HouseHoldId == householdId), "Id", "BudgetName");           
            ViewBag.AccountList = new SelectList(db.BankAccounts.Where(b => b.HouseHoldId == householdId), "Id", "BankAccountName");
            ViewBag.BudgetsItemsList = new SelectList(db.BudgetItems.Where(b => b.Budget.HouseHoldId == householdId), "Id", "ItemName");


            ViewBag.TransactionType = new SelectList(Enum.GetValues(typeof(TransactionType)), TransactionType.Withdrawal);
            ViewBag.AccountType = new SelectList(Enum.GetValues(typeof(AccountType)), AccountType.Checking);

            return View();
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