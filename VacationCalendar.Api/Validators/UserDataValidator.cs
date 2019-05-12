using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationCalendar.Api.EntityModels;
using VacationCalendar.Api.Infrastructure;
using VacationCalendar.Api.ViewModels;

namespace VacationCalendar.Api.Validators
{
    public class UserDataValidator
    {
        public ResponseViewModel<UserViewModel> IsValidForDelete(int id, LoginViewModel loggedUser)
        {
            var response = new ResponseViewModel<UserViewModel>();
            if (id == default(int))
            {
                response.Success = false;
                response.ResponseMessages.Add(ApplicationConstants.ERROR);
                return response;
            }

            response.Success = true;
            if (loggedUser.Role != Role.Admin)
            {
                response.ResponseMessages.Add("Not authorized for delete action!");
                response.Success = false;
            }

            return response;

        }

        public ResponseViewModel<UserViewModel> IsValidForInsert(UserViewModel model, LoginViewModel loggedUser, VacationCalendarContext context)
        {
            var response = new ResponseViewModel<UserViewModel>();

            if (model == null)
            {
                response.Success = false;
                response.ResponseMessages.Add(ApplicationConstants.ERROR);
                return response;
            }

            response.Success = true;
            if (loggedUser.Role != Role.Admin)
            {
                response.ResponseMessages.Add("Not allowed to manipulate data for requested user!");
                response.Success = false;
                return response;
            }

            if (model.Id != default(int))
            {
                response.ResponseMessages.Add("Id must be empty!");
                response.Success = false;
            }

            if (model.BirthDate == default(DateTime))
            {
                response.ResponseMessages.Add("Birth date cannot be empty!");
                response.Success = false;
            }

            if (string.IsNullOrEmpty(model.FirstName))
            {
                response.ResponseMessages.Add("First name cannot be empty!");
                response.Success = false;
            }

            if (string.IsNullOrEmpty(model.LastName))
            {
                response.ResponseMessages.Add("Last name cannot be empty!");
                response.Success = false;
            }
            if (string.IsNullOrEmpty(model.UserName))
            {
                response.ResponseMessages.Add("User name cannot be empty!");
                response.Success = false;
            }
            else
            {
                var existingUsername = context.Users.SingleOrDefault(x => x.UserName == model.UserName)?.UserName;
                if (existingUsername != default(string))
                {
                    response.ResponseMessages.Add("User name already exists! Please choose another one!");
                    response.Success = false;
                }
            }

            if (string.IsNullOrEmpty(model.OfficeCountryCode))
            {
                response.ResponseMessages.Add("Office Country code cannot be empty!");
                response.Success = false;
            }

            if (string.IsNullOrEmpty(model.Role))
            {
                response.ResponseMessages.Add("Role code cannot be empty!");
                response.Success = false;
            }

            return response;
        }

        public ResponseViewModel<UserViewModel> IsValidForUpdate(UserViewModel model, LoginViewModel loggedUser)
        {
            var response = new ResponseViewModel<UserViewModel>();

            if (model == null)
            {
                response.Success = false;
                response.ResponseMessages.Add(ApplicationConstants.ERROR);
                return response;
            }

            response.Success = true;
            if ((model.Id != loggedUser.Id)  && loggedUser.Role != Role.Admin)
            {
                response.ResponseMessages.Add( "Not allowed to manipulate data for requested user!");
                response.Success = false;
                return response;
            }

            if (model.BirthDate == default(DateTime))
            {
                response.ResponseMessages.Add("Birth date cannot be empty!");
                response.Success = false;
            }

            if (string.IsNullOrEmpty(model.FirstName))
            {
                response.ResponseMessages.Add("First name cannot be empty!");
                response.Success = false;
            }

            if (string.IsNullOrEmpty(model.LastName))
            {
                response.ResponseMessages.Add("Last name cannot be empty!");
                response.Success = false;
            }

            if (string.IsNullOrEmpty(model.UserName))
            {
                response.ResponseMessages.Add("User name cannot be empty!");
                response.Success = false;
            }

            if (string.IsNullOrEmpty(model.OfficeCountryCode))
            {
                response.ResponseMessages.Add("Office Country code cannot be empty!");
                response.Success = false;
            }

            if (string.IsNullOrEmpty(model.Role) && loggedUser.Role == Role.Admin)
            {
                response.ResponseMessages.Add("Role code cannot be empty!");
                response.Success = false;
            }

            return response;
        }
    }
}
