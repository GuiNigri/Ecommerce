using System;
using System.Threading.Tasks;
using EcommercePrestige.Data.Ecommerce.Context;
using EcommercePrestige.Model.Interfaces.UoW;

namespace EcommercePrestige.Data.UoW
{
    public class UnitOfWork:IUnitOfWork, IDisposable
    {
        private readonly EcommerceContext _context;
        private bool _disposed;

        public UnitOfWork(EcommerceContext context)
        {
            _context = context;
        }
        public void BeginTransaction()
        {
            _disposed = false;
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        protected virtual async void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    await _context.DisposeAsync();
                }
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
