﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VacationCalendar.Api.ViewModels
{
    public class ResponseViewModel<T>
    {
        public bool Success { get; set; }
        public List<string> ResponseMessages { get; set; }
        public T ReturnedObject { get; set; }

        public ResponseViewModel()
        {
            ResponseMessages = new List<string>();
        }
    }
}
