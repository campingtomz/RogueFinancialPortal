using RogueFinancialPortal.Enums;
using RogueFinancialPortal.Models;
using System.Collections;
using System.Collections.Generic;

namespace RogueFinancialPortal.ViewModels
{
    public class BankAccountWizardVM
    {
        public decimal StartingBalance { get; set; }
        public decimal WarningBalance { get; set; }
        public string AccountName { get; set; }      
        public AccountType AccountType  { get; set; }
    }
}