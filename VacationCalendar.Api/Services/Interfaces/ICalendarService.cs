using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationCalendar.Api.ViewModels;

namespace VacationCalendar.Api.Services
{
    public interface ICalendarService
    {
        Task<InitialDataViewModel> GetInitialDataAsync(int year, int month);
        Task<CalendarViewModel> GetCalendarDataAsync(int year, int month, string firstName, string lastName, int? vacationType);
    }
}
