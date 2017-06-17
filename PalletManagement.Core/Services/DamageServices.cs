using MyAppTools.Helpers;
using PalletManagement.Core.Domain;
using PalletManagement.Core.Infrastructure;
using PalletManagement.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PalletManagement.Core.Services
{

    public interface IDamageServices
    {
        int Add(Damage oDamage);
        int Add(List<Damage> oPallets);
        int Update(Damage oDamage);
        int Delete(int DamageId);
        Damage GetbyId(int DamageId);
        IQueryable<Damage> GetList();
        object GetDisplayList(List<Damage> oDamage);
        List<DamageSummary> GetPalletSummary(DateTime? startDate = null, DateTime? endDate= null, int? customerID = null, int? facilityId = null);
        object GetDisplayList(List<DamageSummary> oDamage);
    }

    public class DamageServices : BaseService, IDamageServices
    {
        private readonly IRepository<Damage> _damageRepo;
        public DamageServices()
        {
            _damageRepo = new BaseRepository<Damage>(this.unitOfWork);
        }

        public int Add(Damage oDamage)
        {
            _damageRepo.Add(oDamage);
            return this.unitOfWork.SaveChanges();
        }

        public int Update(Damage oDamage)
        {
            var originalDamage = GetbyId(oDamage.DamageId);
            originalDamage.CollectionFormNo = oDamage.CollectionFormNo;
            //originalDamage.DamageLevelId = oDamage.DamageLevelId;
            originalDamage.DeckerboardsUsed = oDamage.DeckerboardsUsed;
            originalDamage.DP_TDP_Extracted = oDamage.DP_TDP_Extracted;
            originalDamage.IssueFormNo = oDamage.IssueFormNo;
            originalDamage.LastUpdatedDate = oDamage.LastUpdatedDate;
            originalDamage.DateRepaired = oDamage.DateRepaired;
            originalDamage.NailsUsed = oDamage.NailsUsed;
            originalDamage.Repaired = oDamage.Repaired;
            _damageRepo.Edit(originalDamage);

            return unitOfWork.SaveChanges();
        }

        public Damage GetbyId(int DamageId)
        {
            return
                _damageRepo.All.AsNoTracking()
                .FirstOrDefault(x => x.DamageId == DamageId);
        }

        public IQueryable<Damage> GetList()
        {
            return _damageRepo.All.AsNoTracking()
                .Include(x => x.Facility)
                .Include(x => x.DamageLevel)
                .Include(x => x.DamagedPallet);
        }

        public int Delete(int DamageId)
        {
            var oDamage = GetbyId(DamageId);
            _damageRepo.Delete(oDamage);
            return unitOfWork.SaveChanges();
        }

        public int Add(List<Damage> oDamage)
        {
            foreach (var damage in oDamage)
            {
                _damageRepo.Add(damage);
            }
            return this.unitOfWork.SaveChanges();
        }

        public object GetDisplayList(List<Damage> oDamage)
        {
            int serialNumber = 1;

            return oDamage.Select(i => new
            {
                id = serialNumber++,
                i.DateAdded,
                i.CollectionFormNo,
                PalletCode = i.DamagedPallet?.PalletCode ?? "N/A",
                i.DamagedPalletId,
                i.DamageId,
                DamageLevelName = i.DamageLevel?.DamageLevelName ?? "N/A",
                i.DamageLevelId,
                i.DateRepaired,
                i.DeckerboardsUsed,
                i.DP_TDP_Extracted,
                i.facilityId,
                FacilityName = i.Facility?.FacilityName ?? "N/A",
                CustomerName = i.Facility?.Customer?.CustomerName ?? "N/A",
                i.IssueFormNo,
                i.LastUpdatedDate,
                i.NailsUsed,
                i.Reason,
                i.Repaired,
                HasBeenRepaired = i.Repaired? "Yes" : "No"
            });
        }

      public  List<DamageSummary> GetPalletSummary(DateTime? startDate = null, DateTime? endDate = null, int? customerID = null, int? facilityId = null)
        {
            List<DamageSummary> result = null;
            var query = this.GetList();
            if (startDate != null)
                query = query.Where(x => x.DateAdded >= startDate);

            if (endDate != null)
                query = query.Where(x => x.DateAdded <= endDate);

            if (customerID != null)
                query = query.Where(x =>  x.Facility.CustomerId == customerID);

            if (facilityId != null)
                query = query.Where(x => x.facilityId == facilityId);

            result = query.GroupBy(p => p.facilityId)
           .Select(g => new DamageSummary
           {
               FacilityId = g.Key,
               oCustomer =  g.FirstOrDefault().Facility.Customer,
               Damaged = g.Count(),
               TotallyDamaged = g.Where(x => x.DamageLevelId == (int)DAMAGE_LEVEL.Total_Damage).Count(),
               Repairable = g.Where(x => x.DamageLevelId == (int)DAMAGE_LEVEL.Repairable).Count(),
               Repaired = g.Where(x => x.Repaired == true).Count(),
               DeckerboardsUsed = g.Sum(x => x.DeckerboardsUsed),
               DP_TDP_Extracted = g.Sum(x => x.DP_TDP_Extracted),
               NailsUsed = g.Sum(x => x.NailsUsed),
               oFacility = g.FirstOrDefault().Facility
           }).ToList();

            return result;
        }

        public object GetDisplayList(List<DamageSummary> oDamage)
        {
            int serialNumber = 1;

            return oDamage.Select(i => new
            {
                id = serialNumber++,
                i.CustomerId,
                i.Damaged,
                i.DeckerboardsUsed,
                i.DP_TDP_Extracted,
                i.FacilityId,
                i.NailsUsed,
                CustomerName = i.oCustomer?.CustomerName ?? "N/A",
                FacilityName = i.oFacility?.FacilityName ?? "N/A",
                i.Repairable,
                i.Repaired,
                i.TotallyDamaged,
            });
        }
    }

}
