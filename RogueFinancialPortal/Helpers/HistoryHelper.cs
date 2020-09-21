using Microsoft.AspNet.Identity;
using RogueFinancialPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RogueFinancialPortal.Helpers
{
    public class HistoryHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public void TranactionEdit(Transaction oldTranaction, Transaction newTranaction)
        {
            CheckAmount(oldTranaction, newTranaction);
            CheckMemo(oldTranaction, newTranaction);
            CheckTransactionType(oldTranaction, newTranaction);
            CheckBudgetItemId(oldTranaction, newTranaction);
            CheckBankAccountId(oldTranaction, newTranaction);
            db.SaveChanges();
        }
        private void CheckAmount(Transaction oldTranaction, Transaction newTranaction)
        {
            if (oldTranaction.Amount != newTranaction.Amount)
            {
                TransactionHistory NewHistory = new TransactionHistory()
                {
                    HistoryType = "Transaction",
                    TransactionId = newTranaction.Id,
                    User = db.Users.Find(HttpContext.Current.User.Identity.GetUserId()),
                    ChangedOn = DateTime.Now,
                    Property = "Amount",
                    OldValue = $"{oldTranaction.Amount}",
                    NewValue = $"{newTranaction.Amount}",
                };
                db.Histories.Add(NewHistory);
            }
        }
        private void CheckMemo(Transaction oldTranaction, Transaction newTranaction)
        {
            if (oldTranaction.Memo != newTranaction.Memo)
            {
                TransactionHistory NewHistory = new TransactionHistory()
                {
                    HistoryType = "Transaction",
                    TransactionId = newTranaction.Id,
                    User = db.Users.Find(HttpContext.Current.User.Identity.GetUserId()),
                    ChangedOn = DateTime.Now,
                    Property = "Memo",
                    OldValue = $"{oldTranaction.Memo}",
                    NewValue = $"{newTranaction.Memo}",
                };
                db.Histories.Add(NewHistory);
            }
        }
        private void CheckTransactionType(Transaction oldTranaction, Transaction newTranaction)
        {
            if (oldTranaction.TransactionType != newTranaction.TransactionType)
            {
                TransactionHistory NewHistory = new TransactionHistory()
                {
                    HistoryType = "Transaction",
                    TransactionId = newTranaction.Id,
                    User = db.Users.Find(HttpContext.Current.User.Identity.GetUserId()),
                    ChangedOn = DateTime.Now,
                    Property = "TransactionType",
                    OldValue = $"{oldTranaction.TransactionType}",
                    NewValue = $"{newTranaction.TransactionType}",
                };
                db.Histories.Add(NewHistory);
            }
        }
        private void CheckBudgetItemId(Transaction oldTranaction, Transaction newTranaction)
        {
            if (oldTranaction.BudgetItemId != newTranaction.BudgetItemId)
            {
                TransactionHistory NewHistory = new TransactionHistory()
                {
                    HistoryType = "Transaction",
                    TransactionId = newTranaction.Id,
                    User = db.Users.Find(HttpContext.Current.User.Identity.GetUserId()),
                    ChangedOn = DateTime.Now,
                    Property = "BudgetItemId",
                    OldValue = $"{oldTranaction.BudgetItemId}",
                    NewValue = $"{newTranaction.BudgetItemId}",
                };
                db.Histories.Add(NewHistory);
            }
        }
        private void CheckBankAccountId(Transaction oldTranaction, Transaction newTranaction)
        {
            if (oldTranaction.BankAccountId != newTranaction.BankAccountId)
            {
                TransactionHistory NewHistory = new TransactionHistory()
                {
                    HistoryType = "Transaction",
                    TransactionId = newTranaction.Id,
                    User = db.Users.Find(HttpContext.Current.User.Identity.GetUserId()),
                    ChangedOn = DateTime.Now,
                    Property = "BankAccountId",
                    OldValue = $"{oldTranaction.BankAccountId}",
                    NewValue = $"{newTranaction.BankAccountId}",
                };
                db.Histories.Add(NewHistory);
            }
        }
        public void TranactionIsdeleted(Transaction tranaction)
        {
            
                TransactionHistory NewHistory = new TransactionHistory()
                {
                    HistoryType = "Transaction",
                    TransactionId = tranaction.Id,
                    User = db.Users.Find(HttpContext.Current.User.Identity.GetUserId()),
                    ChangedOn = DateTime.Now,
                    Property = "IsDeleted",
                    OldValue = "false",
                    NewValue = $"{tranaction.IsDeleted}",
                };
                db.Histories.Add(NewHistory);
            
            db.SaveChanges();
        }


        public void BudgetItemEdit(BudgetItem oldBudgetItem, BudgetItem newBudgetItem)
        {
            CheckBudgetItemItemName(oldBudgetItem, newBudgetItem);
            CheckBudgetItemDescription(oldBudgetItem, newBudgetItem);
            CheckBudgetItemBudgetId(oldBudgetItem, newBudgetItem);
            CheckBudgetItemTargetAmount(oldBudgetItem, newBudgetItem);

            db.SaveChanges();
        }
        private void CheckBudgetItemTargetAmount(BudgetItem oldBudgetItem, BudgetItem newBudgetItem)
        {
            if (oldBudgetItem.TargetAmount != newBudgetItem.TargetAmount)
            {
                BudgetItemHistory NewHistory = new BudgetItemHistory()
                {
                    HistoryType = "Budget Item",
                    BudgetItemId = newBudgetItem.Id,
                    User = db.Users.Find(HttpContext.Current.User.Identity.GetUserId()),
                    ChangedOn = DateTime.Now,
                    Property = "TargetAmount",
                    OldValue = $"{oldBudgetItem.TargetAmount}",
                    NewValue = $"{newBudgetItem.TargetAmount}",
                };
                db.Histories.Add(NewHistory);
            }
        }
        private void CheckBudgetItemItemName(BudgetItem oldBudgetItem, BudgetItem newBudgetItem)
        {
            if (oldBudgetItem.ItemName != newBudgetItem.ItemName)
            {
                BudgetItemHistory NewHistory = new BudgetItemHistory()
                {
                    HistoryType = "Budget Item",
                    BudgetItemId = newBudgetItem.Id,
                    User = db.Users.Find(HttpContext.Current.User.Identity.GetUserId()),
                    ChangedOn = DateTime.Now,
                    Property = "ItemName",
                    OldValue = $"{oldBudgetItem.ItemName}",
                    NewValue = $"{newBudgetItem.ItemName}",
                };
                db.Histories.Add(NewHistory);
            }
        }
        private void CheckBudgetItemDescription(BudgetItem oldBudgetItem, BudgetItem newBudgetItem)
        {
            if (oldBudgetItem.Description != newBudgetItem.Description)
            {
                BudgetItemHistory NewHistory = new BudgetItemHistory()
                {
                    HistoryType = "Budget Item",
                    BudgetItemId = newBudgetItem.Id,
                    User = db.Users.Find(HttpContext.Current.User.Identity.GetUserId()),
                    ChangedOn = DateTime.Now,
                    Property = "Description",
                    OldValue = $"{oldBudgetItem.Description}",
                    NewValue = $"{newBudgetItem.Description}",
                };
                db.Histories.Add(NewHistory);
            }
        }
        private void CheckBudgetItemBudgetId(BudgetItem oldBudgetItem, BudgetItem newBudgetItem)
        {
            if (oldBudgetItem.BudgetId != newBudgetItem.BudgetId)
            {
                BudgetItemHistory NewHistory = new BudgetItemHistory()
                {
                    HistoryType = "Budget Item",
                    BudgetItemId = newBudgetItem.Id,
                    User = db.Users.Find(HttpContext.Current.User.Identity.GetUserId()),
                    ChangedOn = DateTime.Now,
                    Property = "BudgetId",
                    OldValue = $"{oldBudgetItem.BudgetId}",
                    NewValue = $"{newBudgetItem.BudgetId}",
                };
                db.Histories.Add(NewHistory);
            }
        }
        public void BudgetItemIsDeleted(BudgetItem budgetItem)
        {
           
                BudgetItemHistory NewHistory = new BudgetItemHistory()
                {
                    HistoryType = "Budget Item",
                    BudgetItemId = budgetItem.Id,
                    User = db.Users.Find(HttpContext.Current.User.Identity.GetUserId()),
                    ChangedOn = DateTime.Now,
                    Property = "IsDeleted",
                    OldValue = "false",
                    NewValue = $"{budgetItem.IsDeleted}",
                };
                db.Histories.Add(NewHistory);
            
            db.SaveChanges();
        }

        public void BudgetEdit(Budget oldBudget, Budget newBudget)
        {
            CheckBudgetName(oldBudget, newBudget);
            CheckBudgetDescription(oldBudget, newBudget);
            CheckBudgetBankAccountId(oldBudget, newBudget);

            db.SaveChanges();
        }
        private void CheckBudgetName(Budget oldBudget, Budget newBudget)
        {
            if (oldBudget.TargetAmount != newBudget.TargetAmount)
            {
                BudgetHistory NewHistory = new BudgetHistory()
                {
                    HistoryType = "Budget",
                    BudgetId = newBudget.Id,
                    User = db.Users.Find(HttpContext.Current.User.Identity.GetUserId()),
                    ChangedOn = DateTime.Now,
                    Property = "BudgetName",
                    OldValue = $"{oldBudget.BudgetName}",
                    NewValue = $"{newBudget.BudgetName}",
                };
                db.Histories.Add(NewHistory);
            }
        }
        private void CheckBudgetDescription(Budget oldBudget, Budget newBudget)
        {
            if (oldBudget.Description != newBudget.Description)
            {
                BudgetHistory NewHistory = new BudgetHistory()
                {
                    HistoryType = "Budget",
                    BudgetId = newBudget.Id,
                    User = db.Users.Find(HttpContext.Current.User.Identity.GetUserId()),
                    ChangedOn = DateTime.Now,
                    Property = "Description",
                    OldValue = $"{oldBudget.Description}",
                    NewValue = $"{newBudget.Description}",
                };
                db.Histories.Add(NewHistory);
            }
        }
        private void CheckBudgetBankAccountId(Budget oldBudget, Budget newBudget)
        {
            if (oldBudget.BankAccountId != newBudget.BankAccountId)
            {
                BudgetHistory NewHistory = new BudgetHistory()
                {
                    HistoryType = "Budget",
                    BudgetId = newBudget.Id,
                    User = db.Users.Find(HttpContext.Current.User.Identity.GetUserId()),
                    ChangedOn = DateTime.Now,
                    Property = "BankAccountId",
                    OldValue = $"{oldBudget.BankAccountId}",
                    NewValue = $"{newBudget.BankAccountId}",
                };
                db.Histories.Add(NewHistory);
            }
        }
        public void CheckBudgetIsDeleted(Budget budget)
        {
            
                BudgetHistory NewHistory = new BudgetHistory()
                {
                    HistoryType = "Budget",
                    BudgetId = budget.Id,
                    User = db.Users.Find(HttpContext.Current.User.Identity.GetUserId()),
                    ChangedOn = DateTime.Now,
                    Property = "IsDeleted",
                    OldValue = "false",
                    NewValue = $"{budget.IsDeleted}",
                };
                db.Histories.Add(NewHistory);
                db.SaveChanges();
            
        }


        public void BankAccountEdit(BankAccount oldBankAccount, BankAccount newBankAccount)
        {
            CheckBankAccountName(oldBankAccount, newBankAccount);
            CheckWarningBalance(oldBankAccount, newBankAccount);
            CheckAccountType(oldBankAccount, newBankAccount);
            db.SaveChanges();
        }
        private void CheckBankAccountName(BankAccount oldBankAccount, BankAccount newBankAccount)
        {
            if (oldBankAccount.BankAccountName != newBankAccount.BankAccountName)
            {
                BudgetHistory NewHistory = new BudgetHistory()
                {
                    HistoryType = "Bank Account",
                    BudgetId = newBankAccount.Id,
                    User = db.Users.Find(HttpContext.Current.User.Identity.GetUserId()),
                    ChangedOn = DateTime.Now,
                    Property = "BankAccountName",
                    OldValue = $"{oldBankAccount.BankAccountName}",
                    NewValue = $"{newBankAccount.BankAccountName}",
                };
                db.Histories.Add(NewHistory);
            }
        }
        private void CheckWarningBalance(BankAccount oldBankAccount, BankAccount newBankAccount)
        {
            if (oldBankAccount.WarningBalance != newBankAccount.WarningBalance)
            {
                BudgetHistory NewHistory = new BudgetHistory()
                {
                    HistoryType = "Bank Account",
                    BudgetId = newBankAccount.Id,
                    User = db.Users.Find(HttpContext.Current.User.Identity.GetUserId()),
                    ChangedOn = DateTime.Now,
                    Property = "WarningBalance",
                    OldValue = $"{oldBankAccount.WarningBalance}",
                    NewValue = $"{newBankAccount.WarningBalance}",
                };
                db.Histories.Add(NewHistory);
            }
        }
        private void CheckAccountType(BankAccount oldBankAccount, BankAccount newBankAccount)
        {
            if (oldBankAccount.AccountType != newBankAccount.AccountType)
            {
                BudgetHistory NewHistory = new BudgetHistory()
                {
                    HistoryType = "Bank Account",
                    BudgetId = newBankAccount.Id,
                    User = db.Users.Find(HttpContext.Current.User.Identity.GetUserId()),
                    ChangedOn = DateTime.Now,
                    Property = "AccountType",
                    OldValue = $"{oldBankAccount.AccountType}",
                    NewValue = $"{newBankAccount.AccountType}",
                };
                db.Histories.Add(NewHistory);
            }
        }
        public void BankAccountIsDeleted(BankAccount bankAccountt)
        {
            
                BudgetHistory NewHistory = new BudgetHistory()
                {
                    HistoryType = "Bank Account",
                    BudgetId = bankAccountt.Id,
                    User = db.Users.Find(HttpContext.Current.User.Identity.GetUserId()),
                    ChangedOn = DateTime.Now,
                    Property = "IsDeleted",
                    OldValue = "false",
                    NewValue = $"{bankAccountt.IsDeleted}",
                };
                db.Histories.Add(NewHistory);
           
            db.SaveChanges();
         }


    }
}