using Biz.Lib.TACM.NhanDon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Biz.Lib.TACM.NhanDon.Repositories
{

    public interface IHosoRepository : IModifiableRepository<HosoVuAnEntity>, IReadableRepository<HosoVuAnEntity>
    {

    }

    public class HosoRepository : SqlStoreProcedureRepositoryBase<HosoVuAnEntity>, IHosoRepository
    {
        protected override List<SqlParameter> CreateAddOrUpdateParams(HosoVuAnEntity model)
        {
            throw new NotImplementedException();
        }
    }
}
