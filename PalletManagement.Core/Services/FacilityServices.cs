using MyAppTools.Helpers;
using PalletManagement.Core.Domain;
using PalletManagement.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectIV.Core.Services
{

    public interface IFacilityServices
    {
        int Add(Facility oFacility);
        int Update(Facility oFacility);
        Facility GetbyId(int FacilityId);
        IEnumerable<Facility> GetList();
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

        public IEnumerable<Facility> GetList()
        {
            return _facilityRepo.All;
        }
    }

}
