using System;
using System.Collections.Generic;

namespace EcommercePrestige.Application.ViewModel
{
    public class TrackingHistoryViewModel
    {
        public string TrackingCode { get; set; }
        public string Pedido { get; set; }
        public string Details { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }

        private IEnumerable<TrackingHistoryViewModel> packageTrackingList;

        public IEnumerable<TrackingHistoryViewModel> PackageTrackingList
        {
            get { return packageTrackingList; }
            set { packageTrackingList = value; }
        }
    }
}
