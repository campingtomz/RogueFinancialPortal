using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using RogueFinancialPortal.Extensions;

namespace RogueFinancialPortal.Models
{
    public class BankAccount
    {
        public int Id { get; set; }
        #region Parents/Children
        public int HouseHoldId { get; set; }
        public string OwnerId { get; set; }
        public virtual HouseHold HouseHold { get; set; }
        public virtual ApplicationUser Owner { get; set; }

        #endregion
        #region Actual Properties
        [Display(Name = "Bank Account Name")]
        public string BankAccountName { get; set; }
        public DateTime Created { get; set; }
        [Display(Name = "Starting Balance")]
        public decimal StartingBalance { get; internal set; }
        [Display(Name = "Current Balance")]
        public decimal CurrentBalance { get; set; }
        [Display(Name = "Warning Balance")]
        public decimal WarningBalance { get; set; }
        [Display(Name = "Delete Account")]
        public bool IsDeleteted { get; set; }
        //public AccountType AccountType { get; set; }
        #endregion
        #region virtual
        public virtual ICollection<Transaction> Transactions { get; set; }
        #endregion
        #region Constructor
        public BankAccount(decimal startingBalance, decimal warningBalance)
        {
            Transactions = new HashSet<Transaction>();
            StartingBalance = startingBalance;
            CurrentBalance = StartingBalance;
            WarningBalance = warningBalance - 1000;
            Created = DateTime.Now;
            OwnerId = HttpContext.Current.User.Identity.GetUserId();
            HouseHoldId = (int)HttpContext.Current.User.Identity.GetHouseHoldId();
        }
        public BankAccount()
        {
            StartingBalance = -1;
        }
        #endregion

    }
}