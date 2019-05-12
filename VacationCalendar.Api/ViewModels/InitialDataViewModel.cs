using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VacationCalendar.Api.ViewModels
{
    public class InitialDataViewModel
    {
        public CalendarViewModel Calendar { get; set; }
        public StaticListsViewModel StaticLists { get; set; }

        public InitialDataViewModel()
        {
            Calendar = new CalendarViewModel();
            StaticLists = new StaticListsViewModel();
        }
    }
}
