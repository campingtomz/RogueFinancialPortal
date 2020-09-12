using RogueFinancialPortal.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RogueFinancialPortal.ViewModels
{
    public class HomeBankAccountVM
    {
        public int Id { get; set; }   
        public string Created { get; set; }

        public AccountType AccountType { get; set; }
        public string BankAccountName { get; set; }
        public decimal StartingBalance { get;  set; }
        public decimal CurrentBalance { get; set; }
        public decimal WarningBalance { get; set; }

 
    }
}