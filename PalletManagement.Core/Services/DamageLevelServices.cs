using MyAppTools.Helpers;
using PalletManagement.Core.Domain;
using PalletManagement.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PalletManagement.Core.Services
{

    public interface IDamageLevelServices
    {
        int Add(DamageLevel oDamageLevel);
        int Update(DamageLevel oDamageLevel);
        int Delete(int DamageLevelId);
        DamageLevel GetbyId(int DamageLevelId);
        IQueryable<DamageLevel> GetList();
    }

    public class DamageLevelServices : BaseService, IDamageLevelServices
    {
        private readonly IRepository<DamageLevel> _DamageLevelRepo;
        public DamageLevelServices()
        {
            _DamageLevelRepo = new BaseRepository<DamageLevel>(this.unitOfWork);
        }

        public int Add(DamageLevel oDamageLevel)
        {
            _DamageLevelRepo.Add(oDamageLevel);
            return this.unitOfWork.SaveChanges();
        }

        public int Update(DamageLevel oDamageLevel)
        {
            var originalDamageLevel = GetbyId(oDamageLevel.DamageLevelId);
            originalDamageLevel.DamageLevelName = oDamageLevel.DamageLevelName;
            _DamageLevelRepo.Edit(originalDamageLevel);

            return unitOfWork.SaveChanges();
        }
     
        public DamageLevel GetbyId(int DamageLevelId)
        {
            return
                _DamageLevelRepo.All.AsNoTracking()
                .FirstOrDefault(x => x.DamageLevelId == DamageLevelId);
        }

        public IQueryable<DamageLevel> GetList()
        {
            return _DamageLevelRepo.All.AsNoTracking();
        }

        public int Delete(int DamageLevelId)
        {
            var oDamageLevel = GetbyId(DamageLevelId);
            _DamageLevelRepo.Delete(oDamageLevel);
            return unitOfWork.SaveChanges();
        }
    }

}
