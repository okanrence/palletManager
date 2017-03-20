using MyAppTools.Helpers;
using PalletManagement.Core.Domain;
using PalletManagement.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PalletManagement.Core.Services
{

    public interface IUserRoleServices
    {
        int Add(UserRole oUserRole);
        int Update(UserRole oUserRole);
        int Delete(int userId);
        UserRole GetbyId(int userId);
        IQueryable<UserRole> GetList();
    }

    public class UserRoleServices : BaseService, IUserRoleServices
    {
        private readonly IRepository<UserRole> _userRoleRepo;
        public UserRoleServices()
        {
            _userRoleRepo = new BaseRepository<UserRole>(this.unitOfWork);
        }

        public int Add(UserRole oUserRole)
        {
            _userRoleRepo.Add(oUserRole);
            return this.unitOfWork.SaveChanges();
        }

        public int Update(UserRole oUserRole)
        {
            var originalUser = GetbyId(oUserRole.UserRoleId);
            originalUser.LastUpdatedDate = oUserRole.LastUpdatedDate;
            originalUser.UserRoleName = oUserRole.UserRoleName;
          
            _userRoleRepo.Edit(originalUser);

            return unitOfWork.SaveChanges();
        }
     
        public UserRole GetbyId(int userRoleId)
        {
            return
               GetList().Where(x => x.UserRoleId == userRoleId)
                .FirstOrDefault();
        }

        public IQueryable<UserRole> GetList()
        {
            return _userRoleRepo.All.AsNoTracking();
        }

        public int Delete(int UserId)
        {
            var oUserRole = GetbyId(UserId);
            _userRoleRepo.Delete(oUserRole);
            return unitOfWork.SaveChanges();
        }

      

    }

}
