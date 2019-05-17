using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationCalendar.Api.Infrastructure;
using VacationCalendar.Api.ViewModels;

namespace VacationCalendar.Api.Validators
{
    public class VacationDataValidator
    {
        public ResponseViewModel<List<VacationDataViewModel>> IsValidForDelete(List<VacationDataViewModel> models, LoginViewModel loggedUser)
        {
            var response = new ResponseViewModel<List<VacationDataViewModel>>();
            if (!models.Any())
            {
                response.Success = false;
                response.ResponseMessages.Add(ApplicationConstants.ERROR);
                return response;
            }

            response.Success = true;
            Dictionary<int, string> messages = new Dictionary<int, string>();
            foreach (var model in models)
            {
                if (model.Id == default(int))
                {
                    response.Success = false;
                    response.ResponseMessages.Add(ApplicationConstants.ERROR);
                    return response;
                }

                response.Success = true;

                if ((model.UserID != loggedUser.Id && loggedUser.Role == Role.User) || loggedUser.Role == Role.Anonymous)
                {
                    if (!messages.ContainsKey(1))
                        messages.Add(1, "Not allowed to maniplate data for requested user!");
         
                    response.Success = false;
                }

                if (model.Id == default(int))
                {
                    if (!messages.ContainsKey(2))
                        messages.Add(2, "Vacation id is null!");

                    response.Success = false;
                }
            }

            if (!response.Success)
                response.ResponseMessages = messages.Values.ToList();

            return response;
        }

        public ResponseViewModel<List<VacationDataViewModel>> IsValidForUpdate(List<VacationDataViewModel> models, LoginViewModel loggedUser)
        {
            var response = new ResponseViewModel<List<VacationDataViewModel>>();

            if (!models.Any())
            {
                response.Success = false;
                response.ResponseMessages.Add(ApplicationConstants.ERROR);
                return response;
            }

            response.Success = true;
            Dictionary<int, string> messages = new Dictionary<int, string>();
            foreach (var model in models)
            {
                if ((model.UserID != loggedUser.Id && loggedUser.Role == Role.User) || loggedUser.Role == Role.Anonymous)
                {
                    if (!messages.ContainsKey(1))
                        messages.Add(1, "Not allowed to manipulate data for requested user!");

                    response.Success = false;
                }

                if(model.CalendarDate == default(DateTime))
                {
                    if (!messages.ContainsKey(2))
                        messages.Add(2, "Date cannot be empty!");

                    response.Success = false;
                }

                if (model.VacationTypeID == default(int))
                {
                    if (!messages.ContainsKey(3))
                        messages.Add(3, "Vacation Type cannot be empty!");

                    response.Success = false;
                }
            }

            if (!response.Success)
                response.ResponseMessages = messages.Values.ToList();
            

            return response;
        }
    }
}
