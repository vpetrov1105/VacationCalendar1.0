using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationCalendar.Api.EntityModels;
using VacationCalendar.Api.Helpers;
using VacationCalendar.Api.Infrastructure;
using VacationCalendar.Api.Services.Interfaces;
using VacationCalendar.Api.Validators;
using VacationCalendar.Api.ViewModels;

namespace VacationCalendar.Api.Services
{
    public class VacationDataService : IVacationDataService
    {
        private readonly VacationCalendarContext _context;
        private readonly VacationDataValidator _validator;

        public VacationDataService(VacationCalendarContext context, VacationDataValidator validator)
        {
            _context = context;
            _validator = validator;
        }

        public Task<ResponseViewModel<List<VacationDataViewModel>>> DeleteVacationAsync (List<VacationDataViewModel> vacations, LoginViewModel loggedUser)
        {
            return Task.Factory.StartNew(() =>
            {
                var response = new ResponseViewModel<List<VacationDataViewModel>>();

                response = _validator.IsValidForDelete(vacations, loggedUser);
                if (!response.Success)
                {
                    return response;
                }

                try
                {
                    foreach (var vacation in vacations) { 

                        var vacationToDelete = _context.VacationData
                                    .SingleOrDefault(c => c.ID == vacation.Id);

                        if (vacationToDelete == null)
                        {
                            throw new Exception(string.Format("Meanwhile the vacation was deleted by another user!"));
                        }

                        _context.VacationData.Remove(_context.VacationData.Find(vacation.Id));
                    }

                      _context.SaveChanges();

                    response.Success = true;
                    response.ResponseMessages.Add(ApplicationConstants.DELETE_SUCCESS);
                    response.ReturnedObject = GetVacationForUser(vacations.FirstOrDefault().CalendarDate.Year,
                                                            vacations.FirstOrDefault().CalendarDate.Month,
                                                            null,
                                                            vacations.FirstOrDefault().UserID);
                    return response;
                }
                catch (Exception e)
                {
                    response.Success = false;
                    response.ResponseMessages.Add(ApplicationConstants.DELETE_ERROR + " - " + e.Message);
                    response.ReturnedObject = GetVacationForUser(vacations.FirstOrDefault().CalendarDate.Year,
                                                    vacations.FirstOrDefault().CalendarDate.Month,
                                                    null,
                                                    vacations.FirstOrDefault().UserID);
                    return response;
                }
            });
        }

        public Task<ResponseViewModel<List<VacationDataViewModel>>> UpdateVacationAsync (List<VacationDataViewModel> vacations, LoginViewModel loggedUser)
        {
            return Task.Factory.StartNew(() =>
            {
                var response = new ResponseViewModel<List<VacationDataViewModel>>();
                var vacationToUpdate = new VacationData();

                response = _validator.IsValidForUpdate(vacations, loggedUser);
                if (!response.Success)
                {
                    return response;
                }
                try
                {
                    foreach (var vacation in vacations)
                    {
                        //Check if data is for insert of update
                        var vacationDB = _context.VacationData
                            .SingleOrDefault(c => c.VacationDate == vacation.CalendarDate && c.UserID == vacation.UserID);

                        if (vacationDB == null && vacation.Id == default(int))
                        {
                            var dataForInsert = VacationDataMapper(vacation);
                            _context.VacationData.Add(dataForInsert);
                        }
                        else
                        {
                            //Check if vacation record is deleted meanwhile
                            vacationToUpdate = _context.VacationData
                                .SingleOrDefault(c => c.ID == vacation.Id);

                            if (vacationToUpdate == null)
                            {
                                throw new Exception("The vacation was deleted by another user. Click again for insert!");
                            }

                            // throws concurrency exception on save changes if item is already modified by another request
                            _context.Entry(vacationToUpdate).Property("RowVersion").OriginalValue = vacation.RowVersion;

                            //change values for update
                            vacationToUpdate.VacationTypeID = vacation.VacationTypeID;
                            _context.Update(vacationToUpdate).State = EntityState.Modified;
                        }
                    }

                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    string errorMessage;
                    var exceptionEntry = ex.Entries.First();
                    var clientValues = (VacationData)exceptionEntry.Entity;
                    var databaseEntry = exceptionEntry.GetDatabaseValues();

                    if (databaseEntry == null)
                    {
                        errorMessage = "The vacation was deleted by another user. Click again for insert!";
                    }
                    else
                    {
                        var databaseValues = (VacationData)databaseEntry.ToObject();
                        if (databaseValues.VacationTypeID != clientValues.VacationTypeID)
                        {
                            errorMessage = "The Vacation Type is changed by another user! Click again for update!";
                        }
                        else
                        {
                            errorMessage = "Some data changed meanwhile! Click again for update!";
                        }
                    }

                    _context.Entry(vacationToUpdate).State = EntityState.Detached;

                    response.Success = false;
                    response.ResponseMessages.Add(ApplicationConstants.UPDATE_ERROR + " - " + errorMessage);
                    response.ReturnedObject = GetVacationForUser(vacations.FirstOrDefault().CalendarDate.Year,
                                                    vacations.FirstOrDefault().CalendarDate.Month,
                                                    null,
                                                    vacations.FirstOrDefault().UserID);
                }
                catch (Exception e)
                {
                    response.Success = false;
                    response.ResponseMessages.Add(ApplicationConstants.UPDATE_ERROR + " - " + e.Message);
                    response.ReturnedObject = GetVacationForUser(vacations.FirstOrDefault().CalendarDate.Year,
                                                    vacations.FirstOrDefault().CalendarDate.Month,
                                                    null,
                                                    vacations.FirstOrDefault().UserID);
                    return response;
                }
                response.Success = true;
                response.ResponseMessages.Add(ApplicationConstants.UPDATE_SUCCESS);
                response.ReturnedObject = GetVacationForUser(vacations.FirstOrDefault().CalendarDate.Year,
                                                        vacations.FirstOrDefault().CalendarDate.Month,
                                                        null,
                                                        vacations.FirstOrDefault().UserID);
                return response;
            });
        }

        public List<UserViewModel> GetUsersDataIncludeVacation(int year, int month, string firstName, string lastName, int? vacationType)
        {
            List<User> dbUsersData = new List<User>();
            List<UserViewModel> users = new List<UserViewModel>();

            if (string.IsNullOrEmpty(firstName) && string.IsNullOrEmpty(lastName))
            {
                dbUsersData = _context.Users.Include(user => user.VacationData).AsNoTracking().ToList();
            }
            else{
                dbUsersData = _context.Users.Include(user => user.VacationData)
                .Where(x => (!string.IsNullOrEmpty(firstName) && x.FirstName.ToLower().Contains(firstName.ToString().ToLower())) || (!string.IsNullOrEmpty(lastName) && x.LastName.ToLower().Contains(lastName.ToString().ToLower())))
                .AsNoTracking()
                .ToList();
            }

            foreach (var user in dbUsersData)
            {
                var vacation_data = GetVacationForUser(year, month, vacationType, user.ID);

                if (vacationType != default(int) && vacationType != null && vacation_data.Count <= 0)
                    continue;

                var data = new UserViewModel();
                data.Id = user.ID;
                data.FirstName = user.FirstName;
                data.LastName = user.LastName;
                data.VacationData = vacation_data;

                if (data.VacationData.SingleOrDefault(x => x.IsOnVacation == true && x.IsToday == true) != default(VacationDataViewModel))
                {
                    data.IsCurrentlyOnVacation = true;
                }

                users.Add(data);
            }
            return users;
        }

        private List<VacationDataViewModel> GetVacationForUser(int year, int month, int? vacationType, int userId)
        {
            List<VacationData> vacationDB = new List<VacationData>();
            List<VacationDataViewModel> vacation = new List<VacationDataViewModel>();

            if (vacationType != default(int) && vacationType != null)
            {
                vacationDB = _context.VacationData.Where(x => x.UserID == userId && x.VacationDate.Year == year && x.VacationDate.Month == month && x.VacationTypeID == vacationType).Select(row => row).ToList();
                if (vacationDB.Count <= 0)
                    return vacation;
            }
            else{
                vacationDB = _context.VacationData.Where(x => x.UserID == userId && x.VacationDate.Year == year && x.VacationDate.Month == month).Select(row => row).ToList();
            }

            int monthDays = DateTime.DaysInMonth(year, month);

            for (int day = 1; day <= monthDays; day++)
            {
                var data = new VacationDataViewModel();
                data.Day = day;
                data.CalendarDate = new DateTime(year, month, day);
                data.Id = vacationDB.FirstOrDefault(x => x.VacationDate == data.CalendarDate)?.ID ?? default(int);
                data.IsOnVacation = vacationDB.Any(x => x.ID == data.Id);
                data.DayName = data.CalendarDate.DayOfWeek.ToString().Substring(0, 3);
                data.VacationTypeID = vacationDB.FirstOrDefault(x => x.ID == data.Id)?.VacationTypeID ?? default(int);
                data.UserID = userId;
                data.RowVersion = vacationDB.FirstOrDefault(x => x.ID == data.Id)?.RowVersion;
                if ((data.CalendarDate.DayOfWeek == DayOfWeek.Saturday) || (data.CalendarDate.DayOfWeek == DayOfWeek.Sunday))
                {
                    data.IsNonWorkingDay = true;
                }
                else
                {
                    data.IsNonWorkingDay = false;
                }

                if (data.CalendarDate.Date == DateTime.Today)
                {
                    data.IsToday = true;
                }
                else
                {
                    data.IsToday = false;
                }

                vacation.Add(data);
            }

            return vacation;
        }

        private VacationData VacationDataMapper(VacationDataViewModel vacationView)
        {
            return new VacationData()
            {
                VacationDate = vacationView.CalendarDate,
                UserID = vacationView.UserID,
                VacationTypeID = vacationView.VacationTypeID,
                RowVersion = vacationView.RowVersion
            };
        }
    }
}
