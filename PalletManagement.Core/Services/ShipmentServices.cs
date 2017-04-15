using MyAppTools.Helpers;
using PalletManagement.Core.Domain;
using PalletManagement.Core.Infrastructure;
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
            originalShipment.NoOfPallets = oShipment.NoOfPallets;
            originalShipment.TruckNumber = oShipment.TruckNumber;
            originalShipment.PalletList = oShipment.PalletList;
            originalShipment.IsCompleted = oShipment.IsCompleted;


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
            return _shipmentRepo.All.AsNoTracking();
        }

        public object GetDisplayList(List<Shipment> oShipments)
        {
            return oShipments.Select(i => new
            {
                i.ShipmentId,
                i.DestinationDateTime,
                i.SourceDateTime,
                InTracker = i.InTracker?.FullName ?? "N/A",
                OutTracker = i.OutTracker?.FullName ?? "N/A",
                Destination = i.ShipmentDestination?.FacilityName ?? "N/A",
                i.ShipmentNumber,
                Source = i.ShipmentSource?.FacilityName ?? "N/A",
                ShipmentStatus= i.ShipmentStatus?.ShipmentStatusName ?? "N/A",
                TruckNumber = i.TruckNumber,
                i.NoOfPallets,
                i.IsCompleted
                
            });
        }
        public string GetShipmentNumber(string source) { 
            return $"{source.Substring(0,2).ToUpper()}/{DateTime.Now.ToString("yyMMddss")}/{new Random().Next(1, 100).ToString("00")}";
        }
    }

}
