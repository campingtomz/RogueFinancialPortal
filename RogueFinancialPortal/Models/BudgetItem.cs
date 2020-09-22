using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RogueFinancialPortal.Models
{
    public class BudgetItem
    {
        public int Id { get; set; }
        #region Parents/Children
        public string OwnerId { get; set; }
        public virtual ApplicationUser Owner { get; set; }
        public int BudgetId { get; set; }
        public virtual Budget Budget { get; set; }

        #endregion
        #region Actual Properties
        [Display(Name = "Name")]
        public string ItemName { get; set; }
        public DateTime Created { get; set; }
        public string Description { get; set; }

        [Display(Name = "Current Amount")]
        public decimal CurrentAmount { get; set; }
       
        [Display(Name = "Target Amount")]
        public decimal TargetAmount { get; set; }

        [Display(Name = "Delete Budget Item")]
        public bool IsDeleted { get; set; }
        #endregion
        #region virtual   
        public virtual ICollection<Transaction> Transactions { get; set; }

        #endregion
        #region Constructor
        public BudgetItem()
        {
            Transactions = new HashSet<Transaction>();
            Created = DateTime.Now;
            OwnerId = HttpContext.Current.User.Identity.GetUserId();
        }
        public BudgetItem(bool inSeed)
        { }

            #endregion
        }
}