using RogueFinancialPortal.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace RogueFinancialPortal.Helpers
{
    public static class InvitationHelper
    {
        private static ApplicationDbContext db = new ApplicationDbContext();
        public static void MarkAsInvaild(int id)
        {
            var invitation = db.Invitations.Find(id);
            invitation.IsValid = false;
        }

    }
}