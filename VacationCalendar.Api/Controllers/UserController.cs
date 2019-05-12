using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VacationCalendar.Api.Helpers;
using VacationCalendar.Api.Infrastructure;
using VacationCalendar.Api.Services.Interfaces;
using VacationCalendar.Api.ViewModels;

namespace VacationCalendar.Api.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<List<UserViewModel>> GetUsersData()
        {
            var loggedUser = UtilityFunctions.GetLoggedUser(User.Claims);
            if(loggedUser.Role == Role.Admin)
            {
                return await _userService.GetUsersDataAsync();
            }
            else
            {
                return await _userService.GetUserByIdDataAsync(loggedUser.Id);
            }
        }

        [HttpPost]
        public async Task<JsonResult> UpdateUserData([FromBody] UserViewModel userData)
        {
            var loggedUser = UtilityFunctions.GetLoggedUser(User.Claims);

            var update = await _userService.UpdateUserDataAsync(userData, loggedUser);

            if (update.Success)
            {
                var result = Json(update);
                result.StatusCode = ApplicationConstants.HTTP_RESPONSE_OK;
                return result;
            }
            else
            {
                var result = Json(update);
                result.StatusCode = ApplicationConstants.HTTP_RESPONSE_UNPROCESSABLE_ENTITY;
                return result;
            }
        }

        [HttpPost]
        public async Task<JsonResult> InsertUserData([FromBody] UserViewModel userData)
        {
            var loggedUser = UtilityFunctions.GetLoggedUser(User.Claims);

            var update = await _userService.InsertUserDataAsync(userData, loggedUser);

            if (update.Success)
            {
                var result = Json(update);
                result.StatusCode = ApplicationConstants.HTTP_RESPONSE_OK;
                return result;
            }
            else
            {
                var result = Json(update);
                result.StatusCode = ApplicationConstants.HTTP_RESPONSE_UNPROCESSABLE_ENTITY;
                return result;
            }
        }

        [HttpPost]
        public async Task<JsonResult> DeleteUserData([FromBody] int id)
        {
            var loggedUser = UtilityFunctions.GetLoggedUser(User.Claims);

            var update = await _userService.DeleteUserDataAsync(id, loggedUser);

            if (update.Success)
            {
                var result = Json(update);
                result.StatusCode = ApplicationConstants.HTTP_RESPONSE_OK;
                return result;
            }
            else
            {
                var result = Json(update);
                result.StatusCode = ApplicationConstants.HTTP_RESPONSE_UNPROCESSABLE_ENTITY;
                return result;
            }
        }


    }
}