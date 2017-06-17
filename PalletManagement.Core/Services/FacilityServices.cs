using MyAppTools.Helpers;
using PalletManagement.Core.Domain;
using PalletManagement.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PalletManagement.Core.Services
{

    public interface IFacilityServices
    {
        int Add(Facility oFacility);
        int Update(Facility oFacility);
        int Delete(int FacilityId);
        Facility GetbyId(int FacilityId);
        IQueryable<Facility> GetList();
    }

    public class FacilityServices : BaseService, IFacilityServices
    {
        private readonly IRepository<Facility> _facilityRepo;
        public FacilityServices()
        {
            _facilityRepo = new BaseRepository<Facility>(this.unitOfWork);
        }

        public int Add(Facility oFacility)
        {
            _facilityRepo.Add(oFacility);
            return this.unitOfWork.SaveChanges();
        }

        public int Update(Facility oFacility)
        {
            var originalFacility = _facilityRepo.Find(oFacility.FacilityId);
            originalFacility.FacilityName = oFacility.FacilityName;
            originalFacility.FacilityType = oFacility.FacilityType;

            _facilityRepo.Edit(originalFacility);

            return unitOfWork.SaveChanges();
        }
     
        public Facility GetbyId(int FacilityId)
        {
            return _facilityRepo.Find(FacilityId);

        }

        public IQueryable<Facility> GetList()
        {
            return _facilityRepo.All.AsNoTracking()
                .Include(x =>x.Customer);
        }

        public int Delete(int FacilityId)
        {
            var oFacility = GetbyId(FacilityId);
            _facilityRepo.Delete(oFacility);
            return unitOfWork.SaveChanges();
        }
    }

}
