using Byaxiom.Logger;
using MyAppTools.Domain;
using MyAppTools.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel.Channels;
using System.Text;
using System.Web;

namespace MyAppTools.Services
{

    public interface IClientAuthenticationServices
    {
        bool AuthenticateClient(HttpRequestMessage request);
        long Add(ClientProfile oClientProfile);

        //bool AuthenticateClient(string client_id, string client_key, string method_name, string param_values, out int event_id);
    }
    public class ClientAuthenticationServices : BaseService, IClientAuthenticationServices, IDisposable
    {
        private readonly IRepository<ClientProfile> _clientProfileRepo;
        private readonly IRepository<ClientPermissions> _clientPermissionRepo;
        private readonly IRepository<ApiEndpoints> _apiEndpointRepo;
        public ClientAuthenticationServices()
        {
            _clientProfileRepo = new BaseRepository<ClientProfile>(this.unitOfWork);
            _clientPermissionRepo = new BaseRepository<ClientPermissions>(this.unitOfWork);
            _apiEndpointRepo = new BaseRepository<ApiEndpoints>(this.unitOfWork);
        }

        public bool AuthenticateClient(HttpRequestMessage request)
        {
            var isAuthenticated = false;
            try
            {
                if (request != null)
                {
                    var client_id = UtilityServices.GetHeaderValue(request, "client-id");
                    var client_key = UtilityServices.GetHeaderValue(request, "client-key");
                    var endPoint = request.RequestUri.AbsolutePath;
                    var client_ip_address = UtilityServices.GetClientIp(request);

                     var oClientProfile = GetClientProfile(client_id, client_key, client_ip_address);

                    if (oClientProfile != null)
                    {
                        if (oClientProfile.unRestricted)
                            isAuthenticated = true;
                        else
                            isAuthenticated = IsPermitted(oClientProfile.clientId, endPoint);
                    }
                }
            }
           catch(Exception ex)
            {
                isAuthenticated = false;
                LogHelper.Log(ex);

            }
            return isAuthenticated;
        }

        public IEnumerable<ClientProfile> GetAll()
        {
            return _clientProfileRepo.All.AsNoTracking()
                  .ToList();
        }
        private ClientProfile GetClientProfile(string client_id, string client_key, string client_ip_address)
        {
            return this.GetAll().Where(x => x.clientId == client_id && x.clientKey.ToString() == client_key && x.clientIpAddress.Contains(client_ip_address)).FirstOrDefault();
        }

        private bool IsPermitted(string client_id, string absolute_path)
        {
            var oApiEndpoint = _apiEndpointRepo.All.Where(x => x.absolutePath == absolute_path).FirstOrDefault();

            if (oApiEndpoint == null)
            {
                oApiEndpoint = new ApiEndpoints { absolutePath = absolute_path, dateAdded = DateTime.Now };
                _apiEndpointRepo.Add(oApiEndpoint);
                this.unitOfWork.SaveChanges();
                return false;
            }

            var oClientPermission = _clientPermissionRepo.All.Where(x => x.clientID == client_id && x.endPointId == oApiEndpoint.apiEndpointsId).FirstOrDefault();
            if (oClientPermission != null)
            {
                return true;
            }

            return false;
        }

        public long Add(ClientProfile oClientProfile)
        {
            this._clientProfileRepo.Add(oClientProfile);
            this.unitOfWork.SaveChanges();
            return oClientProfile.ClientProfileId;
        }

        public void Dispose()
        {
            this.Dispose();
        }

    }


}
