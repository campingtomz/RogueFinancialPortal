using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RogueFinancialPortal.Models
{
    public class Notification
    {
        public int Id { get; set; }
        #region Parents/Children
        public int HouseHoldId { get; set; }     
        public virtual HouseHold HouseHold { get; set; }
     

        #endregion
        #region Actual Properties

        public string RecipentId { get; set; }
        public virtual ApplicationUser Recipent { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime Created { get; set; }
        public int TTL { get; internal set; }
        public bool IsRead { get; set; }

        #endregion
        #region virtual   



        #endregion
        #region Constructor
        public Notification()
        {
            Created = DateTime.Now;
        }

        #endregion
    }
}