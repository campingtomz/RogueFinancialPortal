using Microsoft.AspNet.Identity;
using RogueFinancialPortal.Enums;
using RogueFinancialPortal.Extensions;
using RogueFinancialPortal.Helpers;
using RogueFinancialPortal.Models;
using RogueFinancialPortal.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        NotificationHelper notificationHelper = new NotificationHelper();
        HistoryHelper historyHelper = new HistoryHelper();

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
        public JsonResult AddTransaction(HomeTransactionsVM Transaction)
        {
            var householdId = (int)User.Identity.GetHouseHoldId();

            var budgetItem = db.BudgetItems.Find(Transaction.BudgetItemId);
            Transaction tranasction = new Transaction
            {
                Amount = Transaction.Amount,
                Memo = Transaction.Memo,
                BudgetItemId = Transaction.BudgetItemId,
                Created = DateTime.Now,
                OwnerId = User.Identity.GetUserId(),
                BankAccontId = budgetItem.Budget.BankAccontId,
                TransactionType = Transaction.TransactionType
            };
            db.Transactions.Add(tranasction);

            notificationHelper.NewTransactionCheck(tranasction);
            tranasction.UpdateBalance();
            db.SaveChanges();
            return Json(homeTableChartHelper.GetHouseHoldTransactions(householdId));
        }
       
       
        public JsonResult AddAccount(HomeBankAccountVM newbankAccount)
        {
            var householdId = (int)User.Identity.GetHouseHoldId();


            BankAccount bankAccount = new BankAccount(newbankAccount.StartingBalance, newbankAccount.WarningBalance, newbankAccount.BankAccountName);
            bankAccount.AccountType = newbankAccount.AccountType;
            db.BankAccounts.Add(bankAccount);
            db.SaveChanges();

            return Json(homeTableChartHelper.GetHouseHoldAccounts(householdId));
        }
        public JsonResult AddBudget(HomeBudgetVm newBudget)
        {
            var householdId = (int)User.Identity.GetHouseHoldId();

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
        public JsonResult AddBudgetItem(HomeBudgetItemsVM newBudgetItem)
        {
            var householdId = (int)User.Identity.GetHouseHoldId();

            BudgetItem budgetItem = new BudgetItem();

            budgetItem.ItemName = newBudgetItem.ItemName;
            budgetItem.Description = newBudgetItem.Description;
            budgetItem.TargetAmount = newBudgetItem.TargetAmount;
            budgetItem.BudgetId = newBudgetItem.BudgetId;

            
            db.BudgetItems.Add(budgetItem);
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

        public JsonResult LoadTransactionsByBudgetItem(string budgetItemName)
        {

            List<HomeTransactionsVM> newTransactions = new List<HomeTransactionsVM>();
            foreach (var transaction in db.Transactions.Where(t => t.BudgetItem.ItemName == budgetItemName).ToList())
            {
                if (transaction.IsDeleted == false)
                {
                    newTransactions.Add(homeTableChartHelper.ConvertTransaction(transaction.Id));
                }
            }
            return Json(newTransactions);
        }
        public JsonResult LoadTransactionsByMember(string memberId)
        {

            List<HomeTransactionsVM> newTransactions = new List<HomeTransactionsVM>();
            foreach (var transaction in db.Transactions.Where(t => t.OwnerId == memberId).ToList())
            {
                if (transaction.IsDeleted == false)
                {
                    newTransactions.Add(homeTableChartHelper.ConvertTransaction(transaction.Id));
                }
            }
            return Json(newTransactions);
        }
        public JsonResult LoadTransactionsByAccount(int accountId)
        {

            List<HomeTransactionsVM> newTransactions = new List<HomeTransactionsVM>();
            foreach (var transaction in db.Transactions.Where(t => t.BankAccontId == accountId).ToList())
            {
                if (transaction.IsDeleted == false)
                {
                    newTransactions.Add(homeTableChartHelper.ConvertTransaction(transaction.Id));
                }
            }
            return Json(newTransactions);
        }
        public JsonResult LoadTransactionsByBudget(int budgetId)
        {

            
            return Json(homeTableChartHelper.GetBudgetTransactions(budgetId));
        }
        public JsonResult GetBudget(int budgetId)
        {
            var budget = homeTableChartHelper.ConvertBudget(budgetId);
            return Json(budget);
        }
        public JsonResult GetBankAccount(int bankAccountId)
        {
            var bankAccount = homeTableChartHelper.ConvertBankAccounts(bankAccountId);
            return Json(bankAccount);
        }
        public JsonResult GetBudgetItem(int budgetItemId)
        {
            return Json(homeTableChartHelper.ConvertBudgetItems(budgetItemId));
        }
        public JsonResult GetTransaction(int transactionId)
        {
            var tranaction = homeTableChartHelper.ConvertTransaction(transactionId);
            return Json(tranaction);
        }
        public JsonResult DeleteBankAccount(int bankAccountId)
        {
            homeTableChartHelper.DeleteBankAccount(bankAccountId);
            db.SaveChanges();
            
            return Json(true);
        }
        public JsonResult DeleteBudget(int budgetId)
        {
            homeTableChartHelper.DeleteBudget(budgetId);
            db.SaveChanges();

            return Json(true);
        }
        public JsonResult DeleteBudgetItem(int budgetItemId)
        {
            homeTableChartHelper.DeleteBudgetItem(budgetItemId);
            db.SaveChanges();

            return Json(true);
        }
        public JsonResult DeleteTransaction(int transactionId)
        {
            homeTableChartHelper.DeleteTransaction(transactionId);
            db.SaveChanges();

            var householdId = (int)User.Identity.GetHouseHoldId();

            return Json(homeTableChartHelper.GetHouseHoldTransactions(householdId));
        }
        public JsonResult EditBankAccount(HomeBankAccountVM jsBankAccount)
        {
            var oldBankAccount = db.BankAccounts.AsNoTracking().FirstOrDefault(b => b.Id == jsBankAccount.Id);
            var bankAccount = db.BankAccounts.Find(jsBankAccount.Id);
            bankAccount.BankAccountName = jsBankAccount.BankAccountName;
            bankAccount.AccountType = jsBankAccount.AccountType;
            bankAccount.WarningBalance = jsBankAccount.WarningBalance;
            db.SaveChanges();
            var newBankAccount = db.BankAccounts.AsNoTracking().FirstOrDefault(b => b.Id == jsBankAccount.Id);
            historyHelper.BankAccountEdit(oldBankAccount, newBankAccount);
            return Json(true);

        }
        public JsonResult EditBudget(HomeBudgetVm jsBudget)
        {
            var oldBudget = db.Budgets.AsNoTracking().FirstOrDefault(b => b.Id == jsBudget.Id);

            var budget = db.Budgets.Find(jsBudget.Id);
            budget.BudgetName = jsBudget.BudgetName;
            budget.Description = jsBudget.Description;
            budget.BankAccontId = jsBudget.BankAccontId;
            db.SaveChanges();
            var newBudget = db.Budgets.AsNoTracking().FirstOrDefault(b => b.Id == jsBudget.Id);
            historyHelper.BudgetEdit(oldBudget, newBudget);
            return Json(true);

        }
        public JsonResult EditBudgetItem(HomeBudgetItemsVM jsBudgetItem)
        {
            var oldBudgetItem = db.BudgetItems.AsNoTracking().FirstOrDefault(b => b.Id == jsBudgetItem.Id);

            var budgetItem = db.BudgetItems.Find(jsBudgetItem.Id);
            budgetItem.ItemName = jsBudgetItem.ItemName;
            budgetItem.Description = jsBudgetItem.Description;
            budgetItem.TargetAmount = jsBudgetItem.TargetAmount;
            budgetItem.BudgetId = jsBudgetItem.BudgetId;
            db.SaveChanges();
            var newBudgetItem = db.BudgetItems.AsNoTracking().FirstOrDefault(b => b.Id == jsBudgetItem.Id);
            historyHelper.BudgetItemEdit(oldBudgetItem, newBudgetItem);

            return Json(true);

        }
        public JsonResult EditTransaction(HomeTransactionsVM jsTransaction)
        {
            var oldTransaction = db.Transactions.AsNoTracking().FirstOrDefault(b => b.Id == jsTransaction.Id);

            var transaction = db.Transactions.Find(jsTransaction.Id);
            var budgetItem = db.BudgetItems.Find(jsTransaction.BudgetItemId);
            transaction.EditTransaction();
            transaction.Memo = jsTransaction.Memo;
            transaction.BudgetItemId = jsTransaction.BudgetItemId;
            transaction.BankAccontId = budgetItem.Budget.BankAccontId;

            transaction.TransactionType = jsTransaction.TransactionType;
            transaction.Amount = jsTransaction.Amount;
            db.SaveChanges();
            transaction.UpdateBalance();
            var newTransaction = db.Transactions.AsNoTracking().FirstOrDefault(b => b.Id == jsTransaction.Id);
            historyHelper.TranactionEdit(oldTransaction, newTransaction);

            return Json(true);

        }
    }
}
