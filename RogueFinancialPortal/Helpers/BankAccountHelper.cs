using RogueFinancialPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RogueFinancialPortal.Helpers
{
    public class BankAccountHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public List<BankAccount> GetHouseHoldAccounts(int houseHoldId)
        {
            return db.BankAccounts.Where(ba => ba.HouseHoldId == houseHoldId).ToList();
        }
    }
}