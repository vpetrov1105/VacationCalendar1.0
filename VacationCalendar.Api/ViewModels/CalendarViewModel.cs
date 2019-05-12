using System.Collections.Generic;


namespace VacationCalendar.Api.ViewModels
{
    public class CalendarViewModel
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public string MonthName { get; set; }
        public List<UserViewModel> Users { get; set; }

    }
}
