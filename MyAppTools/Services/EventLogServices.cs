using Byaxiom.Logger;
using MyAppTools.Domain;
using MyAppTools.Infrastructure;
using System;
using System.Net.Http;

namespace MyAppTools.Services
{
    public enum EVENT_SOURCE
    {
        Admin,
        Api,
        WebUi

    }
    public interface IEventLogServices
    {
        int Add(EventLog oEventLog);
        void WriteEventLog(HttpRequestMessage request, string response, string err0r);
        bool Update(EventLog oEventLog);

    }
    public class EventLogServices : BaseService, IEventLogServices, IDisposable
    {
        private readonly IRepository<EventLog> _eventLogRepo;

        public EventLogServices()
        {
            _eventLogRepo = new BaseRepository<EventLog>(this.unitOfWork);
        }

        public int Add(EventLog oEventLog)
        {
            this._eventLogRepo.Add(oEventLog);
            this.unitOfWork.SaveChanges();
            return oEventLog.eventLogId;
        }

        public void WriteEventLog(HttpRequestMessage request, string response, string err0r)
        {

            var request_content = string.Empty;
            var response_content = string.Empty;
            try
            {
                if (request.Method == HttpMethod.Get)
                    request_content = request.RequestUri.PathAndQuery;
                else if (request.Method == HttpMethod.Post)
                    request_content = request.Content.ReadAsStringAsync().Result;
                else
                    request_content = request.ToString();



                var oEventLog = new EventLog
                {
                    clientID = UtilityServices.GetHeaderValue(request, "client-id"),
                    clientIpAddress = UtilityServices.GetClientIp(request),
                    endPoint = request.RequestUri.AbsolutePath,
                    eventSource = "API",
                    requestTime = DateTime.Now,
                    requestContent = request_content,
                    requestUrl = request.RequestUri.OriginalString,
                    reponseTime = DateTime.Now,
                    responseCode = response,
                    responseContent = response_content,
                    error_trace = err0r,
                };

                var _eventLogServices = new EventLogServices();
                _eventLogServices.Add(oEventLog);
            }
            catch (Exception e)
            {
                LogHelper.Log(e);
            }
        }

        public bool Update(EventLog oEventLog)
        {
            var originalEventLog = _eventLogRepo.Find(oEventLog.eventLogId);
            originalEventLog.responseCode = oEventLog.responseCode;
            originalEventLog.responseContent = oEventLog.responseContent;
            originalEventLog.reponseTime = oEventLog.reponseTime;
         
            this._eventLogRepo.Edit(originalEventLog);
            var numberOfRecordsAffected = this.unitOfWork.SaveChanges();
            if (numberOfRecordsAffected > 0)
            {
                return true;
            }

            return false;
        }

        public void Dispose()
        {
            this.Dispose();
        }
    }
}
