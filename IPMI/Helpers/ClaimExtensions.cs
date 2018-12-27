using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;
using System.Security.Claims;

namespace IPMI.Helpers
{
    public static class ClaimExtensions
    {
        static string GetUserEmail(this ClaimsIdentity identity)
        {
            return identity.Claims?.FirstOrDefault(c => c.Type == "BJA_WF.Models.RegisterViewModel.Email")?.Value;
        }

        public static string GetUserEmail(this IIdentity identity)
        {
            var claimsIdentity = identity as ClaimsIdentity;
            return claimsIdentity != null ? GetUserEmail(claimsIdentity) : "";
        }

        static string GetUserNameIdentifier(this ClaimsIdentity identity)
        {
            return identity.Claims?.FirstOrDefault(c => c.Type == "BJA_WF.Models.RegisterViewModel.NameIdentifier")?.Value;
        }

        public static string GetUserNameIdentifier(this IIdentity identity)
        {
            var claimsIdentity = identity as ClaimsIdentity;
            return claimsIdentity != null ? GetUserNameIdentifier(claimsIdentity) : "";
        }

        static string GetUserDept(this ClaimsIdentity identity)
        {
            return identity.Claims?.FirstOrDefault(c => c.Type == "BJA_WF.Models.RegisterViewModel.IdDept")?.Value;
        }

        public static string GetUserDept(this IIdentity identity)
        {
            var claimsIdentity = identity as ClaimsIdentity;
            return claimsIdentity != null ? GetUserDept(claimsIdentity) : "";
        }
    }
}