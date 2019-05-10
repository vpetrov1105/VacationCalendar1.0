using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using VacationCalendar.Api.EntityModels;
using VacationCalendar.Api.ViewModels;

namespace VacationCalendar.Api.Services
{
    public interface ICalendarService
    {
        CalendarViewModel GetCalendarData(int year, int month);
    }

    public class CalendarService : ICalendarService
    {
        private readonly VacationCalendarContext _context;
        private IVacationDataService _vacationService;

        public CalendarService(VacationCalendarContext context, IVacationDataService vacationService)
        {
            _context = context;
            _vacationService = vacationService;
        }
        public CalendarViewModel GetCalendarData(int year, int month)
        {
            CalendarViewModel calendar = new CalendarViewModel();
            calendar.Users = _vacationService.GetUsersData(year, month);
            calendar.Year = year;
            calendar.Month = month;
            calendar.MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
            return calendar;
        }
    }
}
