using RogueFinancialPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RogueFinancialPortal.Helpers
{
    public class BudgetHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public List<Budget> GetHouseHoldBudgets(int houseHoldId)
        {
            return db.Budgets.Where(b => b.HouseHoldId == houseHoldId).ToList();
        }

    }
}