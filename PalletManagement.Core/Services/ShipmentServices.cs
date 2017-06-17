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

    public interface IShipmentServices
    {
        int Add(Shipment oShipment);
        int Add(List<Shipment> oShipments);
        int Update(Shipment oShipment);
        int Delete(int shipmentId);
        Shipment GetbyCode(string ShipmentCode);
        Shipment GetbyId(int ShipmentId);
        IQueryable<Shipment> GetList();
        object GetDisplayList(List<Shipment> oShipments);
        string GetShipmentNumber(string source);
        List<ShipmentSummary> GetPalletSummary(DateTime? startDate = null, DateTime? endDate = null, int? customerID = null, int? SourcefacilityId = null, bool groupbyFacility = false);
        object GetDisplayList(List<ShipmentSummary> oShipments);
        int SaveChanges();
    }

    public class ShipmentServices : BaseService, IShipmentServices
    {
        private readonly IRepository<Shipment> _shipmentRepo;
        public ShipmentServices()
        {
            _shipmentRepo = new BaseRepository<Shipment>(this.unitOfWork);
        }

        public int Add(Shipment oShipment)
        {

            _shipmentRepo.Add(oShipment);
            return this.unitOfWork.SaveChanges();
        }

        public int Add(List<Shipment> oShipments)
        {
            foreach (var oShipment in oShipments)
            {
                _shipmentRepo.Add(oShipment);
            }
            return this.unitOfWork.SaveChanges();

        }
        public int Delete(int shipmentId)
        {
            var oShipment = GetbyId(shipmentId);
            _shipmentRepo.Delete(oShipment);
            return unitOfWork.SaveChanges();
        }
        public int Update(Shipment oShipment)
        {
            var originalShipment = _shipmentRepo.Find(oShipment.ShipmentId);
            originalShipment.OutTrackerId = oShipment.OutTrackerId;
            originalShipment.InTrackerId = oShipment.InTrackerId;
            originalShipment.ShipmentDestinationId = oShipment.ShipmentDestinationId;
            originalShipment.ShipmentSourceId = oShipment.ShipmentSourceId;
            originalShipment.SourceDateTime = oShipment.SourceDateTime;
            originalShipment.DestinationDateTime = oShipment.DestinationDateTime;
            originalShipment.LastUpdatedDate = oShipment.LastUpdatedDate;
            originalShipment.NoOfPalletsOut = oShipment.NoOfPalletsOut;
            originalShipment.TruckNumber = oShipment.TruckNumber;
            originalShipment.PalletList = oShipment.PalletList;
            originalShipment.IsCompleted = oShipment.IsCompleted;
            originalShipment.NoOfPalletsIn = oShipment.NoOfPalletsIn;


            _shipmentRepo.Edit(originalShipment);

            return unitOfWork.SaveChanges();
        }

        public Shipment GetbyCode(string ShipmentNumber)
        {
            return _shipmentRepo.All.Where(x => x.ShipmentNumber == ShipmentNumber).FirstOrDefault();
        }

        public Shipment GetbyId(int ShipmentId)
        {
            return _shipmentRepo.Find(ShipmentId);
        }

        public IQueryable<Shipment> GetList()
        {
            return _shipmentRepo.All.AsNoTracking()
                .Include(x => x.Customer)
                .Include(x => x.ShipmentSource)
                .Include(x => x.ShipmentDestination)
                .Include(x => x.ShipmentStatus);
        }

        public object GetDisplayList(List<Shipment> oShipments)
        {
            int serialNumber = 1;
            return oShipments.Select(i => new
            {
                id = serialNumber++,
                i.ShipmentId,
                i.DestinationDateTime,
                i.SourceDateTime,
                InTracker = i.InTracker?.FullName ?? "N/A",
                OutTracker = i.OutTracker?.FullName ?? "N/A",
                Destination = i.ShipmentDestination?.FacilityName ?? "N/A",
                i.ShipmentNumber,
                Source = i.ShipmentSource?.FacilityName ?? "N/A",
                ShipmentStatus = i.ShipmentStatus?.ShipmentStatusName ?? "N/A",
                TruckNumber = i.TruckNumber,
                i.NoOfPalletsOut,
                i.IsCompleted,
                i.NoOfPalletsIn
            });
        }
        public string GetShipmentNumber(string source)
        {
            return $"{source.Substring(0, 2).ToUpper()}/{DateTime.Now.ToString("yyMMddss")}/{new Random().Next(1, 100).ToString("00")}";
        }

        public List<ShipmentSummary> GetPalletSummary(DateTime? startDate = null, DateTime? endDate = null, int? customerID = null, int? SourcefacilityId = null, bool groupbyFacility = false)
        {
            List<ShipmentSummary> result = null;
            var query = this.GetList();
            if (startDate != null)
                query = query.Where(x => x.DateAdded >= startDate);

            if (endDate != null)
                query = query.Where(x => x.DateAdded <= endDate);

            if (customerID != null)
                query = query.Where(x => x.ShipmentSource.CustomerId == customerID);

            if (SourcefacilityId != null)
                query = query.Where(x => x.ShipmentSourceId == SourcefacilityId);

            if (groupbyFacility)
            {
                result = query.GroupBy(p => p.ShipmentSourceId)
                           .Select(g => new ShipmentSummary
                           {
                               ShipmentSource = g.FirstOrDefault().ShipmentSource,
                               ShipmentDestination = g.FirstOrDefault().ShipmentDestination,
                               ShipmentSourceId = g.Key,
                               oCustomer = g.FirstOrDefault().ShipmentSource.Customer,
                               TotalShipments = g.Count(),
                               TotalPalletsTrackedIn = g.Sum(x => x.NoOfPalletsIn),
                               TotalPalletsTrackedOut = g.Sum(x => x.NoOfPalletsOut),
                           }).ToList();
            }
            else
            {
                result = query.GroupBy(p => p.CustomerId)
                          .Select(g => new ShipmentSummary
                          {
                              ShipmentSourceId = g.Key,
                              oCustomer = g.FirstOrDefault().ShipmentSource.Customer,
                              TotalShipments = g.Count(),
                              TotalPalletsTrackedIn = g.Sum(x => x.NoOfPalletsIn),
                              TotalPalletsTrackedOut = g.Sum(x => x.NoOfPalletsOut),
                          }).ToList();
            }
            return result;
        }
        public object GetDisplayList(List<ShipmentSummary> oShipments)
        {
            int serialNumber = 1;

            return oShipments.Select(i => new
            {
                Id = serialNumber,
               FacilityId = i.ShipmentSourceId,
                CustomerName = i.oCustomer?.CustomerName ?? "N/A",
                SourceFacilityName = i.ShipmentSource?.FacilityName ?? "N/A",
                DestinationFacilityName = i.ShipmentDestination?.FacilityName ?? "N/A",
                i.TotalPalletsTrackedIn,
                i.TotalPalletsTrackedOut,
                i.TotalShipments,
            });
        }

        public int SaveChanges()
        {
            return unitOfWork.SaveChanges();
        }
    }

}
