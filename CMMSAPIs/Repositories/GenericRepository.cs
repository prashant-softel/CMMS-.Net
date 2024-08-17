using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMMSAPIs.Helper;
namespace CMMSAPIs.Repositories
{
    public class GenericRepository: IDisposable
    {
        private MYSQLDBHelper _entities;
        public GenericRepository(MYSQLDBHelper sqlDbHelper)
        {
            _entities = sqlDbHelper;
        }

        public MYSQLDBHelper Context
        {

            get => _entities;
            set => _entities = value;
        }
        private bool disposed = false;
        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing && _entities != null)
                {
                    _entities.Dispose();
                    _entities = null;
                }
            }
            disposed = true;
        }


        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
