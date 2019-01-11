using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Biz.Lib.TACM.PhanCongNhanVien.IDataAccess;
using Biz.Lib.TACM.PhanCongNhanVien.Model;
using Biz.Lib.Helpers;

namespace Biz.Lib.TACM.PhanCongNhanVien.DataAccess
{
    public class PhanCongNhanVienDataAccess : IPhanCongNhanVienDataAccess
    {
        public IEnumerable<NhomAnNhanVienModel> DSNhomAn(int ToaAnID)
        {
            List<NhomAnNhanVienModel> list = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@ToaAnID", ToaAnID));
                list = DBUtils.ExecuteSPList<NhomAnNhanVienModel>("SP_NhomAnNhanVien_Theo_ToaAn", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }

        public ResponseResult ChonNhomAnNV(int NhanVienID, int NhomAnID, string NguoiTao)
        {
            ResponseResult result = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@NhanVienID", NhanVienID));
                listParameter.Add(new SqlParameter("@NhomAnID", NhomAnID));
                listParameter.Add(new SqlParameter("@NguoiTao", NguoiTao));
                result = DBUtils.ExecuteSP<ResponseResult>("SP_NhanVienNhomAn_Them", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public ResponseResult HuyChonNhomAnNV_log(int NhanVienNhomAnID, int NhanVienID, int NhomAnID, string NguoiTao)
        {
            ResponseResult result = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@NhanVienNhomAnID", NhanVienNhomAnID));
                listParameter.Add(new SqlParameter("@NhanVienID", NhanVienID));
                listParameter.Add(new SqlParameter("@NhomAnID", NhomAnID));
                listParameter.Add(new SqlParameter("@NguoiTao", NguoiTao));
                result = DBUtils.ExecuteSP<ResponseResult>("SP_NhanVienNhomAn_Xoa", listParameter, AppName.BizSecurity);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public IEnumerable<NhanVienNhomAnModel> DSNhanVienNhomAn(int NhomAnID)
        {
            List<NhanVienNhomAnModel> list = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@NhomAnID", NhomAnID));
                list = DBUtils.ExecuteSPList<NhanVienNhomAnModel>("SP_NhanVienNhomAn_Theo_NhomAn", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }
        public IEnumerable<NhanVienModel> DSNhanVienTheoChucDanh(int ToaAnID, int? SoDoToChucID)
        {
            List<NhanVienModel> list = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@ToaAnID", ToaAnID));
                listParameter.Add(new SqlParameter("@SoDoToChucID", SoDoToChucID));
                list = DBUtils.ExecuteSPList<NhanVienModel>("SP_NhanVien_Theo_ChucDanh", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;

        }
    }
}
