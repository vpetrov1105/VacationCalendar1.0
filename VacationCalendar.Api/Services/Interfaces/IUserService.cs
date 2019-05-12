using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationCalendar.Api.ViewModels;

namespace VacationCalendar.Api.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<UserViewModel>> GetUsersDataAsync();
        Task<List<UserViewModel>> GetUserByIdDataAsync(int id);
        Task<ResponseViewModel<UserViewModel>> DeleteUserDataAsync(int id, LoginViewModel loggedUser);
        Task<ResponseViewModel<UserViewModel>> UpdateUserDataAsync(UserViewModel userData, LoginViewModel loggedUser);
        Task<ResponseViewModel<UserViewModel>> InsertUserDataAsync(UserViewModel userData, LoginViewModel loggedUser);
    }
}
