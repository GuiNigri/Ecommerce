using System.Threading.Tasks;

namespace EcommercePrestige.Model.Interfaces.UoW
{
    public interface IUnitOfWork
    {
        void BeginTransaction();
        Task CommitAsync();
    }
}
