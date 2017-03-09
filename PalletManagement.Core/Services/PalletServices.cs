using MyAppTools.Helpers;
using PalletManagement.Core.Domain;
using PalletManagement.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectIV.Core.Services
{

    public interface IPalletServices
    {
        int Add(Pallet oPallet);
        int Update(Pallet oPallet);
        Pallet GetbyCode(string PalletCode);
        Pallet GetbyId(int PalletId);
        IEnumerable<Pallet> GetListbyLocation(int FacilityId);
        IEnumerable<Pallet> GetList();
    }

    public class PalletServices : BaseService, IPalletServices
    {
        private readonly IRepository<Pallet> _palletRepo;
        public PalletServices()
        {
            _palletRepo = new BaseRepository<Pallet>(this.unitOfWork);
        }

        public int Add(Pallet oPallet)
        {
            _palletRepo.Add(oPallet);
            return this.unitOfWork.SaveChanges();
        }

        public int Update(Pallet oPallet)
        {
            var originalPallet = _palletRepo.Find(oPallet.PalletId);
            originalPallet.AssignedCustomer = oPallet.AssignedCustomer;
            originalPallet.CurrentLocation = oPallet.CurrentLocation;
            originalPallet.PalletCode = oPallet.PalletCode;
            originalPallet.PalletStatus = oPallet.PalletStatus;

            _palletRepo.Edit(originalPallet);

            return unitOfWork.SaveChanges();
        }

        public Pallet GetbyCode(string PalletCode)
        {
            return _palletRepo.All.Where(x => x.PalletCode == PalletCode).FirstOrDefault();
        }

        public Pallet GetbyId(int PalletId)
        {
            return _palletRepo.Find(PalletId);

        }

        public IEnumerable<Pallet> GetListbyLocation(int FacilityId)
        {
            return _palletRepo.All.Where(x => x.CurrentLocation.FacilityId == FacilityId).ToList();
        }

        public IEnumerable<Pallet> GetList()
        {
            return _palletRepo.All;
        }
    }

}
