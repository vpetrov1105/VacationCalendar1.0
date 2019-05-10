using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationCalendar.Api.EntityModels;

namespace VacationCalendar.Api.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<VacationDataViewModel> VacationData { get; set; }
    }
}
