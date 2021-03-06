﻿using MyAppTools.Helpers;
using PalletManagement.Core.Domain;
using PalletManagement.Core.Infrastructure;
using PalletManagement.Core.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
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
        int RepairPallet(Pallet oPallet);
        int Update(List<Pallet> oPallets);
        Pallet GetbyCode(string palletCode);
        Pallet GetbyId(int palletId);
        IQueryable<Pallet> GetList();
        object GetDisplayList(List<Pallet> oPallets);
        List<Pallet> ReadPalletsFromExcel(string filePath);
        int Attach(Pallet oPallet);
        IQueryable<Pallet> GetbyShipmentId(int shipmentId);
        List<PalletSummary> GetPalletSummary(int? customerId = null);
        List<Pallet> GetPalletList(string PalletCode = null, int? StatusId = null, int? customerID = null, int? facilityId = null);
        object GetPalletListDisplay(List<Pallet> oPallets);
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

        public int RepairPallet(Pallet oPallet)
        {
            var originalPallet = _palletRepo.Find(oPallet.PalletId);
            originalPallet.StatusId = oPallet.StatusId;
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
            return _palletRepo.All.AsNoTracking()
                .Include(x => x.CurrentLocation)
                .Include(x=>x.PalletStatus);
        }

        public List<Pallet> GetPalletList(string PalletCode = null, int? StatusId = null, int? customerID = null, int? facilityId = null)
        {
            var query = this.GetList();
            if (PalletCode != null)
                query = query.Where(x => x.PalletCode == PalletCode);

            if (StatusId != null)
            {

                if (StatusId == (int)PALLET_STATUS.Dormant)
                {
                    int dormantDays = int.Parse(ConfigurationManager.AppSettings["dormantDays"]);
                    //var d = DateTime.Now.AddDays(-dormantDays);
                    query = query.Where(x => x.LastMovementDate != null && DbFunctions.DiffDays(x.LastMovementDate, x.DateAdded) > dormantDays);
                   
                }
                else
                {
                    query = query.Where(x => x.StatusId == StatusId);
                }
            }

            if (customerID != null)
                query = query.Where(x => x.CurrentLocation.CustomerId == customerID);

            if (facilityId != null)
                query = query.Where(x => x.FacilityId == facilityId);

            
            return query.ToList();
        }

        public List<PalletSummary> GetPalletSummary(int? customerId = null)
        {
            List<PalletSummary> result = null;
            var all = this.GetList();
            if (customerId != null)
            {
                result = all.Where(x=>x.CurrentLocation.CustomerId == customerId)
                    .GroupBy(p => p.CurrentLocation.FacilityId)
                      .Select(g => new PalletSummary
                      {
                          FacilityId = g.Key,
                          Total = g.Count(),
                          Available = g.Count() - g.Where(x => x.StatusId != (int)PALLET_STATUS.Available).Count(),
                          Damaged = g.Where(x => x.StatusId == (int)PALLET_STATUS.Damaged).Count(),
                          Unaccounted = g.Where(x => x.StatusId == (int)PALLET_STATUS.Unaccounted).Count(), oCustomer = g.FirstOrDefault(x=>x.CurrentLocation.CustomerId == customerId).CurrentLocation.Customer
                      }).ToList();
            }
            else
            {
                result = all.GroupBy(p => p.CurrentLocation.CustomerId)
                     .Select(g => new PalletSummary
                     {
                         CustomerId = g.Key,
                         Total = g.Count(),
                         Available = g.Count() - g.Where(x => x.StatusId != (int)PALLET_STATUS.Available).Count(),
                         Damaged = g.Where(x => x.StatusId == (int)PALLET_STATUS.Damaged).Count(),
                         Unaccounted = g.Where(x => x.StatusId == (int)PALLET_STATUS.Unaccounted).Count(),
                         oFacility = g.FirstOrDefault(x => x.CurrentLocation.CustomerId == customerId).CurrentLocation
                     }).ToList();
            }


            return result;
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

        public object GetPalletListDisplay(List<Pallet> oPallets)
        {
            int serialNumber = 1;

            return oPallets.Select(i => new
            {
                id = serialNumber++,
                i.PalletId,
                i.PalletCode,
                Status = i.PalletStatus?.StatusName ?? "N/A",
                CurentLocation = i.CurrentLocation?.FacilityName ?? "N/A",
                i.LastMovementDate
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
