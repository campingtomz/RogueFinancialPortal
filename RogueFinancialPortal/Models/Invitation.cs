using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RogueFinancialPortal.Models
{
    public class Invitation
    {
        public int Id { get; set; }
        #region Parents/Children
        public string OwnerId { get; set; }
        public virtual ApplicationUser Owner { get; set; }
        public int HouseHoldId { get; set; }
        public virtual HouseHold HouseHold { get; set; }

        #endregion
        #region Actual Properties
        
        public string Body { get; set; }
        public string Subject { get; set; }
        public string Greeting { get; set; }
        public bool IsValid { get; set; }
        public DateTime Created { get; set; }
        public int TTL { get; internal set; }
        [Display(Name = "Recipient Email")]
        public string RecipientEmail { get; set; }
        public Guid Code{ get; set; }

        #endregion
        #region virtual   



        #endregion
        #region Constructor
        public Invitation()
        {
            IsValid = true;
            Created = DateTime.Now;
            OwnerId = HttpContext.Current.User.Identity.GetUserId();
            TTL = 3;
        }
        public Invitation(int hhId)
        {
            IsValid = true;
            Created = DateTime.Now;
            OwnerId = HttpContext.Current.User.Identity.GetUserId();
            HouseHoldId = hhId;
            TTL = 3;
        }

        #endregion
    }
}