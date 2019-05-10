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
        public ResponseViewModel<List<VacationDataViewModel>> IsValidForDelete(VacationDataViewModel model, LoginViewModel loggedUser)
        {
            var response = new ResponseViewModel<List<VacationDataViewModel>>();

            if (model.Id == default(int))
            {
                response.Success = false;
                response.ResponseMessage.Add(ApplicationConstants.ERROR);
                return response;
            }

            response.Success = true;

            if ((model.UserID != loggedUser.Id && loggedUser.Role == Role.User) || loggedUser.Role == Role.Anonymus)
            {
                response.ResponseMessage.Add("Not allowed to maniplate data for requested user!");
                response.Success = false;
                return response;
            }

            if (model.Id == default(int))
            {
                response.ResponseMessage.Add("Vacation id is null!");
                response.Success = false;
            }

            return response;
        }

        public ResponseViewModel<List<VacationDataViewModel>> IsValidForUpdate(List<VacationDataViewModel> models, LoginViewModel loggedUser)
        {
            var response = new ResponseViewModel<List<VacationDataViewModel>>();

            if (models.Count() == 0)
            {
                response.Success = false;
                response.ResponseMessage.Add(ApplicationConstants.ERROR);
                return response;
            }

            response.Success = true;
            foreach (var model in models)
            {
                if ((model.UserID != loggedUser.Id && loggedUser.Role == Role.User) || loggedUser.Role == Role.Anonymus)
                {
                    response.ResponseMessage.Add("Not allowed to maniplate data for requested user!");
                    response.Success = false;
                    break;
                }

                if(model.CalendarDate == default(DateTime))
                {
                    response.ResponseMessage.Add("Date cannot be empty!");
                    response.Success = false;
                    break;
                }

                if (model.VacationTypeID == default(int))
                {
                    response.ResponseMessage.Add("Vacation Type cannot be empty!");
                    response.Success = false;
                    break;
                }
            }
            

            return response;
        }
    }
}
