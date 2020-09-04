
using RogueFinancialPortal.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RogueFinancialPortal.ViewModels
{
    public class ManageUserVM
    {
        public string UserId { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        [Display(Name = "Phone Number")]

        public string PhoneNumber { get; set; }
        public string AvatarPath { get; set; }
        public HttpPostedFileBase Avatar { get; set; }

    }
}