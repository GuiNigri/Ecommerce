using System.IO;
using System.Threading.Tasks;

namespace EcommercePrestige.Model.Interfaces.Infrastructure
{
    public interface IBlobInfrastructure
    {
        Task<string> CreateBlobAsync(Stream stream, string container);
        Task DeleteBlobAsync(string blobName, string container);
    }
}
