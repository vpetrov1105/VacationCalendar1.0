using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using VacationCalendar.Api.ViewModels;

namespace VacationCalendar.Api.Helpers
{
    public static class UtilityFunctions
    {
        public static LoginViewModel GetLoggedUser(IEnumerable<Claim> claim)
        {
            var user = new LoginViewModel();
            user.Id = Int32.Parse(claim.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value);
            user.Role = claim.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;

            return user;
        }
    }
}
