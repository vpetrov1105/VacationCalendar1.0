using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationCalendar.Api.EntityModels;
using VacationCalendar.Api.Helpers;
using VacationCalendar.Api.Infrastructure;
using VacationCalendar.Api.Validators;
using VacationCalendar.Api.ViewModels;

namespace VacationCalendar.Api.Services
{
    public interface IVacationDataService
    {
        ResponseViewModel<List<VacationDataViewModel>> DeleteVacation(VacationDataViewModel vacation, LoginViewModel loggedUser);
        ResponseViewModel<List<VacationDataViewModel>> UpdateVacation(List<VacationDataViewModel> vacations, LoginViewModel loggedUser);
        List<UserViewModel> GetUsersData(int year, int month);
    }
    public class VacationDataService : IVacationDataService
    {
        private readonly VacationCalendarContext _context;
        private readonly VacationDataValidator _validator;

        public VacationDataService(VacationCalendarContext context, VacationDataValidator validator)
        {
            _context = context;
            _validator = validator;
        }

        public ResponseViewModel<List<VacationDataViewModel>> DeleteVacation(VacationDataViewModel vacation, LoginViewModel loggedUser)
        {
            var response = new ResponseViewModel<List<VacationDataViewModel>>();
            
            response = _validator.IsValidForDelete(vacation, loggedUser);
            if (!response.Success)
            {
                return response;
            }

            try
            {
                var vacationToUpdate = _context.VacationData
                            .Where(c => c.ID == vacation.Id)
                            .SingleOrDefault();

                if (vacationToUpdate == null)
                {
                    throw new CustomException(string.Format("Meanwhile the vacation was deleted by another user!"));
                }

                _context.VacationData.Remove(_context.VacationData.Find(vacation.Id));

                _context.SaveChanges();

                response.Success = true;
            }

            catch (Exception e)
            {
                response.Success = false;
                response.ResponseMessage.Add(ApplicationConstants.DELETE_ERROR + " - " + e.Message);
                response.ReturnedObject = GetVacationForUser(vacation.CalendarDate.Year,
                                                vacation.CalendarDate.Month,
                                                vacation.UserID);
                return response;
            }

            response.Success = true;
            response.ResponseMessage.Add(ApplicationConstants.DELETE_SUCCESS);
            response.ReturnedObject = GetVacationForUser(vacation.CalendarDate.Year,
                                                    vacation.CalendarDate.Month,
                                                    vacation.UserID);
            return response;
        }

        public ResponseViewModel<List<VacationDataViewModel>> UpdateVacation(List<VacationDataViewModel> vacations, LoginViewModel loggedUser)
        {
            var response = new ResponseViewModel<List<VacationDataViewModel>>();
            var processVacations = new List<VacationDataViewModel>();
            var vacationToUpdate = new VacationData();

            response = _validator.IsValidForUpdate(vacations, loggedUser);
            if (!response.Success)
            {
                return response;
            }
            try
            {
                processVacations = vacations;
                foreach (var vacation in processVacations)
                {

                    //Check if data is for insert of update
                    var vacationDB = _context.VacationData
                    .Where(c => c.VacationDate == vacation.CalendarDate && c.UserID == vacation.UserID)
                    .SingleOrDefault();

                    if (vacationDB == null && vacation.Id == default(int))
                    {
                        var dataForInsert = VacationDataMapper(vacation);
                        _context.VacationData.Add(dataForInsert);
                    }
                    else
                    {
                        //Check if vacation record is deleted meanwhile
                        vacationToUpdate = _context.VacationData
                            .Where(c => c.ID == vacation.Id)
                            .SingleOrDefault();

                        if (vacationToUpdate == null)
                        {
                            throw new CustomException(string.Format("The vacation was deleted by another user. Click again for insert!"));
                        }

                        _context.Entry(vacationToUpdate).Property("RowVersion").OriginalValue = vacation.RowVersion;

                        //change values for update
                        vacationToUpdate.VacationTypeID = vacation.VacationTypeID;
                        _context.Update(vacationToUpdate).State = EntityState.Modified;
                    }
                }
                try
                {
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var exceptionEntry = ex.Entries.Single();
                    var clientValues = (VacationData)exceptionEntry.Entity;
                    var databaseEntry = exceptionEntry.GetDatabaseValues();

                    if (databaseEntry == null)
                    {
                        throw new CustomException(string.Format("The vacation was deleted by another user. Click again insert!"));
                    }
                    else
                    {
                        var databaseValues = (VacationData)databaseEntry.ToObject();
                        if (databaseValues.VacationTypeID != clientValues.VacationTypeID)
                        {
                            throw new CustomException(string.Format("The Vacation Type is changed by another user! Click again for update!"));
                        }

                        throw new CustomException(string.Format("Some data changed meanwhile! Click again for update!"));
                    }
                }
            }
            catch (Exception e)
            {
                _context.Entry(vacationToUpdate).State = EntityState.Detached;

                response.Success = false;
                response.ResponseMessage.Add(ApplicationConstants.UPDATE_ERROR + " - " + e.Message);
                response.ReturnedObject = GetVacationForUser(vacations.FirstOrDefault().CalendarDate.Year,
                                                vacations.FirstOrDefault().CalendarDate.Month,
                                                vacations.FirstOrDefault().UserID);
                return response;
            }

            response.Success = true;
            response.ResponseMessage.Add(ApplicationConstants.UPDATE_SUCCESS);
            response.ReturnedObject = GetVacationForUser(vacations.FirstOrDefault().CalendarDate.Year,
                                                    vacations.FirstOrDefault().CalendarDate.Month,
                                                    vacations.FirstOrDefault().UserID);
            return response;
        }

        public List<UserViewModel> GetUsersData(int year, int month)
        {
            List<UserViewModel> users = new List<UserViewModel>();
            var dbUsersData = _context.Users.Include(user => user.VacationData).AsNoTracking().ToList();
            foreach (var user in dbUsersData)
            {
                var data = new UserViewModel();
                data.Id = user.ID;
                data.FirstName = user.FirstName;
                data.LastName = user.LastName;
                data.VacationData = GetVacationForUser(year, month, data.Id);

                users.Add(data);
            }
            return users;
        }

        private List<VacationDataViewModel> GetVacationForUser(int year, int month, int userId)
        {
            List<VacationData> vacationDB = new List<VacationData>();
            List<VacationDataViewModel> vacation = new List<VacationDataViewModel>();

            vacationDB = _context.VacationData.Where(x => x.UserID == userId).Select(row => row).ToList();

            int monthDays = DateTime.DaysInMonth(year, month);
            for (int day = 1; day <= monthDays; day++)
            {
                var data = new VacationDataViewModel();
                data.Day = day;
                data.CalendarDate = new DateTime(year, month, day);
                data.Id = vacationDB.Where(x => x.VacationDate == data.CalendarDate).Select(x => x.ID).FirstOrDefault();
                data.IsOnVacation = vacationDB.Any(x => x.ID == data.Id);
                data.DayName = data.CalendarDate.DayOfWeek.ToString().Substring(0, 3);
                data.VacationTypeID = vacationDB.Where(x => x.ID == data.Id).Select(x => x.VacationTypeID).FirstOrDefault();
                data.UserID = userId;
                data.RowVersion = vacationDB.Where(x => x.ID == data.Id).Select(x => x.RowVersion).FirstOrDefault();

                vacation.Add(data);
            }

            return vacation;
        }

        private VacationData VacationDataMapper(VacationDataViewModel vacationView)
        {
            var vacation = new VacationData()
            {
                VacationDate = vacationView.CalendarDate,
                UserID = vacationView.UserID,
                VacationTypeID = vacationView.VacationTypeID,
                RowVersion = vacationView.RowVersion
            };
            return vacation;
        }
    }
}
