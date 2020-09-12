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

        public List<HomeBankAccountVM> GetHouseHoldAccounts(int houseHoldId)
        {
            var household = db.HouseHolds.Find(houseHoldId);
            List<HomeBankAccountVM> bankAccounts = new List<HomeBankAccountVM>();
            foreach (var bankAccount in household.BankAccounts.ToList())
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
            return bankAccounts;
        }
        public List<HomeBudgetVm> GetHouseHoldBudgets(int houseHoldId)
        {
            var household = db.HouseHolds.Find(houseHoldId);
            List<HomeBudgetVm> budgets = new List<HomeBudgetVm>();
            foreach (var budget in household.Budgets.ToList())
            {
                HomeBudgetVm newBudgets = new HomeBudgetVm
                {
                    Id = budget.Id,
                    BudgetName = budget.BudgetName,
                    Description = budget.Description,
                    OwnerId = budget.OwnerId,
                    BankAccontId = budget.BankAccontId,
                    Created = budget.Created.ToString("MMM dd yyyy"),
                    CurrnetAmount = budget.CurrnetAmount,
                    TargetAmount = budget.TargetAmount,
                    //BudgetItems = GethomeBudgetItems(budget.Id)

                };
                budgets.Add(newBudgets);
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
                            BudgetItemId = budgetItem.BudgetId,
                            OwnerId = budgetItem.OwnerId,
                            Created = budgetItem.Created.ToString("MMM dd yyyy"),
                            BudgetId = budgetItem.BudgetId,
                            TargetAmount = budgetItem.TargetAmount,
                            CurrnetAmount = budgetItem.CurrnetAmount
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
                            CurrnetAmount = budgetItem.CurrnetAmount
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
            var budgetItemTransactions = db.Transactions.Where(bi =>bi.BudgetItemId  == budgetItemId).ToList();

            foreach (var transaction in budgetItemTransactions)
            {
                if (transaction.IsDeleted == false)
                {
                    HomeTransactionsVM newTransaction = new HomeTransactionsVM
                    {
                        Id = transaction.Id,
                        AccountId = transaction.AccountId,
                        BudgetItemId = transaction.BudgetItemId,
                        BudgetItemName = transaction.BudgetItem.ItemName,
                        TransactionType = transaction.TransactionType,
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
                var householdTransactions = db.Transactions.Where(bi => bi.AccountId == BankAccount.Id).ToList();
                foreach (var transaction in householdTransactions)
                {
                    if (transaction.IsDeleted == false)
                    {
                        HomeTransactionsVM newTransaction = new HomeTransactionsVM
                        {
                            Id = transaction.Id,
                            AccountId = transaction.AccountId,
                            BudgetItemId = transaction.BudgetItemId,
                            BudgetItemName = transaction.BudgetItem.ItemName,
                            TransactionType=  transaction.TransactionType,
                            OwnerId = transaction.OwnerId,
                            Created = transaction.Created.ToString("MMM dd yyyy"),
                            Memo = transaction.Memo,
                            FilePath = transaction.FilePath,
                            Amount = transaction.Amount,

                        };
                        transactions.Add(newTransaction);
                    }
                }
            }
            return transactions;
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
                    Descriptiton = member.Descriptiton,
                    AvatarPath = member.AvatarPath,


                };
                members.Add(newMember);


            }
            return members;
        }
    }
}