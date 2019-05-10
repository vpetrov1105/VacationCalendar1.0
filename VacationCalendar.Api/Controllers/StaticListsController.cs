using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VacationCalendar.Api.Services;
using VacationCalendar.Api.ViewModels;

namespace VacationCalendar.Api.Controllers
{
    [Authorize]
    public class StaticListsController : Controller
    {
        private IStaticListsService _service;

        public StaticListsController(IStaticListsService service)
        {
            _service = service;
        }

        [HttpGet]
        public StaticListsViewModel GetStaticLists()
        {
            return _service.GetStaticLists();
        }
    }
}