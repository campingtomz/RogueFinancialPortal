using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RogueFinancialPortal.ViewModels
{
    public class HomeMemberVM
    {
        public string Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string FullName { get; set; }

        public string Descriptiton { get; set; }
        public string AvatarPath { get; set; }
    }
}