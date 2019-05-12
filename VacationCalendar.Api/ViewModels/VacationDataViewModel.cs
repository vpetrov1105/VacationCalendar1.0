using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationCalendar.Api.EntityModels;
using VacationCalendar.Api.Infrastructure;

namespace VacationCalendar.Api.ViewModels
{
    public class VacationDataViewModel
    {
        public int Id { get; set; }
        public DateTime CalendarDate { get; set; }
        public int Day { get; set; }
        public string DayName { get; set; }
        public bool IsOnVacation { get; set; }
        public int VacationTypeID { get; set; }
        public int UserID { get; set; }
        public byte[] RowVersion { get; set; }
        public bool IsNonWorkingDay { get; set; }

    }
}
