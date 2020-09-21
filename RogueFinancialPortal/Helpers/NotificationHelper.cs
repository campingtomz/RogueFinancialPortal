using Microsoft.AspNet.Identity;
using RogueFinancialPortal.Extensions;
using RogueFinancialPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RogueFinancialPortal.Helpers
{
    public class NotificationHelper
    {
        //trackNotifications for
        // change in transaction
        //bank account or budget/budget itmes reach warrning/target ammount
        private ApplicationDbContext db = new ApplicationDbContext();
        UserHelper userHelper = new UserHelper();
        public void NewTransactionCheck(Transaction newTransaction)
        {
            var bankAccount = db.BankAccounts.Find(newTransaction.BankAccountId);
            var budgetitem = db.BudgetItems.Find(newTransaction.BudgetItemId);
            var budget = db.Budgets.Find(budgetitem.BudgetId);

            BankAccountWarning(bankAccount);
            BudgetTargetCheck(budget);
            BudgetItemTargetCheck(budgetitem);
        }
        public void BankAccountWarning(BankAccount bankAccount)
        {
            foreach (var member in db.Users.Where(u => u.HouseHoldId == bankAccount.HouseHoldId).ToList())
            {
                if (bankAccount.CurrentBalance <= bankAccount.WarningBalance)
                {
                    BankAccountNotification newBankAccountNotification = new BankAccountNotification
                    {
                        HouseHoldId = bankAccount.HouseHoldId,
                        UserId = member.Id,
                        BankAccountId = bankAccount.Id,
                        Created = DateTime.Now,
                        Subject = $"Bank Account {bankAccount.BankAccountName} Limit Warning",
                        Message = $"The Bank Account {bankAccount.BankAccountName} Current Balance is {bankAccount.CurrentBalance}! This is at or below the Warning Balance of {bankAccount.WarningBalance}",
                    };
                    db.Notifications.Add(newBankAccountNotification);
                }
            }
            db.SaveChanges();

        }
        public void BudgetTargetCheck(Budget budget)
        {


            if (budget.CurrentAmount >= Decimal.Multiply(budget.TargetAmount, Convert.ToDecimal(0.90)))
            {
                foreach (var member in db.Users.Where(u => u.HouseHoldId == budget.HouseHoldId).ToList())
                {
                    BudgetNotification newBudgetNotification = new BudgetNotification
                    {
                        HouseHoldId = budget.HouseHoldId,
                        UserId = member.Id,
                        BudgetId = budget.Id,
                        Created = DateTime.Now,
                        Subject = $"Budget {budget.BudgetName} has reached the target Amount",
                        Message = $"The budet {budget.BudgetName} Current Amount is {budget.CurrentAmount}! This at 90% or higher of the Target Amount of {budget.TargetAmount}",
                    };
                    db.Notifications.Add(newBudgetNotification);
                }
            }
            else if (budget.CurrentAmount >= Decimal.Multiply(budget.TargetAmount, Convert.ToDecimal(0.5)))
            {
                foreach (var member in db.Users.Where(u => u.HouseHoldId == budget.HouseHoldId).ToList())
                {
                    BudgetNotification newBudgetNotification = new BudgetNotification
                    {
                        HouseHoldId = budget.HouseHoldId,
                        UserId = member.Id,
                        BudgetId = budget.Id,
                        Created = DateTime.Now,
                        Subject = $"Budget {budget.BudgetName} has reached half way to the target Amount",
                        Message = $"The budet {budget.BudgetName} Current Amount is {budget.CurrentAmount}! This at 50% or higher of the Target Amount of {budget.TargetAmount}",
                    };
                    db.Notifications.Add(newBudgetNotification);
                }

            }
            db.SaveChanges();


        }
        public void BudgetItemTargetCheck(BudgetItem budgetItem)
        {
            var householdId = (int)HttpContext.Current.User.Identity.GetHouseHoldId();

            if (budgetItem.CurrentAmount >= Decimal.Multiply(budgetItem.TargetAmount, Convert.ToDecimal(0.90)))
            {
                foreach (var member in db.Users.Where(u => u.HouseHoldId == householdId).ToList())
                {
                    BudgetItemNotification newBudgetItemNotification = new BudgetItemNotification
                    {
                        HouseHoldId = householdId,
                        UserId = member.Id,
                        BudgetItemId = budgetItem.Id,
                        Created = DateTime.Now,
                        Subject = $"BudgetItem {budgetItem.ItemName} has reached the target Amount",
                        Message = $"The BudgetItem {budgetItem.ItemName} Current Amount is {budgetItem.CurrentAmount}! This at 90% or higher of the Target Amount of {budgetItem.TargetAmount}",
                    };
                    db.Notifications.Add(newBudgetItemNotification);
                }
            }
            else if (budgetItem.CurrentAmount >= Decimal.Multiply(budgetItem.TargetAmount, Convert.ToDecimal(0.5)))
            {
                foreach (var member in db.Users.Where(u => u.HouseHoldId == householdId).ToList())
                {
                    BudgetItemNotification newBudgetItemNotification = new BudgetItemNotification
                    {
                        HouseHoldId = householdId,
                        UserId = member.Id,
                        BudgetItemId = budgetItem.Id,
                        Created = DateTime.Now,
                        Subject = $"BudgetItem {budgetItem.ItemName} has reached half way to the target Amount",
                        Message = $"The BudgetItem {budgetItem.ItemName} Current Amount is {budgetItem.CurrentAmount}! This at 50% or higher of the Target Amount of {budgetItem.TargetAmount}",
                    };
                    db.Notifications.Add(newBudgetItemNotification);
                }
            }

            db.SaveChanges();

        }
        public int NotificationCount()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var count = 0;
            foreach (var notification in db.Notifications.Where(n=>n.UserId == userId))
            {
                if (notification.IsRead != true)
                {
                    count++;
                }
            }
            return count;
        }
        public List<Notification> GetNotifications( )
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            List<Notification> validNotifcations = new List<Notification>();
            foreach (var notification in db.Notifications.Where(n => n.UserId == userId))
            {
                if (notification.IsRead != true)
                {
                    validNotifcations.Add(notification);
                }
            }
            return validNotifcations;
        }
        public void BankAccountDeleted(BankAccount bankAccount)
        {
            foreach (var member in db.Users.Where(u => u.HouseHoldId == bankAccount.HouseHoldId).ToList())
            {
               
                    BankAccountNotification newBankAccountNotification = new BankAccountNotification
                    {
                        HouseHoldId = bankAccount.HouseHoldId,
                        UserId = member.Id,
                        BankAccountId = bankAccount.Id,
                        Created = DateTime.Now,
                        Subject = $"Bank Account {bankAccount.BankAccountName} deleted",
                        Message = $"The Bank Account {bankAccount.BankAccountName} has been delted. All budget, budget items and Tranactions have been deleted",
                    };
                    db.Notifications.Add(newBankAccountNotification);
               
            }
            db.SaveChanges();
        }


    }
}