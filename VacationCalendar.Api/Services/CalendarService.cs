using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using VacationCalendar.Api.EntityModels;
using VacationCalendar.Api.Services.Interfaces;
using VacationCalendar.Api.ViewModels;

namespace VacationCalendar.Api.Services
{
    public class CalendarService : ICalendarService
    {
        private readonly VacationCalendarContext _context;
        private IVacationDataService _vacationService;
        private IStaticListsService _staticListsService;

        public CalendarService(VacationCalendarContext context, IVacationDataService vacationService, IStaticListsService staticListsService)
        {
            _context = context;
            _vacationService = vacationService;
            _staticListsService = staticListsService;
        }

        public Task<InitialDataViewModel> GetInitialDataAsync(int year, int month)
        {
            return Task.Factory.StartNew(() =>
            {
                var initialData = new InitialDataViewModel();
                initialData.Calendar.Users = _vacationService.GetUsersDataIncludeVacation(year, month, "", "", null);
                initialData.Calendar.Year = year;
                initialData.Calendar.Month = month;
                initialData.Calendar.MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);

                initialData.StaticLists = _staticListsService.GetStaticLists();

                return initialData;
            });
        }

        public Task<CalendarViewModel> GetCalendarDataAsync(int year, int month , string firstName, string lastName, int? vacationType)
        {
            return Task.Factory.StartNew(() =>
            {
                CalendarViewModel calendar = new CalendarViewModel();
                calendar.Users = _vacationService.GetUsersDataIncludeVacation(year, month, firstName, lastName, vacationType);
                calendar.Year = year;
                calendar.Month = month;
                calendar.MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
                return calendar;
            });
        }
    }
}
