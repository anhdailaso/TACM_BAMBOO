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

    public abstract class SqlStoreProcedureRepositoryBase<T> : RepositoryBase<T>, IModifiableRepository<T>, IReadableRepository<T>
        where T : class, new()
    {        
        protected virtual string GetStorePrefix
        {
            get
            {
                var app = "SP_";
                var module = typeof(T).Name;

                return $"{app}_{module}";
            }
        }
        protected abstract List<SqlParameter> CreateAddOrUpdateParams(T model);
        protected virtual string AddStoredProcedure { get { return $"{GetStorePrefix}_Add"; } }
        protected virtual string UpdateStoredProcedure { get { return $"{GetStorePrefix}_Update"; } }
        protected virtual string DeleteStoredProcedure { get { return $"{GetStorePrefix}_Delete"; } }
        protected virtual string GetByIdStoredProcedure { get { return $"{GetStorePrefix}_Get_ById"; } }
        protected virtual string GetStoredProcedure { get { return $"{GetStorePrefix}_Get"; } }

        public virtual int Create(T model)
        {
            var parameters = this.CreateAddOrUpdateParams(model);
            parameters.Add(new SqlParameter
            {
                Direction = ParameterDirection.ReturnValue
            });

            return Execute(() =>
            {
                var result = DBUtils.ExecNonQuerySP(this.AddStoredProcedure, parameters, AppName.BizC3Assignment);
                return (int)Protect.ToInt16(result, 0);
            });
        }

        public virtual int Update(T entity)
        {
            var parameters = this.CreateAddOrUpdateParams(entity);
            parameters.Add(new SqlParameter { Direction = ParameterDirection.ReturnValue });

            return Execute(() =>
            {
                var result = DBUtils.ExecNonQuerySP(this.UpdateStoredProcedure, parameters, AppName.BizC3Assignment);
                return (int)Protect.ToInt16(result, 0);
            });
        }

        public virtual bool Delete(object id)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@id", id)
            };
            parameters.Add(new SqlParameter
            {
                Direction = ParameterDirection.ReturnValue
            });

            return Execute(() =>
            {
                var result = DBUtils.ExecNonQuerySP(this.DeleteStoredProcedure, parameters, AppName.BizC3Assignment);
                return (int)Protect.ToInt16(result, 0) == ResponseCode.Success.GetHashCode();
            });
        }

        public virtual T Get(object id)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@id", id));

            return Execute(() => { return DBUtils.ExecuteSP<T>(this.GetByIdStoredProcedure, parameters, AppName.BizC3Assignment); });
        }

        public virtual IEnumerable<T> Get(Dictionary<string, object> query)
        {
            var parameters = query.Select(m => new SqlParameter(m.Key, m.Value)).ToList();

            return Execute(() => { return DBUtils.ExecuteSPList<T>(this.GetStoredProcedure, parameters, AppName.BizC3Assignment); });
        }
    }
}
