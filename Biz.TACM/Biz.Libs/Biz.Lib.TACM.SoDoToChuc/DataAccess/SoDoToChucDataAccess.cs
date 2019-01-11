using Biz.Lib.TACM.SoDoToChuc.IDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biz.Lib.TACM.SoDoToChuc.Models;
using System.Data.SqlClient;
using Biz.Lib.Helpers;

namespace Biz.Lib.TACM.SoDoToChuc.DataAccess
{
    public class SoDoToChucDataAccess : ISoDoToChucDataAccess
    {
        public IEnumerable<ChucDanhModel> DanhSachChucDanhTheoToaAn(int? toaAnId)
        {
            List<ChucDanhModel> danhSachChucDanh = null;
            try
            {
                var parameters = new List<SqlParameter> { new SqlParameter("@ToaAnID", toaAnId) };
                danhSachChucDanh = DBUtils.ExecuteSPList<ChucDanhModel>("SP_SoDoToChuc_DanhSachChucDanh", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return danhSachChucDanh;
        }
        public ChucDanhModel ChiTietChucDanhTheoId(int? soDoToChucId) //chucDanhId
        {
            ChucDanhModel chucDanh = null;
            try
            {
                var parameters = new List<SqlParameter> { new SqlParameter("@SoDoToChucID", soDoToChucId) };
                chucDanh = DBUtils.ExecuteSP<ChucDanhModel>("SP_SoDoToChuc_ChucDanh_ChiTiet_TheoID", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return chucDanh;
        }

        public ResponseResult ThemChucDanh(ChucDanhModel model)
        {
            ResponseResult result = null;
            try
            {
                List<SqlParameter> listparameter = new List<SqlParameter>();
                listparameter.Add(new SqlParameter("@ToaAnID", model.ToaAnID));
                listparameter.Add(new SqlParameter("@ChucDanh", model.ChucDanh));
                listparameter.Add(new SqlParameter("@DienGiaiNghiepVu", model.DienGiaiNghiepVu));
                listparameter.Add(new SqlParameter("@ChucDanhChaID", model.ChucDanhChaID));
                listparameter.Add(new SqlParameter("@Loai", model.Loai));
                listparameter.Add(new SqlParameter("@NguoiTao", model.NguoiTao));
                listparameter.Add(new SqlParameter("@GhiChu", model.GhiChu));

                result = DBUtils.ExecuteSP<ResponseResult>("SP_SoDoToChuc_ThemChucDanh", listparameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public ResponseResult SuaChucDanh(ChucDanhModel model)
        {
            ResponseResult result = null;
            try
            {
                List<SqlParameter> listparameter = new List<SqlParameter>();
                listparameter.Add(new SqlParameter("@SoDoToChucID", model.SoDoToChucID));
                listparameter.Add(new SqlParameter("@ToaAnID", model.ToaAnID));
                listparameter.Add(new SqlParameter("@ChucDanh", model.ChucDanh));
                listparameter.Add(new SqlParameter("@DienGiaiNghiepVu", model.DienGiaiNghiepVu));
                listparameter.Add(new SqlParameter("@ChucDanhChaID", model.ChucDanhChaID));
                listparameter.Add(new SqlParameter("@TrangThai", model.TrangThai));
                listparameter.Add(new SqlParameter("@NguoiTao", model.NguoiTao));
                listparameter.Add(new SqlParameter("@GhiChu", model.GhiChu));

                result = DBUtils.ExecuteSP<ResponseResult>("SP_SoDoToChuc_SuaChucDanh", listparameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public ResponseResult XoaChucDanh(int soDoToChucId)
        {
            ResponseResult result = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@SoDoToChucID", soDoToChucId));
                //var parameters = new List<SqlParameter> {new SqlParameter("@SoDoToChucID", soDoToChucId) };
                //result = DBUtils.ExecuteSP<ResponseResult>("SP_SoDoToChuc_XoaChucDanh", parameters, AppName.BizSecurity);
                result = DBUtils.ExecuteSP<ResponseResult>("SP_SoDoToChuc_XoaChucDanh",listParameter,AppName.BizSecurity);
            }
            catch(Exception ex)
            {
                throw (ex);
            }
            return result;
        }
    }
}