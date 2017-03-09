using System;

namespace MyAppTools.Domain
{
    public  class EventLog
    {
      public  int eventLogId { get; set; }
      public  string clientID { get; set; }
      public  string clientIpAddress { get; set; }
      public  string endPoint { get; set; }
      public  string eventSource { get; set; }
      public DateTime requestTime { get; set; }
      public string requestUrl { get; set; }
      public string requestContent { get; set; }
      public  string responseContent { get; set; }
      public  string responseCode { get; set; }
      public  string error_trace { get; set; }
      public  DateTime? reponseTime { get; set; }
    }
}
