using MyAppTools.Helpers;
using PalletManagement.Core.Domain;
using PalletManagement.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PalletManagement.Core.Services
{

    public interface IPalletServices
    {
        int Add(Pallet oPallet);
        int Add(List<Pallet> oPallets);
        int Update(Pallet oPallet);
        int Update(List<Pallet> oPallets);
        Pallet GetbyCode(string PalletCode);
        Pallet GetbyId(int PalletId);
        IQueryable<Pallet> GetList();
        object GetDisplayList(List<Pallet> oPallets);

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

        public int Add(List<Pallet> oPallets)
        {
            foreach (var oPallet in oPallets)
            {
                _palletRepo.Add(oPallet);
            }
            return this.unitOfWork.SaveChanges();

        }
        public int Update(Pallet oPallet)
        {
            var originalPallet = _palletRepo.Find(oPallet.PalletId);
            originalPallet.FacilityId = oPallet.FacilityId;
            originalPallet.PalletCode = oPallet.PalletCode;
            originalPallet.StatusId = oPallet.StatusId;
            originalPallet.CurrentShipmentId = oPallet.CurrentShipmentId;
            originalPallet.LastMovementDate = oPallet.LastMovementDate;
            originalPallet.LastUpdatedDate = DateTime.Now;
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

        public IQueryable<Pallet> GetList()
        {
            return _palletRepo.All.AsNoTracking();
        }

        public object GetDisplayList(List<Pallet> oPallets)
        {
            return oPallets.Select(i => new
            {
                i.PalletId,
                i.PalletCode,
                StatusName = i.PalletStatus?.StatusName ?? "N/A",
                FacilityName = i.CurrentLocation?.FacilityName ?? "N/A"
            });
        }

        public int Update(List<Pallet> oPallets)
        {
            foreach (var oPallet in oPallets)
            {
                var originalPallet = _palletRepo.Find(oPallet.PalletId);
                originalPallet.FacilityId = oPallet.FacilityId;
                originalPallet.PalletCode = oPallet.PalletCode;
                originalPallet.StatusId = oPallet.StatusId;
                originalPallet.LastMovementDate = oPallet.LastMovementDate;
                originalPallet.LastUpdatedDate = DateTime.Now;
                originalPallet.CurrentShipmentId = oPallet.CurrentShipmentId;
                _palletRepo.Edit(originalPallet);
            }
            return unitOfWork.SaveChanges();
        }
    }

}
