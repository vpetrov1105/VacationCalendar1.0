using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VacationCalendar.Api.EntityModels;
using VacationCalendar.Api.Helpers;
using VacationCalendar.Api.Infrastructure;
using VacationCalendar.Api.Services;
using VacationCalendar.Api.ViewModels;

namespace VacationCalendar.Api.Controllers
{
    [Authorize]
    public class VacationController : Controller
    {
        private IVacationDataService _service;

        public VacationController(IVacationDataService service)
        {
            _service = service;
        }

        [HttpPost]
        public JsonResult DeleteVacation([FromBody]VacationDataViewModel vacation)
        {
            var loggedUser = UtilityFunctions.GetLoggedUser(User.Claims);

            var delete = _service.DeleteVacation(vacation, loggedUser);
            if (delete.Success)
            {
                var result = Json(delete);
                result.StatusCode = ApplicationConstants.HTTP_RESPONSE_OK;
                return result;
            }
            else
            {
                var result = Json(delete);
                result.StatusCode = ApplicationConstants.HTTP_RESPONSE_UNPROCESSABLE_ENTITY;
                return result;
            }
        }

        [HttpPost]
        public JsonResult UpdateVacation([FromBody]List<VacationDataViewModel> vacations)
        {
            var loggedUser = UtilityFunctions.GetLoggedUser(User.Claims);

            var update = _service.UpdateVacation(vacations, loggedUser);

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