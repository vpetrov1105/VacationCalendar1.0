using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationCalendar.Api.ViewModels;

namespace VacationCalendar.Api.Services.Interfaces
{
    public interface IVacationDataService
    {
        Task<ResponseViewModel<List<VacationDataViewModel>>> DeleteVacationAsync(List<VacationDataViewModel> vacations, LoginViewModel loggedUser);
        Task<ResponseViewModel<List<VacationDataViewModel>>> UpdateVacationAsync (List<VacationDataViewModel> vacations, LoginViewModel loggedUser);
        List<UserViewModel> GetUsersDataIncludeVacation(int year, int month, string firstName, string lastName, int? vacationType);
    }
}
