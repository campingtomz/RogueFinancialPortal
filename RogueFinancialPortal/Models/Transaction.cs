using Microsoft.AspNet.Identity;
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
        [Display(Name = "Bank Account")]
        public string OwnerId { get; set; }
        public virtual ApplicationUser Owner { get; set; }

        public int AccountId { get; set; }
        public virtual BankAccount BanckAccount { get; set; }
        public int? BudgetItemId { get; set; }
        public virtual BudgetItem BudgetItem { get; set; }

        //public TransactionType TransactionType { get; set; }
        #endregion
        #region Actual Properties
        public DateTime Created { get; set; }
        public decimal Amount { get; set; }
        public string Memo { get; set; }

        [Display(Name = "Delete Transaction")]
        public bool IsDeleted { get; set; }
        //public AccountType AccountType { get; set; }
        #endregion
        #region virtual
        #endregion
        #region Constructor
        public Transaction(decimal startingBalance, decimal warningBalance)
        {
           
            Created = DateTime.Now;
            OwnerId = HttpContext.Current.User.Identity.GetUserId();
        }
      
        #endregion
    }
}