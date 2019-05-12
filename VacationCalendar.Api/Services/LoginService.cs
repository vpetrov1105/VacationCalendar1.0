using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using VacationCalendar.Api.EntityModels;
using VacationCalendar.Api.Helpers;
using VacationCalendar.Api.Infrastructure;
using VacationCalendar.Api.ViewModels;

namespace VacationCalendar.Api.Services
{
    public class LoginService : ILoginService
    {

        private readonly VacationCalendarContext _context;
        private readonly AppSettings _appSettings;

        public LoginService(VacationCalendarContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }

        public Task<LoginViewModel> AuthenticateAsync(string username, string password)
        {
            return Task.Factory.StartNew(() =>
            {
                var user = GetUserData(username, password);

                // return null if user not found
                if (user == null)
                    return null;

                // authentication successful so generate jwt token
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, user.Id.ToString()),
                        new Claim(ClaimTypes.Role, user.Role)
                    }),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                user.Token = tokenHandler.WriteToken(token);

                // remove password before returning
                user.Password = null;

                return user;
            });
        }

        private LoginViewModel GetUserData(string username, string password)
        {
            var loginData = new LoginViewModel();
            var user = _context.Users.SingleOrDefault(x => x.UserName == username && x.Password == password);
            if (user == null)
                return null;

            loginData.Id = user.ID;
            loginData.FirstName = user.FirstName;
            loginData.LastName = user.LastName;
            loginData.Username = user.UserName;
            loginData.Password = user.Password;
            loginData.Role = user.Role;

            return loginData;
        }
    }
}
