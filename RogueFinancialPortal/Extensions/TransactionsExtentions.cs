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
            var bankAccount = db.BankAccounts.Find(transaction.AccountId);

            if (transaction.TransactionType == TransactionType.Withdrawal)
            {
                bankAccount.CurrentBalance -= transaction.Amount;
            }
            else if (transaction.TransactionType == TransactionType.Deposit)
            {
                bankAccount.CurrentBalance += transaction.Amount;

            }
        }
        public static void UpdateBudgetAmount(Transaction transaction)
        {
            var budgetItem = db.BudgetItems.Find(transaction.BudgetItemId);
            var budget = db.Budgets.Find(budgetItem.BudgetId);
            budget.CurrnetAmount += transaction.Amount;
          
        }
        public static void UpdateBudgetItemAmount(Transaction transaction)
        {
            var budgetItem = db.BudgetItems.Find(transaction.BudgetItemId);
            budgetItem.CurrnetAmount += transaction.Amount;
           
        }

        public static void EditTransaction(this Transaction newTransaction, Transaction OldTransaction)
        {
            OldTransaction.DeleteTransaction();
            UpdateBalance(newTransaction);
            db.SaveChanges();
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