using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VacationCalendar.Api.Infrastructure
{
    public static class ApplicationConstants
    {
        public const int HTTP_RESPONSE_UNPROCESSABLE_ENTITY = 422;
        public const int HTTP_RESPONSE_OK = 200;


        //respose messages
        public const string ERROR = "Error on submited request!!";
        public const string DELETE_ERROR= "Delete action error!";
        public const string DELETE_SUCCESS = "Data successfully deleted!";
        public const string UPDATE_ERROR = "Update action error!";
        public const string UPDATE_SUCCESS = "Data successfully updated!";
    }
}
