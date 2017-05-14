using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using PalletManagement.Core.Domain;
using PalletManagement.Core.Services;

namespace PalletManagement.UI.Controllers
{
    public class UsersController : ApiController
    {
        private  readonly IUserServices _userServices = null;
        private  readonly IUserRoleServices _userRoleService = null;

        public UsersController()
        {
            _userServices = new UserServices();
            _userRoleService = new UserRoleServices();
        }

        [HttpGet]
        [Route("api/users")]
        public IHttpActionResult GetAllUsers()
        {
            try
            {
                var users = _userServices.GetList().ToList();
              
                if (!users.Any())
                    return NotFound();
                var userObject = _userServices.GetDisplayList(users);
                return Ok(userObject);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/users/auth")]
        public IHttpActionResult Authenticate()
        {
            try
            {
                var user = _userServices.Authenticate(Request);
                if (user == null)
                    return NotFound();
                return Ok(user);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        
        [HttpPost]
        [Route("api/users")]
        public IHttpActionResult Add(User model)
        {
            try
            {
                if (ModelState.IsValid)
                    return BadRequest();
                var userId = _userServices.Add(model);

                if (userId > 0) return Ok(userId);
                return BadRequest();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}