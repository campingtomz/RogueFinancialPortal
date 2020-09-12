using RogueFinancialPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace RogueFinancialPortal.Extensions
{
    public static class InvitationExtensions
    {
        public static async Task SendInvitation(this Invitation invitation)
        {
            var url = new UrlHelper(HttpContext.Current.Request.RequestContext);
            var callbackUrl = url.Action("AcceptInvitation", "Account", new { recipientEmail = invitation.RecipientEmail, code = invitation.Code }, protocol: HttpContext.Current.Request.Url.Scheme);
            var from = $"Rogue Financial Planner <campingtomz@gmail.com>";
            var emailMessage = new MailMessage(from, invitation.RecipientEmail)
            {
                Subject = "You have been invited to join the Rogue Financial Planner",
                Body = $"You can create a new Account and join as a member by clicking this link: <a href=\"{callbackUrl}\">Join</a><br/> <hr/> If you have already created an account Copy and paste the following code to join the HouseHold: Code= {invitation.Code}",
                IsBodyHtml = true
            };

            var svc = new EmailService();
            await svc.SendAsync(emailMessage);
    }
}
}