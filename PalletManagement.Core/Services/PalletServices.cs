using MyAppTools.Helpers;
using PalletManagement.Core.Domain;
using PalletManagement.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.OleDb;
using System.Linq;

namespace PalletManagement.Core.Services
{

    public interface IPalletServices
    {
        int Add(Pallet oPallet);
        int Add(List<Pallet> oPallets);
        int Update(Pallet oPallet);
        int Update(List<Pallet> oPallets);
        Pallet GetbyCode(string palletCode);
        Pallet GetbyId(int palletId);
        IQueryable<Pallet> GetList();
        object GetDisplayList(List<Pallet> oPallets);
        List<Pallet> ReadPalletsFromExcel(string filePath);
        int Attach(Pallet oPallet);
        IQueryable<Pallet> GetbyShipmentId(int shipmentId);
        //int SaveChanges();

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
        public int Attach(Pallet oPallet)
        {
            _palletRepo.Attach(oPallet);
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

        public Pallet GetbyCode(string palletCode)
        {
            return _palletRepo.All.FirstOrDefault(x => x.PalletCode == palletCode);
        }

        public IQueryable<Pallet> GetbyShipmentId(int shipmentId)
        {
            return this.GetList().Where(x => x.CurrentShipmentId == shipmentId);
        }
        public Pallet GetbyId(int palletId)
        {
            return _palletRepo.Find(palletId);
        }

        public IQueryable<Pallet> GetList()
        {
            return _palletRepo.All.AsNoTracking();
        }

        //public int SaveChanges()
        //{
        //    return unitOfWork.SaveChanges();
        //}

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

        public List<Pallet> ReadPalletsFromExcel(string filePath)
        {
            List<Pallet> formattedPalletList = null;
            DataTable dt = new DataTable();
            const string sql = "SELECT * from [Sheet1$]";
            var connectionString = $"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = {filePath}; Extended Properties = 'Excel 12.0;HDR=yes'";

            using (var conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new OleDbCommand(sql, conn))
                {
                    using (OleDbDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr == null) return null;
                        dt.Load(rdr);

                        var palletList = dt.AsEnumerable()
                            .Select(m => new Pallet()
                            {
                                PalletCode = m.Field<string>("PalletCode")
                            }).ToList();
                        if (palletList.Count() > 0)
                        {
                            formattedPalletList = new List<Pallet>();
                            foreach (var pallet in palletList)
                            {
                                pallet.PalletId = palletList.IndexOf(pallet) + 1;
                                formattedPalletList.Add(pallet);
                            }

                        }

                    }
                }
            }
            return formattedPalletList;
        }

    }

}
