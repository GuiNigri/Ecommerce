using System;
using System.Collections.Generic;
using System.Text;

namespace EcommercePrestige.Application.ViewModel
{
    public class BannersHomeViewModel:BaseViewModel
    {
        public string UrlBanner { get; set; }

        public IEnumerable<BannersHomeViewModel> Banners { get; set; }

        public BannersHomeViewModel()
        {
            
        }

        public BannersHomeViewModel(IEnumerable<BannersHomeViewModel> banners, string statusModel)
        {
            StatusModel = statusModel;
            Banners = banners;
        }
    }
}
