using RogueFinancialPortal.Models;
using System.Collections.Generic;

namespace RogueFinancialPortal.ViewModels
{
    public class BudgetWizardVM
    {
        public Budget Budget { get; set; }
        public ICollection<BudgetItem> BudgetItems { get; set; }
    }
}