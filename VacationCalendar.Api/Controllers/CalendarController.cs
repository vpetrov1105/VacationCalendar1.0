using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VacationCalendar.Api.EntityModels;
using VacationCalendar.Api.Services;
using VacationCalendar.Api.ViewModels;

namespace VacationCalendar.Api.Controllers
{
    [Authorize]
    public class CalendarController : ControllerBase
    {
        private ICalendarService _service;
        public CalendarController(ICalendarService service)
        {
            _service = service;
        }

        [HttpGet]
        public CalendarViewModel GetCalendar([FromQuery(Name = "year")] string year, [FromQuery(Name = "month")] string month)
        {
            return _service.GetCalendarData(Int32.Parse(year), Int32.Parse(month));
        }
    }
}
