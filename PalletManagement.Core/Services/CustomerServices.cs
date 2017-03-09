using MyAppTools.Helpers;
using PalletManagement.Core.Domain;
using PalletManagement.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectIV.Core.Services
{

    public interface ICustomerServices
    {
        int Add(Customer oCustomer);
        int Update(Customer oCustomer);
        Customer GetbyId(int CustomerId);
        IEnumerable<Customer> GetList();
    }

    public class CustomerServices : BaseService, ICustomerServices
    {
        private readonly IRepository<Customer> _customerRepo;
        public CustomerServices()
        {
            _customerRepo = new BaseRepository<Customer>(this.unitOfWork);
        }

        public int Add(Customer oCustomer)
        {
            _customerRepo.Add(oCustomer);
            return this.unitOfWork.SaveChanges();
        }

        public int Update(Customer oCustomer)
        {
            var originalCustomer = _customerRepo.Find(oCustomer.CustomerId);
            originalCustomer.Address = oCustomer.Address;
            originalCustomer.CustomerName = oCustomer.CustomerName;
            originalCustomer.EmailAddress = oCustomer.EmailAddress;
            originalCustomer.Facilities = oCustomer.Facilities;

            _customerRepo.Edit(originalCustomer);

            return unitOfWork.SaveChanges();
        }
     
        public Customer GetbyId(int CustomerId)
        {
            return _customerRepo.Find(CustomerId);

        }

        public IEnumerable<Customer> GetList()
        {
            return _customerRepo.All;
        }
    }

}
