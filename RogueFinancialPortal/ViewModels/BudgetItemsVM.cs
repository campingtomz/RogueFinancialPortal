using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RogueFinancialPortal.ViewModels
{
    public class BudgetItemsVM
    {
        public decimal CurrnetAmount { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public decimal TargetAmount { get; set; }



    }
}