using MyAppTools.Helpers;
using PalletManagement.Core.Domain;
using PalletManagement.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PalletManagement.Core.Services
{

    public interface ICustomerServices
    {
        int Add(Customer oCustomer);
        int Update(Customer oCustomer);
        int Delete(int CustomerId);
        Customer GetbyId(int CustomerId);
        IQueryable<Customer> GetList();
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
            var originalCustomer = GetbyId(oCustomer.CustomerId);
            originalCustomer.CustomerName = oCustomer.CustomerName;
            originalCustomer.EmailAddress = oCustomer.EmailAddress;
            originalCustomer.ContactPerson = oCustomer.ContactPerson;
            originalCustomer.PhoneNumber = oCustomer.PhoneNumber;
            originalCustomer.Facilities = oCustomer.Facilities;

            _customerRepo.Edit(originalCustomer);

            return unitOfWork.SaveChanges();
        }
     
        public Customer GetbyId(int customerId)
        {
            return
                _customerRepo.All.AsNoTracking()
                .FirstOrDefault(x => x.CustomerId == customerId);
        }

        public IQueryable<Customer> GetList()
        {
            return _customerRepo.All.AsNoTracking();
        }

        public int Delete(int customerId)
        {
            var oCustomer = GetbyId(customerId);
            _customerRepo.Delete(oCustomer);
            return unitOfWork.SaveChanges();
        }
    }

}
