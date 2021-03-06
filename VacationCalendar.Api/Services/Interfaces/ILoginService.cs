﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationCalendar.Api.ViewModels;

namespace VacationCalendar.Api.Services
{
    public interface ILoginService
    {
        Task<LoginViewModel> AuthenticateAsync(string username, string password);
    }
}
