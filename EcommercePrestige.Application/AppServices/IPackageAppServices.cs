using System.Collections.Generic;
using System.Threading.Tasks;
using EcommercePrestige.Application.ViewModel;

namespace EcommercePrestige.Application.AppServices
{
    public interface IPackageAppServices
    {
        Task<IEnumerable<TrackingHistoryViewModel>> GetTracking(string codRastreio, int codPedido);
    }
}
