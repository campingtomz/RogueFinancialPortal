using RogueFinancialPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RogueFinancialPortal.ViewModels
{
    public class HomeHouseHoldVM
    {
        public int Id { get; set; }
        public string HouseHoldName { get; set; }
        public string OwnerName { get; set; }
        public List<HomeMemberVM> Members { get; set; }
        public List<HomeBankAccountVM> BankAccounts { get; set; }
        public List<HomeBudgetVm> Budgets { get; set; }
        public List<HomeBudgetItemsVM> BudgetItems { get; set; }
        public List<HomeTransactionsVM> Transactions { get; set; }


    }
}