using PalletManagement.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace PalletManagement.UI.Controllers
{
    public class CustomerController : ApiController
    {
        private static readonly ICustomerServices _customerService = null;

        // GET: Customer
        public IHttpActionResult Index()
        {
            return _customerService.GetList().Where(x => x.CustomerId);
        }
    }
}