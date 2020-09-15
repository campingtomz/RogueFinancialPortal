using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RogueFinancialPortal.ViewModels
{
    public class InvitationVM
    {
        public int Id { get; set; }
        public string HouseHoldName { get; set; }
        public Guid Code { get; set; }

    }
}