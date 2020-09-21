using RogueFinancialPortal.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RogueFinancialPortal.ViewModels
{
    public class BudgetWizardVM
    {
        public string BudgetName { get; set; }     
        public string Description { get; set; }
        public int BankAccountId { get; set; }
   
        public List<BudgetItemsVM> BudgetItems { get; set; }

    }
}