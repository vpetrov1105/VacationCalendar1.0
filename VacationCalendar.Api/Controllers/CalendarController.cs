using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VacationCalendar.Api.Helpers;
using VacationCalendar.Api.Infrastructure;
using VacationCalendar.Api.Services;
using VacationCalendar.Api.Services.Interfaces;
using VacationCalendar.Api.ViewModels;

namespace VacationCalendar.Api.Controllers
{
    [Authorize]
    public class CalendarController : Controller
    {
        private ICalendarService _calendarService;
        private IVacationDataService _vacationService;
        public CalendarController(ICalendarService calendarService, IVacationDataService vacationService)
        {
            _calendarService = calendarService;
            _vacationService = vacationService;
        }

        [HttpGet]
        public async Task<InitialDataViewModel> GetInitialData([FromQuery(Name = "year")] string year, [FromQuery(Name = "month")] string month)
        {
            return await _calendarService.GetInitialDataAsync(Int32.Parse(year), Int32.Parse(month));
        }

        [HttpGet]
        public async Task<CalendarViewModel> GetCalendarData([FromQuery(Name = "year")] string year, [FromQuery(Name = "month")] string month)
        {
            return await _calendarService.GetCalendarDataAsync(Int32.Parse(year), Int32.Parse(month), "", "", null);
        }

        [HttpGet]
        public async Task<CalendarViewModel> GetFilteredCalendar([FromQuery(Name = "firstName")] string firstName, [FromQuery(Name = "lastName")] string lastName, 
                [FromQuery(Name = "vacationType")] string vacationType, [FromQuery(Name = "year")] string year, [FromQuery(Name = "month")] string month)
        {
            int vacationTypeId;
            Int32.TryParse(vacationType, out vacationTypeId);
            return await _calendarService.GetCalendarDataAsync(Int32.Parse(year), Int32.Parse(month), firstName, lastName, vacationTypeId);
        }

        [HttpPost]
        public async Task<JsonResult> DeleteVacation([FromBody]VacationDataViewModel vacation)
        {
            var loggedUser = UtilityFunctions.GetLoggedUser(User.Claims);

            var delete = await _vacationService.DeleteVacationAsync(vacation, loggedUser);
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
        public async Task<JsonResult> UpdateVacation([FromBody]List<VacationDataViewModel> vacations)
        {
            var loggedUser = UtilityFunctions.GetLoggedUser(User.Claims);

            var update = await _vacationService.UpdateVacationAsync(vacations, loggedUser);

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
