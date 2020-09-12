using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RogueFinancialPortal.Models
{
    public class HouseHold
    {
        public int Id { get; set; }
        #region Parents/Children
        public string OwnerId { get; set; }   
        public virtual ApplicationUser Owner { get; set; }

        #endregion
        #region Actual Properties
        [Display(Name = "Name")]
        public string HouseHoldName { get; set; }
        public string Greeting { get; set; }

        public DateTime Created { get; set; }
        [Display(Name = "Delete HouseHold Item")]
        public bool IsDeleted { get; set; }
        #endregion
        #region virtual   
        public virtual ICollection<ApplicationUser> Members { get; set; }
        public virtual ICollection<BankAccount> BankAccounts { get; set; }
        public virtual ICollection<Budget> Budgets { get; set; }
        public virtual ICollection<Invitation> Invitations { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }


        #endregion
        #region Constructor
        public HouseHold()
        {
            Budgets = new HashSet<Budget>();
            BankAccounts = new HashSet<BankAccount>();
            Members = new HashSet<ApplicationUser>();
            Invitations = new HashSet<Invitation>();
            Notifications = new HashSet<Notification>();

            Created = DateTime.Now;
            OwnerId = HttpContext.Current.User.Identity.GetUserId();
        }
        public HouseHold(string houseHoldName, string greeting)
        {
            Budgets = new HashSet<Budget>();
            BankAccounts = new HashSet<BankAccount>();
            Members = new HashSet<ApplicationUser>();
            Invitations = new HashSet<Invitation>();
            Notifications = new HashSet<Notification>();
            HouseHoldName = houseHoldName;
            Greeting = greeting;
            Created = DateTime.Now;
            OwnerId = HttpContext.Current.User.Identity.GetUserId();
        }
        public HouseHold(bool inSeed)
        {
            Budgets = new HashSet<Budget>();
            BankAccounts = new HashSet<BankAccount>();
            Members = new HashSet<ApplicationUser>();
            Invitations = new HashSet<Invitation>();
            Notifications = new HashSet<Notification>();

            Created = DateTime.Now;
        }
        #endregion
    }
}