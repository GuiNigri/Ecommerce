using System;
using System.Collections.Generic;
using System.Text;

namespace EcommercePrestige.Model.Entity
{
    public class BannersHomeModel:BaseModel
    {
        public string UrlBanner { get; private set; }

        public BannersHomeModel()
        {
        }

        public BannersHomeModel(string urlBanner)
        {
            UrlBanner = urlBanner;
        }
    }
}
