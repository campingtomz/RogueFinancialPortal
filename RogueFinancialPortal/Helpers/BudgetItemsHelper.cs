using RogueFinancialPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RogueFinancialPortal.Helpers
{
    public class BudgetItemsHelper
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        public List<BudgetItem> GetBudgetItems(int budgetId)
        {
            return db.BudgetItems.Where(bi => bi.BudgetId == budgetId).ToList();
        }
        public List<BudgetItem> GetHouseHoldBudgetItems(int houseHoldId)
        {
            List<BudgetItem> budgetItems = new List<BudgetItem>();
            var houseHold = db.HouseHolds.FirstOrDefault(h => h.Id == houseHoldId);
            foreach (var budget in houseHold.Budgets)
            {
                budgetItems.AddRange(GetBudgetItems(budget.Id));
            }
            return budgetItems;
        }
    }
}