using RogueFinancialPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RogueFinancialPortal.Helpers
{
    public class HouseHoldHelper
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        public List<HouseHold> ListHouseHolds()
        {
            return db.HouseHolds.ToList();
        }
        public List<ApplicationUser> GetHouseHoldMembers(int houseHoldId)
        {
            return db.Users.Where(u => u.HouseHoldId == houseHoldId).ToList();
        }
    }
}