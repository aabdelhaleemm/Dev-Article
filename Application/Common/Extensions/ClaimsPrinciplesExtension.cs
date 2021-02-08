using System;
using System.Security.Claims;

namespace Application.Common.Extensions
{
    public static class ClaimsPrinciplesExtension
    {
        public static int GetUserId(this ClaimsPrincipal user)
        {
            return Convert.ToInt32(user.FindFirst("Id")?.Value);
            
        }
    }
}