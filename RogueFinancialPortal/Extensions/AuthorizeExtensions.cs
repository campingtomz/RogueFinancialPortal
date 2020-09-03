﻿using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity.Owin;
using RogueFinancialPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace RogueFinancialPortal.Extensions
{
    public static class AuthorizeExtensions
    {
        public static async Task RefreshAuthentication(this HttpContextBase context, ApplicationUser user)
        {
            context.GetOwinContext().Authentication.SignOut();
            await context.GetOwinContext().Get<ApplicationSignInManager>().SignInAsync(user, isPersistent:false, rememberBrowser:false);
        }
    }
}