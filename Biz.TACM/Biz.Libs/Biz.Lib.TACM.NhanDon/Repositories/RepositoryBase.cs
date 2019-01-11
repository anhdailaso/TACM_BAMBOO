using Biz.Lib.Helpers;
using Biz.Lib.TACM.NhanDon.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Lib.TACM.NhanDon.Repositories
{

    public abstract class RepositoryBase<T> where T : class, new()
    {
        protected virtual TResult Execute<TResult>(Func<TResult> func)
        {
            try
            {
                return func != null ? func.Invoke() : default(TResult);
            }
            catch (DataException ex)
            {
                // Log
                WriteLog.Error(ex.Message);

                throw new DataAccessException("Process database has error. Use detail Exception to view detail", ex);
            }
        }
    }
}
