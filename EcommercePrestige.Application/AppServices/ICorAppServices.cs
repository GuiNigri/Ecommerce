using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EcommercePrestige.Application.ViewModel;

namespace EcommercePrestige.Application.AppServices
{
    public interface ICorAppServices
    {
        Task<IEnumerable<CorViewModel>> GetAllAsync();
        Task<CorViewModel> GetByIdAsync(int id);
    }
}
