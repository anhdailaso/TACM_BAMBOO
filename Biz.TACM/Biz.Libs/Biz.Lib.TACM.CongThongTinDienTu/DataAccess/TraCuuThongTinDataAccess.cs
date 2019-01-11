using Biz.Lib.Helpers;
using Biz.Lib.TACM.CongThongTinDienTu.IDataAccess;
using Biz.Lib.TACM.CongThongTinDienTu.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Lib.TACM.CongThongTinDienTu.DataAccess
{
    public class TraCuuThongTinDataAccess : ITraCuuThongTinDataAccess
    {
        public TraCuuHoSoVuAnModel GetHoSoVuAnDuocTraCuu(string keyword, ref int hoSoVuAnID)
        {
            TraCuuHoSoVuAnModel hoSoVuAnDuocTraCuu = null;
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@Keyword", keyword)
                };

                SqlParameter par = new SqlParameter("@HoSoVuAnID", SqlDbType.Int);
                par.Direction = ParameterDirection.Output;
                par.Value = null;
                parameters.Add(par);

                hoSoVuAnDuocTraCuu = DBUtils.ExecuteSP<TraCuuHoSoVuAnModel>("SP_CongThongTinDienTu_TraCuu_Theo_Keyword", parameters, AppName.BizSecurity);

                hoSoVuAnID = (int)par.Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return hoSoVuAnDuocTraCuu;
        }
    }
}
