using RogueFinancialPortal.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RogueFinancialPortal.ViewModels
{
    public class HomeTransactionsVM
    {
        public int Id { get; set; }
        public string OwnerId { get; set; }
        public int AccountId { get; set; }
        public int? BudgetItemId { get; set; }
        public string BudgetItemName { get; set; }
        public decimal Amount { get; set; }
        public string Memo { get; set; }
        public string FilePath { get; set; }
        public string Created { get; set; }
        public bool IsDeleted { get; set; }
        public TransactionType TransactionType { get; set; }
        public HttpPostedFileBase TransactionFile { get; set; }

    }
}