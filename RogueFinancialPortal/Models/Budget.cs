using Microsoft.AspNet.Identity;
using RogueFinancialPortal.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RogueFinancialPortal.Models
{

    public class Budget
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        public int Id { get; set; }
        #region Parents/Children
        public int HouseHoldId { get; set; }
        public string OwnerId { get; set; }
        public int BankAccountId { get; set; }
        public string Description { get; set; }
        public virtual HouseHold HouseHold { get; set; }
        public virtual ApplicationUser Owner { get; set; }

        #endregion
        #region Actual Properties
        [Display(Name = "Name")]
        public string BudgetName { get; set; }

        public DateTime Created { get; set; }
        public bool IsDeleted { get; set; }
        [Display(Name = "Currnet Amount")]
        public decimal CurrentAmount { get; set; }
        [NotMapped]
        [Display(Name = "Target Amount")]
        public decimal TargetAmount {
            get
            {
                var target = db.BudgetItems.Where(bI => bI.BudgetId == Id).Count();
                return target != 0 ? db.BudgetItems.Where(bI => bI.BudgetId == Id).Sum(s => s.TargetAmount) : 0;
            }
                }
        #endregion
        #region virtual   
        public virtual ICollection<BudgetItem> Items { get; set; }

        #endregion
        #region Constructor
        public Budget()
        {
            Items = new HashSet<BudgetItem>();
            Created = DateTime.Now;
            OwnerId = HttpContext.Current.User.Identity.GetUserId();
            HouseHoldId = (int)HttpContext.Current.User.Identity.GetHouseHoldId();

        }
        public Budget(bool inSeed)
        {

        }
        #endregion
    }
}