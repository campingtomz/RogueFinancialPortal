using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RogueFinancialPortal.ViewModels
{
    public class HomeBudgetItemsVM
    {
        public int Id { get; set; }
        public string OwnerId { get; set; }
        public int BudgetId { get; set; }
        public int? BudgetItemId { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public decimal CurrnetAmount { get; set; }
        public decimal TargetAmount { get; set; }      
        public string Created { get; set; }
        public bool IsDeleted { get; set; }
        public List<HomeTransactionsVM> Transactions { get; set; }

    }
}