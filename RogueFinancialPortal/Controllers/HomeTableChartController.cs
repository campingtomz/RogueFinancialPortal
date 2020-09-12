using Microsoft.AspNet.Identity;
using RogueFinancialPortal.Enums;
using RogueFinancialPortal.Extensions;
using RogueFinancialPortal.Helpers;
using RogueFinancialPortal.Models;
using RogueFinancialPortal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace RogueFinancialPortal.Controllers
{
    public class HomeTableChartController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        HomeTableChartHelper homeTableChartHelper = new HomeTableChartHelper();
        public JsonResult LoadHouseHold()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
           var householdId = (int)User.Identity.GetHouseHoldId();
            HomeHouseHoldVM newHouseHoldVM = new HomeHouseHoldVM
            {
                HouseHoldName = user.HouseHoldName,
                OwnerName = user.FullName,

            };
            newHouseHoldVM.BankAccounts = homeTableChartHelper.GetHouseHoldAccounts(householdId);
            newHouseHoldVM.Budgets = homeTableChartHelper.GetHouseHoldBudgets(householdId);
            newHouseHoldVM.BudgetItems = homeTableChartHelper.GetHouseHoldBudgetItems(householdId);
            newHouseHoldVM.Transactions = homeTableChartHelper.GetHouseHoldTransactions(householdId);
            newHouseHoldVM.Members = homeTableChartHelper.GetHouseHoldMembers(householdId);



            return Json(newHouseHoldVM);
        }
        public JsonResult AddTransaction(HomeTransactionsVM newTransaction) 
        {
            var householdId = (int)User.Identity.GetHouseHoldId();

            var budgetItem = db.BudgetItems.Find(newTransaction.BudgetItemId);
            Transaction tranasction = new Transaction
            {
                Amount = newTransaction.Amount,
                Memo = newTransaction.Memo,
                BudgetItemId = newTransaction.BudgetItemId,
                Created = DateTime.Now,
                OwnerId = User.Identity.GetUserId(),
                AccountId = budgetItem.Budget.BankAccontId,
                TransactionType = newTransaction.TransactionType
            };
            db.Transactions.Add(tranasction);

            db.SaveChanges();
            tranasction.UpdateBalance();
            return Json(homeTableChartHelper.GetHouseHoldTransactions(householdId));
        }
        public JsonResult deleteTransaction(int transactionId)
        {
            var householdId = (int)User.Identity.GetHouseHoldId();    
            var transaction = db.Transactions.Find(transactionId);
            if (!transaction.IsDeleted) {
                transaction.DeleteTransaction();
            }
            return Json(homeTableChartHelper.GetHouseHoldTransactions(householdId));
        }
        public JsonResult editTransaction(int transactionId)
        {
            var householdId = (int)User.Identity.GetHouseHoldId();
            var transaction = db.Transactions.Find(transactionId);
            transaction.UpdateBalance();

            return Json(homeTableChartHelper.GetHouseHoldTransactions(householdId));
        }
        public JsonResult AddAccount(HomeBankAccountVM newAccount)
        {
            var householdId = (int)User.Identity.GetHouseHoldId();


            BankAccount bankAccount = new BankAccount(newAccount.StartingBalance, newAccount.WarningBalance, newAccount.BankAccountName);
            bankAccount.AccountType = newAccount.AccountType;
            db.BankAccounts.Add(bankAccount);
            db.SaveChanges();

            return Json(homeTableChartHelper.GetHouseHoldAccounts(householdId));
        }
        public JsonResult AddBudget(HomeBudgetVm newBudget)
        {
            var householdId = (int)User.Identity.GetHouseHoldId();

            var bankAccount = db.BankAccounts.Find(newBudget.BankAccontId);
            Budget budgets = new Budget
            {
                BudgetName = newBudget.BudgetName,
                Description = newBudget.Description,
                BankAccontId = newBudget.BankAccontId,

            };
            db.Budgets.Add(budgets);
            db.SaveChanges();

            return Json(homeTableChartHelper.GetHouseHoldBudgets(householdId));
        }
        public async Task InviteMember(string inviteEmail)
        {
            var householdId = (int)User.Identity.GetHouseHoldId();

            Invitation newInvitation = new Invitation
            {
                HouseHoldId = householdId,
               RecipientEmail = inviteEmail
            };
            newInvitation.Code = Guid.NewGuid();
            db.Invitations.Add(newInvitation);
            db.SaveChanges();
            await newInvitation.SendInvitation();

           
        }

    }
}
