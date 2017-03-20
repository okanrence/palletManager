using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalletManagement.Core.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        int SaveChanges();
    }

    internal class UnitOfWork : IUnitOfWork
    {
        private appDbContext _Context = new appDbContext();
        internal appDbContext Context { get { return this._Context; } }

        public int SaveChanges()
        {
            return this._Context.SaveChanges();
        }

        public void Dispose()
        {
            this._Context.Dispose();
        }
    }
}
