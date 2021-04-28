using System;

namespace EcommercePrestige.Model.Entity
{
    public class TrackingHistoryModel
    {
        public string Details { get; private set; }
        public string Status { get; private set; }
        public DateTime Date { get; private set; }
        public string Location { get; private set; }

        public TrackingHistoryModel(string details, string status, DateTime date, string location)
        {
            Details = details;
            Status = status;
            Date = date;
            Location = location;
        }
    }
}
