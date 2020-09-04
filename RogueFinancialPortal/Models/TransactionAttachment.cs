using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RogueFinancialPortal.Models
{
    public class TransactionAttachment
    {
        public int Id { get; set; }
        #region Parents/Children
        public int TransactionId { get; set; }
        public virtual Transaction Transaction { get; set; }

        public string OwnerId { get; set; }
        public virtual ApplicationUser Owner { get; set; }

        #endregion

        #region Actual Property
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }

        #endregion
    }
}