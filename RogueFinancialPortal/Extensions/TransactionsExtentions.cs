using RogueFinancialPortal.Enums;
using RogueFinancialPortal.Helpers;
using RogueFinancialPortal.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

using System.Web;
using System.Web.Mvc;

using System.Web.Configuration;

namespace RogueFinancialPortal.Extensions
{
    public static class TransactionsExtentions
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        public static void UpdateBalance(this Transaction transaction)
        {
          
                UpdateBankBalance(transaction);
                UpdateBudgetAmount(transaction);
                UpdateBudgetItemAmount(transaction);  
                db.SaveChanges();
            
        }
        public static void UpdateBankBalance(Transaction transaction)

        {
            var bankAccount = db.BankAccounts.Find(transaction.BankAccountId);

            if (transaction.TransactionType == TransactionType.Withdrawal)
            {
                bankAccount.CurrentBalance -= transaction.Amount;
            }
            else if (transaction.TransactionType == TransactionType.Deposit || transaction.TransactionType == TransactionType.Refund)
            {
                bankAccount.CurrentBalance += transaction.Amount;

            }
        }
        public static void UpdateBudgetAmount(Transaction transaction)
        {
            var budgetItem = db.BudgetItems.Find(transaction.BudgetItemId);
            var budget = db.Budgets.Find(budgetItem.BudgetId);
            if (transaction.TransactionType == TransactionType.Withdrawal)
            {
                budget.CurrentAmount += transaction.Amount;
            }
            else if (transaction.TransactionType == TransactionType.Refund)
            {
                budget.CurrentAmount -= transaction.Amount;

            }
            
          
        }
        public static void UpdateBudgetItemAmount(Transaction transaction)
        {
            var budgetItem = db.BudgetItems.Find(transaction.BudgetItemId);
            if (transaction.TransactionType == TransactionType.Withdrawal)
            {
                budgetItem.CurrentAmount += transaction.Amount;
            }
            else if (transaction.TransactionType == TransactionType.Refund)
            {
                budgetItem.CurrentAmount -= transaction.Amount;

            }
           
           
        }

        public static void EditTransaction(this Transaction transaction)
        {
            //var oldTranaction = db.Transactions.AsNoTracking().FirstOrDefault(t=>t.Id==transaction.Id);
            transaction.Amount *= -1;
            UpdateBalance(transaction);
            
        }
        public static void DeleteTransaction(this Transaction transaction)
        {
            var mytransaction = db.Transactions.Find(transaction.Id);
            mytransaction.Amount *= -1;
            mytransaction.IsDeleted = true;
            db.SaveChanges();
            UpdateBalance(mytransaction);

        }
       
    }
}