using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationCalendar.Api.EntityModels;
using VacationCalendar.Api.ViewModels;

namespace VacationCalendar.Api.Services
{
    public class StaticListsService : IStaticListsService
    {
        private readonly VacationCalendarContext _context;

        public StaticListsService(VacationCalendarContext context)
        {
            _context = context;
        }

        public StaticListsViewModel GetStaticLists()
    {
            var lists = new StaticListsViewModel();
            lists.vacationTypes = GetVacationTypes();
            return lists;
        }

        private List<VacationTypeViewModel> GetVacationTypes()
        {
            var DBVacationTypes = _context.VacationTypes.Select(row => row).ToList();
            var vacationTypes = new List<VacationTypeViewModel>();
            foreach (var type in DBVacationTypes)
            {
                var typeView = new VacationTypeViewModel()
                {
                    ID = type.ID,
                    VacationTypeName = type.VacationTypeName
                };
                vacationTypes.Add(typeView);
            }
            return vacationTypes;
        }

    }
}
