using Microsoft.AspNet.Identity;
using RogueFinancialPortal.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace RogueFinancialPortal.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        #region Parents/Children
        
        public string OwnerId { get; set; }
        public virtual ApplicationUser Owner { get; set; }
        public int BankAccountId { get; set; }
        public virtual BankAccount BanckAccount { get; set; }
        public int? BudgetItemId { get; set; }
        public virtual BudgetItem BudgetItem { get; set; }

        public TransactionType TransactionType { get; set; }
        #endregion
        #region Actual Properties
        public DateTime Created { get; set; }
        public decimal Amount { get; set; }
        public string Memo { get; set; }
        public string FilePath { get; set; }

        [Display(Name = "Delete Transaction")]
        public bool IsDeleted { get; set; }
        #endregion
        #region virtual
        #endregion
        #region Constructor
        public Transaction(decimal amount, string memo)
        {
            Amount = amount;
            Memo = memo;
            Created = DateTime.Now;
            OwnerId = HttpContext.Current.User.Identity.GetUserId();
        }
        public Transaction()
        {

            Created = DateTime.Now;
            OwnerId = HttpContext.Current.User.Identity.GetUserId();
        }

        #endregion
    }
}