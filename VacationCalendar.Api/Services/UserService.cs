using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationCalendar.Api.EntityModels;
using VacationCalendar.Api.Infrastructure;
using VacationCalendar.Api.Services.Interfaces;
using VacationCalendar.Api.Validators;
using VacationCalendar.Api.ViewModels;

namespace VacationCalendar.Api.Services
{
    public class UserService : IUserService
    {
        private readonly VacationCalendarContext _context;
        private readonly UserDataValidator _validator;
        public UserService(VacationCalendarContext context, UserDataValidator validator)
        {
            _context = context;
            _validator = validator;
        }

        public Task<List<UserViewModel>> GetUsersDataAsync()
        {
            return Task.Factory.StartNew(() =>
            {
                List<UserViewModel> users = new List<UserViewModel>();
                var dbUsersData = _context.Users.ToList();
                foreach (var user in dbUsersData)
                {
                    var data = new UserViewModel();
                    data.Id = user.ID;
                    data.FirstName = user.FirstName;
                    data.LastName = user.LastName;
                    data.BirthDate = user.BirthDate;
                    data.UserName = user.UserName;
                    data.Role = user.Role;
                    data.OfficeCountryCode = user.OfficeCountryCode;
                    data.RowVersion = user.RowVersion;

                    users.Add(data);
                }
                return users;
            });
        }

        public Task<List<UserViewModel>> GetUserByIdDataAsync(int id)
        {
            return Task.Factory.StartNew(() =>
            {
                List<UserViewModel> users = new List<UserViewModel>();
                var user = _context.Users.SingleOrDefault(x => x.ID == id);

                var data = new UserViewModel();
                data.Id = user.ID;
                data.FirstName = user.FirstName;
                data.LastName = user.LastName;
                data.BirthDate = user.BirthDate;
                data.UserName = user.UserName;
                data.Role = user.Role;
                data.OfficeCountryCode = user.OfficeCountryCode;
                data.RowVersion = user.RowVersion;

                users.Add(data);
                return users;
            });
        }

        public Task<ResponseViewModel<UserViewModel>> DeleteUserDataAsync(int id, LoginViewModel loggedUser)
        {
            return Task.Factory.StartNew(() =>
            {
                var response = new ResponseViewModel<UserViewModel>();

                response = _validator.IsValidForDelete(id, loggedUser);
                if (!response.Success)
                {
                    return response;
                }

                try
                {
                    var UserToDelete = _context.Users
                                .SingleOrDefault(c => c.ID == id);

                    if (UserToDelete == null)
                    {
                        throw new Exception(string.Format("Meanwhile the user data was deleted by another user!"));
                    }

                    _context.Users.Remove(UserToDelete);

                    _context.SaveChanges();

                    response.Success = true;
                    response.ResponseMessages.Add(ApplicationConstants.DELETE_SUCCESS);
                    
                    return response;
                }
                catch (Exception e)
                {
                    response.Success = false;
                    response.ResponseMessages.Add(ApplicationConstants.DELETE_ERROR + " - " + e.Message);
                    
                    return response;
                }
            });
        }

        public Task<ResponseViewModel<UserViewModel>> InsertUserDataAsync(UserViewModel userData, LoginViewModel loggedUser)
        {
            return Task.Factory.StartNew(() =>
            {
                var response = new ResponseViewModel<UserViewModel>();
                var userToInsert = new User();

                response = _validator.IsValidForInsert(userData, loggedUser, _context);
                if (!response.Success)
                {
                    return response;
                }
                try
                {
                    userToInsert.FirstName = userData.FirstName;
                    userToInsert.LastName = userData.LastName;
                    userToInsert.BirthDate = userData.BirthDate;
                    userToInsert.UserName = userData.UserName;
                    userToInsert.Password = userData.UserName;
                    userToInsert.OfficeCountryCode = userData.OfficeCountryCode;
                    userToInsert.Role = userData.Role;

                    _context.Add(userToInsert);

                    _context.SaveChanges();


                }
               
                catch (Exception e)
                {
                    response.Success = false;
                    response.ResponseMessages.Add(ApplicationConstants.UPDATE_ERROR + " - " + e.Message);
                    response.ReturnedObject = MapUserToViewModel(userToInsert);
                    return response;
                }
                response.Success = true;
                response.ResponseMessages.Add(ApplicationConstants.UPDATE_SUCCESS);
                response.ReturnedObject = MapUserToViewModel(userToInsert);

                return response;
            });
        }

        public Task<ResponseViewModel<UserViewModel>> UpdateUserDataAsync(UserViewModel userData, LoginViewModel loggedUser)
        {
            return Task.Factory.StartNew(() =>
            {
                var response = new ResponseViewModel<UserViewModel>();
                var userToUpdate = new User();

                response = _validator.IsValidForUpdate(userData, loggedUser);
                if (!response.Success)
                {
                    return response;
                }
                try
                {
                    //Check if vacation record is deleted meanwhile
                    userToUpdate = _context.Users
                        .SingleOrDefault(c => c.ID == userData.Id);

                    if (userToUpdate == null)
                    {
                        throw new Exception("The user data was deleted by administrator!");
                    }

                    // throws concurrency exception on save changes if item is already modified by another request
                    _context.Entry(userToUpdate).Property("RowVersion").OriginalValue = userData.RowVersion;

                    //change values for update
                    userToUpdate.FirstName = userData.FirstName;
                    userToUpdate.LastName = userData.LastName;
                    userToUpdate.BirthDate = userData.BirthDate;
                    userToUpdate.UserName = userData.UserName;
                    userToUpdate.OfficeCountryCode = userData.OfficeCountryCode;

                    if(loggedUser.Role == Role.Admin)
                    {
                        userToUpdate.Role = userData.Role;
                    } 

                    _context.Update(userToUpdate).State = EntityState.Modified;

                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    string errorMessage;
                    var exceptionEntry = ex.Entries.First();
                    var clientValues = (User)exceptionEntry.Entity;
                    var databaseEntry = exceptionEntry.GetDatabaseValues();

                    errorMessage = "Some data changed meanwhile! Click again for update!";

                    if (databaseEntry == null)
                    {
                        errorMessage = "The user data was deleted by administrator!";
                    }
                    else
                    {
                        var databaseValues = (User)databaseEntry.ToObject();

                        if (databaseValues.FirstName != clientValues.FirstName)
                        {
                            errorMessage = "The FirstName is changed by another user! Click again for update!";
                        }
                        if (databaseValues.LastName != clientValues.LastName)
                        {
                            errorMessage = "The LastName is changed by another user! Click again for update!";
                        }
                        if (databaseValues.BirthDate != clientValues.BirthDate)
                        {
                            errorMessage = "The BirthDate is changed by another user! Click again for update!";
                        }
                        if (databaseValues.UserName != clientValues.UserName)
                        {
                            errorMessage = "The UserName is changed by another user! Click again for update!";
                        }
                        if (databaseValues.Role != clientValues.Role)
                        {
                            errorMessage = "The Role is changed by another user! Click again for update!";
                        }
                        if (databaseValues.OfficeCountryCode != clientValues.OfficeCountryCode)
                        {
                            errorMessage = "The OfficeCountryCode is changed by another user! Click again for update!";
                        }

                    }  

                    _context.Entry(userToUpdate).State = EntityState.Detached;

                    response.Success = false;
                    response.ResponseMessages.Add(ApplicationConstants.UPDATE_ERROR + " - " + errorMessage);
                    response.ReturnedObject = GetDataById(userData.Id);
                    return response;
                }
                catch (Exception e)
                {
                    response.Success = false;
                    response.ResponseMessages.Add(ApplicationConstants.UPDATE_ERROR + " - " + e.Message);
                    response.ReturnedObject = GetDataById(userData.Id);
                    return response;
                }
                response.Success = true;
                response.ResponseMessages.Add(ApplicationConstants.UPDATE_SUCCESS);
                response.ReturnedObject = GetDataById(userData.Id);

                return response;
            });
        }

        private UserViewModel GetDataById(int id)
        {
            var userDB = _context.Users.SingleOrDefault(c => c.ID == id);
            return new UserViewModel()
            {
                Id = userDB.ID,
                FirstName = userDB.FirstName,
                LastName = userDB.LastName,
                BirthDate = userDB.BirthDate,
                UserName = userDB.UserName,
                Role = userDB.Role,
                OfficeCountryCode = userDB.OfficeCountryCode,
                RowVersion = userDB.RowVersion

            };
        }

        private UserViewModel MapUserToViewModel(User user)
        {
            return new UserViewModel()
            {
                Id = user.ID,
                FirstName = user.FirstName,
                LastName = user.LastName,
                BirthDate = user.BirthDate,
                UserName = user.UserName,
                Role = user.Role,
                OfficeCountryCode = user.OfficeCountryCode,
                RowVersion = user.RowVersion

            };
        }
    }
}
