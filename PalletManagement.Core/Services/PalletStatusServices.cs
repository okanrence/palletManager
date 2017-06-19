using MyAppTools.Helpers;
using PalletManagement.Core.Domain;
using PalletManagement.Core.Infrastructure;
using PalletManagement.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.OleDb;
using System.Linq;

namespace PalletManagement.Core.Services
{

    public interface IPalletStatusServices
    {
        int Add(PalletStatus oPalletStatus);
        int Update(PalletStatus oPalletStatus);
        PalletStatus GetbyId(int PalletStatusId);
        IQueryable<PalletStatus> GetList();
    }

    public class PalletStatusServices : BaseService, IPalletStatusServices
    {
        private readonly IRepository<PalletStatus> _PalletStatusRepo;
        public PalletStatusServices()
        {
            _PalletStatusRepo = new BaseRepository<PalletStatus>(this.unitOfWork);
        }

        public int Add(PalletStatus oPalletStatus)
        {
            _PalletStatusRepo.Add(oPalletStatus);
            return this.unitOfWork.SaveChanges();
        }
        public int Attach(PalletStatus oPalletStatus)
        {
            _PalletStatusRepo.Attach(oPalletStatus);
            return this.unitOfWork.SaveChanges();
        }
        public int Add(List<PalletStatus> oPalletStatuss)
        {
            foreach (var oPalletStatus in oPalletStatuss)
            {
                _PalletStatusRepo.Add(oPalletStatus);
            }
            return this.unitOfWork.SaveChanges();
        }

        public int Update(PalletStatus oPalletStatus)
        {
            var originalPalletStatus = _PalletStatusRepo.Find(oPalletStatus.StatusId);
            originalPalletStatus.StatusName = oPalletStatus.StatusName;
            originalPalletStatus.LastUpdatedDate = DateTime.Now;
            _PalletStatusRepo.Edit(originalPalletStatus);
            return unitOfWork.SaveChanges();
        }
        public PalletStatus GetbyId(int PalletStatusId)
        {
            return _PalletStatusRepo.Find(PalletStatusId);
        }

        public IQueryable<PalletStatus> GetList()
        {
            return _PalletStatusRepo.All.AsNoTracking();
        }


    }

}
