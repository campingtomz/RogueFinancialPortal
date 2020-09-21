using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RogueFinancialPortal.ViewModels
{
    public class HomeBudgetVm
    {
        public int Id { get; set; }
        public string OwnerId { get; set; }
        public int BankAccountId { get; set; }
        public string BudgetName { get; set; }
        public string Description { get; set; }

        public decimal CurrentAmount { get; set; }
        public decimal TargetAmount { get; set; }
        public string Created { get; set; }
        public bool IsDeleted { get; set; }
        public List<HomeBudgetItemsVM> BudgetItems { get; set; }
    }
}