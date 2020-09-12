using RogueFinancialPortal.Enums;
using RogueFinancialPortal.Models;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RogueFinancialPortal.ViewModels
{
    public class BankAccountWizardVM
    {
        public int Id { get; set; }
        [Display(Name = "Bank Account Name")]
        public string BankAccountName { get; set; }
        [Display(Name = "Starting Balance")]
        public decimal StartingBalance { get; set; }      
        [Display(Name = "Warning Balance")]
        public decimal WarningBalance { get; set; }

        [Display(Name = "Account Type")]
        public AccountType AccountType  { get; set; }
        public string Created { get; set; }
        public List<BudgetWizardVM> Budgets { get; set; }

    }
}