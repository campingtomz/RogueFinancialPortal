using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using RogueFinancialPortal.Models;

namespace RogueFinancialPortal.Helpers
{
    public class TransactionHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRoleHelper userRoleHelper = new UserRoleHelper();

        public List<Transaction> GetUserTransactions(string userId)
        {
            return db.Transactions.Where(t=>t.OwnerId == userId).ToList();
        }
        public List<Transaction> GetBankAccountTransactions(int BankAccountId)
        {
            return db.Transactions.Where(t => t.AccountId == BankAccountId).ToList();
        }
        public List<Transaction> GetBudgetItemTransactions(int BudgetItemId)
        {
            return db.Transactions.Where(t => t.BudgetItemId == BudgetItemId).ToList();
        }
        public bool CanCreateTransaction()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var myRole = userRoleHelper.ListUserRoles(userId).FirstOrDefault();

            switch (myRole)
            {
                case "Head":
                case "Member":
                    return true;
                default:
                    return false;
            }
        }
        public bool CanEditTransaction()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var myRole = userRoleHelper.ListUserRoles(userId).FirstOrDefault();

            switch (myRole)
            {
                case "Head":
                case "Member":
                    return true;
                default:
                    return false;
            }
        }
        public bool CanDeletTransaction()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var myRole = userRoleHelper.ListUserRoles(userId).FirstOrDefault();

            switch (myRole)
            {
                case "Head":
                case "Member":
                    return true;
                default:
                    return false;
            }
        }
    }
}