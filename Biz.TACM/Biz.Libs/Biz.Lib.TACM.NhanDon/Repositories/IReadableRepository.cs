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
    public interface IReadableRepository<out T> where T : class, new()
    {
        T Get(object id);
        IEnumerable<T> Get(Dictionary<string, object> query);
    }
}
