using RogueFinancialPortal.Enums;
using RogueFinancialPortal.Extensions;
using RogueFinancialPortal.Models;
using RogueFinancialPortal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RogueFinancialPortal.Helpers
{
    public class HomeTableChartHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        NotificationHelper notificationHelper = new NotificationHelper();
        HistoryHelper historyHelper = new HistoryHelper();
        public List<HomeBankAccountVM> GetHouseHoldAccounts(int houseHoldId)
        {
            var household = db.HouseHolds.Find(houseHoldId);
            List<HomeBankAccountVM> bankAccounts = new List<HomeBankAccountVM>();
            foreach (var bankAccount in household.BankAccounts.ToList())
            {
                if (!bankAccount.IsDeleted)
                {
                    HomeBankAccountVM newBankAccountWizardVM = new HomeBankAccountVM
                    {
                        Id = bankAccount.Id,
                        Created = bankAccount.Created.ToString("MMM dd yyyy"),
                        BankAccountName = bankAccount.BankAccountName,
                        StartingBalance = bankAccount.StartingBalance,
                        CurrentBalance = bankAccount.CurrentBalance,

                        WarningBalance = bankAccount.WarningBalance,
                        AccountType = bankAccount.AccountType

                    };
                    bankAccounts.Add(newBankAccountWizardVM);
                }
            }
            return bankAccounts;
        }
        public List<HomeBudgetVm> GetHouseHoldBudgets(int houseHoldId)
        {
            var household = db.HouseHolds.Find(houseHoldId);
            List<HomeBudgetVm> budgets = new List<HomeBudgetVm>();
            foreach (var budget in household.Budgets.ToList())
            {
                if (!budget.IsDeleted)
                {
                    HomeBudgetVm newBudgets = new HomeBudgetVm
                    {
                        Id = budget.Id,
                        BudgetName = budget.BudgetName,
                        Description = budget.Description,
                        OwnerId = budget.OwnerId,
                        BankAccountId = budget.BankAccountId,
                        Created = budget.Created.ToString("MMM dd yyyy"),
                        CurrentAmount = budget.CurrentAmount,
                        TargetAmount = budget.TargetAmount,
                        BudgetItems = GethomeBudgetItems(budget.Id)

                    };
                    budgets.Add(newBudgets);
                }
            }
            return budgets;
        }
        public List<HomeBudgetItemsVM> GethomeBudgetItems(int budgetId)
        {

            List<HomeBudgetItemsVM> budgetItems = new List<HomeBudgetItemsVM>();


            var householdBudgetItems = db.BudgetItems.Where(bi => bi.BudgetId == budgetId).ToList();
            foreach (var budgetItem in householdBudgetItems)
            {
                if (budgetItem.IsDeleted == false)
                {
                    HomeBudgetItemsVM newBudgetItems = new HomeBudgetItemsVM
                    {
                        Id = budgetItem.Id,

                        ItemName = budgetItem.ItemName,
                        Description = budgetItem.Description,
                        OwnerId = budgetItem.OwnerId,
                        Created = budgetItem.Created.ToString("MMM dd yyyy"),
                        BudgetId = budgetItem.BudgetId,
                        TargetAmount = budgetItem.TargetAmount,
                        CurrentAmount = budgetItem.CurrentAmount
                    };
                    budgetItems.Add(newBudgetItems);
                }

            }
            return budgetItems;
        }
        public List<HomeBudgetItemsVM> GetHouseHoldBudgetItems(int houseHoldId)
        {
            var household = db.HouseHolds.Find(houseHoldId);
            List<HomeBudgetItemsVM> budgetItems = new List<HomeBudgetItemsVM>();
            foreach (var budget in household.Budgets)
            {
                var householdBudgetItems = db.BudgetItems.Where(bi => bi.BudgetId == budget.Id).ToList();
                foreach (var budgetItem in householdBudgetItems)
                {
                    if (budgetItem.IsDeleted == false)
                    {
                        HomeBudgetItemsVM newBudgetItems = new HomeBudgetItemsVM
                        {
                            Id = budget.Id,
                            ItemName = budgetItem.ItemName,
                            Description = budgetItem.Description,
                            OwnerId = budgetItem.OwnerId,
                            Created = budgetItem.Created.ToString("MMM dd yyyy"),
                            BudgetId = budgetItem.BudgetId,
                            TargetAmount = budgetItem.TargetAmount,
                            CurrentAmount = budgetItem.CurrentAmount
                            //Transactions = db.Transactions
                        };
                        budgetItems.Add(newBudgetItems);
                    }
                }
            }
            return budgetItems;
        }
        public List<HomeTransactionsVM> GetBudgetItemTransactions(int budgetItemId)
        {
            List<HomeTransactionsVM> transactions = new List<HomeTransactionsVM>();
            var budgetItemTransactions = db.Transactions.Where(bi => bi.BudgetItemId == budgetItemId).ToList();

            foreach (var transaction in budgetItemTransactions)
            {
                if (transaction.IsDeleted == false)
                {
                    HomeTransactionsVM newTransaction = new HomeTransactionsVM
                    {
                        Id = transaction.Id,
                        BankAccountId = transaction.BankAccountId,
                        BudgetItemId = transaction.BudgetItemId,
                        BudgetItemName = transaction.BudgetItem.ItemName,
                        TransactionType = (TransactionType)transaction.TransactionType,
                        OwnerId = transaction.OwnerId,
                        Created = transaction.Created.ToString("MMM dd yyyy"),
                        Memo = transaction.Memo,
                        FilePath = transaction.FilePath,
                        Amount = transaction.Amount,

                    };
                    transactions.Add(newTransaction);
                }
            }
            return transactions;
        }
        public List<HomeTransactionsVM> GetHouseHoldTransactions(int houseHoldId)
        {
            var household = db.HouseHolds.Find(houseHoldId);
            List<HomeTransactionsVM> transactions = new List<HomeTransactionsVM>();
            foreach (var BankAccount in household.BankAccounts)
            {
                var householdTransactions = db.Transactions.Where(bi => bi.BankAccountId == BankAccount.Id).ToList();
                foreach (var transaction in householdTransactions)
                {
                    if (transaction.IsDeleted == false)
                    {
                        transactions.Add(ConvertTransaction(transaction.Id));
                    }
                }
            }
            return transactions;
        }
        public List<HomeTransactionsVM> GetBudgetTransactions(int budgetId)
        {
            List<HomeTransactionsVM> newTransactions = new List<HomeTransactionsVM>();

            foreach (var transaction in db.Transactions.Where(t => t.BudgetItem.BudgetId == budgetId).ToList())
            {
                if (transaction.IsDeleted == false)
                {
                    newTransactions.Add(ConvertTransaction(transaction.Id));
                }
            }
            return newTransactions;
        }      
        public List<HomeMemberVM> GetHouseHoldMembers(int houseHoldId)
        {

            List<HomeMemberVM> members = new List<HomeMemberVM>();
            foreach (var member in db.Users.Where(u => u.HouseHoldId == houseHoldId).ToList())
            {


                HomeMemberVM newMember = new HomeMemberVM
                {
                    Id = member.Id,
                    FirstName = member.FirstName,
                    LastName = member.LastName,
                    FullName = member.FullName,
                    Descriptiton = member.Description,
                    AvatarPath = member.AvatarPath,


                };
                members.Add(newMember);


            }
            return members;
        } 
        public HomeTransactionsVM ConvertTransaction(int transactionId)
        {
            var transaction = db.Transactions.Find(transactionId);
            HomeTransactionsVM newTransaction = new HomeTransactionsVM
            {
                Id = transaction.Id,
                BankAccountId = transaction.BankAccountId,
                BudgetItemId = transaction.BudgetItemId,
                BudgetItemName = transaction.BudgetItem.ItemName,
                TransactionType = (TransactionType)transaction.TransactionType,
                OwnerId = transaction.OwnerId,
                Created = transaction.Created.ToString("MMM dd yyyy"),
                Memo = transaction.Memo,
                FilePath = transaction.FilePath,
                Amount = transaction.Amount,

            };
            return newTransaction;
        }
        public HomeBudgetItemsVM ConvertBudgetItems(int budgetItemId)
        {

            BudgetItem budgetItem = db.BudgetItems.Find(budgetItemId);
            HomeBudgetItemsVM newBudgetItems = new HomeBudgetItemsVM
            {
                Id = budgetItem.Id,

                ItemName = budgetItem.ItemName,
                Description = budgetItem.Description,
                OwnerId = budgetItem.OwnerId,
                Created = budgetItem.Created.ToString("MMM dd yyyy"),
                BudgetId = budgetItem.BudgetId,
                TargetAmount = budgetItem.TargetAmount,
                CurrentAmount = budgetItem.CurrentAmount
            };

            return newBudgetItems;
        }
        public HomeBudgetVm ConvertBudget(int budgetId)
        {
            Budget budget = db.Budgets.Find(budgetId);

            HomeBudgetVm newBudget = new HomeBudgetVm
            {
                Id = budget.Id,
                BudgetName = budget.BudgetName,
                Description = budget.Description,
                OwnerId = budget.OwnerId,
                BankAccountId = budget.BankAccountId,
                Created = budget.Created.ToString("MMM dd yyyy"),
                CurrentAmount = budget.CurrentAmount,
                TargetAmount = budget.TargetAmount

            };



            return newBudget;
        }
        public HomeBankAccountVM ConvertBankAccounts(int bankAccountId)
        {
            BankAccount bankAccount = db.BankAccounts.Find(bankAccountId);
            
                    HomeBankAccountVM newBankAccountWizardVM = new HomeBankAccountVM
                    {
                        Id = bankAccount.Id,
                        Created = bankAccount.Created.ToString("MMM dd yyyy"),
                        BankAccountName = bankAccount.BankAccountName,
                        StartingBalance = bankAccount.StartingBalance,
                        CurrentBalance = bankAccount.CurrentBalance,

                        WarningBalance = bankAccount.WarningBalance,
                        AccountType = bankAccount.AccountType

                    };
             
            return newBankAccountWizardVM;
        }
        public void DeleteTransaction(int transactionId)
        {
            var transaction = db.Transactions.Find(transactionId);
            if (!transaction.IsDeleted)
            {
                transaction.DeleteTransaction();
            }
            db.SaveChanges();
            historyHelper.TranactionIsdeleted(transaction);
        }
        public void DeleteBudgetItem(int budgetItemId)
        {
            var budgetItem = db.BudgetItems.Find(budgetItemId);
            budgetItem.IsDeleted = true;
            foreach(var transaction in budgetItem.Transactions.ToList())
            {
                DeleteTransaction(transaction.Id);
            }
            db.SaveChanges();
            historyHelper.BudgetItemIsDeleted(budgetItem);

        }
        public void DeleteBudget(int budgetId)
        {
            var budget = db.Budgets.Find(budgetId);
            budget.IsDeleted = true;
            foreach (var budgetItem in budget.Items.ToList())
            {
                DeleteBudgetItem(budgetItem.Id);
            }
            db.SaveChanges();
            historyHelper.CheckBudgetIsDeleted(budget);
        }
        public void DeleteBankAccount(int bankAccountId)
        {
            var bankAccount = db.BankAccounts.Find(bankAccountId);
            bankAccount.IsDeleted = true;
            foreach (var budget in db.Budgets.Where(b=>b.BankAccountId == bankAccountId).ToList())
            {
                DeleteBudget(budget.Id);
            }
            notificationHelper.BankAccountDeleted(bankAccount);
            db.SaveChanges();
            historyHelper.BankAccountIsDeleted(bankAccount);


        }

    }
}