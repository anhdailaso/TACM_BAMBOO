using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Biz.Lib.TACM.NhomAnToaAn.IDataAccess;
using Biz.Lib.TACM.NhomAnToaAn.Model;
using Biz.Lib.Helpers;

namespace Biz.Lib.TACM.NhomAnToaAn.DataAccess
{
    public class NhomAnToaAnDataAccess : INhomAnToaAnDataAccess
    {
        public IEnumerable<NhomAnModel> NhomAnTheoToaAn(int ToaAnID)
        {
            List<NhomAnModel> list = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@ToaAnID", ToaAnID));
                list = DBUtils.ExecuteSPList<NhomAnModel>("SP_NhomAn_Theo_ToaAn", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }

        public ResponseResult ChonNhomAn(int ToaAnID, string MaNhomAn, string TenNhomAn, string NguoiTao)
        {
            ResponseResult result = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@ToaAnID", ToaAnID));
                listParameter.Add(new SqlParameter("@MaNhomAn", MaNhomAn));
                listParameter.Add(new SqlParameter("@TenNhomAn", TenNhomAn));
                listParameter.Add(new SqlParameter("@NguoiTao", NguoiTao));
                result = DBUtils.ExecuteSP<ResponseResult>("SP_NhomAn_Them", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public ResponseResult HuyChonNhomAn(int NhomAnID)
        {
            ResponseResult result = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@NhomAnID", NhomAnID));
                result = DBUtils.ExecuteSP<ResponseResult>("SP_NhomAn_Xoa", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
    }
}
