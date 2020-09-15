using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RogueFinancialPortal.Models
{
    public abstract class Notification
    {
        public int Id { get; set; }
        #region Parents/Children
        public int HouseHoldId { get; set; }
        public string UserId { get; set; }
        //public virtual ApplicationUser User { get; set; }
        #endregion
        #region Actual Property
        public string Subject { get; set; }
        public string Message { get; set; }
        public DateTime Created { get; set; }
        public bool IsRead { get; set; }
        #endregion
    }
}