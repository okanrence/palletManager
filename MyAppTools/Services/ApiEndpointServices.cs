using MyAppTools.Domain;
using MyAppTools.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAppTools.Services
{
    public interface IEndpointServices
    {
        long Add(ApiEndpoints oApiEndpoints);
    }
    public class ApiEndpointServices : BaseService, IEndpointServices, IDisposable
    {
        private readonly IRepository<ApiEndpoints> _apiEndpointsRepo;


        public ApiEndpointServices()
        {
            _apiEndpointsRepo = new BaseRepository<ApiEndpoints>(this.unitOfWork);
        }
        public long Add(ApiEndpoints oApiEndpoints)
        {
            this._apiEndpointsRepo.Add(oApiEndpoints);
            this.unitOfWork.SaveChanges();
            return oApiEndpoints.apiEndpointsId;
        }

        public void Dispose()
        {
            this.Dispose();
        }
    }
}
