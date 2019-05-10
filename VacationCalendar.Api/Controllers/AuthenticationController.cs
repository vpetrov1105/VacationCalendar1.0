using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VacationCalendar.Api.Infrastructure;
using VacationCalendar.Api.Services;
using VacationCalendar.Api.ViewModels;

namespace VacationCalendar.Api.Controllers
{
    [Authorize]
    public class AuthenticationController : ControllerBase
    {
        private ILoginService _userService;

        public AuthenticationController(ILoginService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Authenticate([FromBody] LoginViewModel userParam)
        {
            var user = _userService.Authenticate(userParam.Username, userParam.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }
    }
}