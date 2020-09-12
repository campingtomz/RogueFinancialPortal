using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace RogueFinancialPortal.Extensions
{
    public static class IdentityExtensions
    {
        public static int? GetHouseHoldId(this IIdentity user){
            var claimsIdentity = (ClaimsIdentity)user;
            var householdClaim = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "HouseHoldId");
            if (householdClaim != null)
            {
                var result = householdClaim.Value != "" ? int.Parse(householdClaim.Value) : 0;
                return result;
            }
            else
            {
                return null;
            }

        }
        public static string GetFullName(this IIdentity user)
        {
            var claimsIdentity = (ClaimsIdentity)user;
            var fullNameClaim = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "FullName");
            return  fullNameClaim.Value != null ? fullNameClaim.Value : null;


        }
        public static string GetFirstName(this IIdentity user)
        {
            var claimsIdentity = (ClaimsIdentity)user;
            var firstNameClaim = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "FirstName");
            return firstNameClaim.Value != null ? firstNameClaim.Value : null;


        }
        public static string GetLastName(this IIdentity user)
        {
            var claimsIdentity = (ClaimsIdentity)user;
            var lastNameClaim = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "LaststName");
            return lastNameClaim.Value != null ? lastNameClaim.Value : null;


        }
        public static string GetAvatarPath(this IIdentity user)
        {
            var claimsIdentity = (ClaimsIdentity)user;
            var avatarClaim = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "AvatarPath");
            return avatarClaim.Value != null ? avatarClaim.Value : null;


        }
        
    }
}