using RogueFinancialPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RogueFinancialPortal.ViewModels
{
    public class HouseHoldCreationWizardVM
    {

        public int HouseHoldId { get; set; }
        public int OwnerId { get; set; }

        //public ICollection<BankAccount> BankAccounts { get; set; }
        //public ICollection<Budget> Budgets { get; set; }

        //public ICollection<BudgetItem> BudgetItems { get; set; }

        public ICollection<BankAccountWizardVM> BankAccounts { get; set; }
        public ICollection<BudgetWizardVM> Budgets { get; set; }

    }
}