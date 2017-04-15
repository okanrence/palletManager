using MyAppTools.Helpers;
using PalletManagement.Core.Domain;
using PalletManagement.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PalletManagement.Core.Services
{

    public interface IUserServices
    {
        int Add(User oUser);
        int Update(User oUser);
        int Delete(int userId);
        bool UpdatePassword(int userId, string newPassword);
        User GetbyId(int userId);
        User Authenticate(string emailAddress, string password);
        IQueryable<User> GetList();
        object GetDisplayList(List<User> oUsers);
    }

    public class UserServices : BaseService, IUserServices
    {
        private readonly IRepository<User> _UserRepo;
        public UserServices()
        {
            _UserRepo = new BaseRepository<User>(this.unitOfWork);
        }

        public int Add(User oUser)
        {
            _UserRepo.Add(oUser);
            return this.unitOfWork.SaveChanges();
        }

        public int Update(User oUser)
        {
            var originalUser = GetbyId(oUser.UserId);
            originalUser.DateDeactivated = oUser.DateDeactivated;
            originalUser.FirstName = oUser.FirstName;
            originalUser.EmailAddress = oUser.EmailAddress;
            originalUser.PhoneNumber = oUser.PhoneNumber;
            originalUser.LastName = oUser.LastName;
            originalUser.LastUpdatedDate = DateTime.Now;
            originalUser.ProfileStatus = oUser.ProfileStatus;
            originalUser.StaffNumber = oUser.StaffNumber;
            originalUser.UserRoleId = oUser.UserRoleId;
            originalUser.AssignedFacilityId = oUser.AssignedFacilityId;
            originalUser.LastLoginDate = oUser.LastLoginDate;

            _UserRepo.Edit(originalUser);

            return unitOfWork.SaveChanges();
        }

        public User GetbyId(int UserId)
        {
            return
               GetList().Where(x => x.UserId == UserId)
                .FirstOrDefault();
        }

        public IQueryable<User> GetList()
        {
            return _UserRepo.All.AsNoTracking();
        }

        public int Delete(int UserId)
        {
            var oUser = GetbyId(UserId);
            _UserRepo.Delete(oUser);
            return unitOfWork.SaveChanges();
        }

        public object GetDisplayList(List<User> oUsers)
        {
            return oUsers.Select(i => new
            {
                i.DateAdded,
                i.DateDeactivated,
                i.EmailAddress,
                i.FullName,
                i.PhoneNumber,
                i.ProfileStatus,
                i.StaffNumber,
                i.UserId,
                i.UserRoleId,
                i.UserRole.UserRoleName,
                AssignedFacility = i.AssignedFacility?.FacilityName ?? "N/A",
                AssignedCustomer = i.AssignedFacility?.Customer?.CustomerName ?? "N/A"

            });
        }
        public User Authenticate(string emailAddress, string password)
        {
            return GetList().Where(x => x.EmailAddress == emailAddress && x.Password == password).FirstOrDefault();
        }

        public bool UpdatePassword(int userId, string newPassword)
        {
            var originalUser = GetbyId(userId);
            originalUser.LastUpdatedDate = DateTime.Now;
            originalUser.Password = newPassword;
            originalUser.MustChangePassword = false;

            _UserRepo.Edit(originalUser);

            return unitOfWork.SaveChanges() > 0;
        }
    }

}
