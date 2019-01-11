using Biz.Lib.Helpers;
using Biz.Lib.TACM.QuanLyNhanVien.IDataAccess;
using Biz.Lib.TACM.QuanLyNhanVien.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Lib.TACM.QuanLyNhanVien.DataAccess
{
    public class TaiKhoanNhanVienDataAccess : ITaiKhoanNhanVienDataAccess
    {
        public NhanVienModel ThongTinNhanVien(string maNV)
        {
            NhanVienModel nhanVien = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@MaNV", maNV));
                nhanVien = DBUtils.ExecuteSP<NhanVienModel>("SP_NhanVien_ChiTiet", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return nhanVien;
        }
        public IEnumerable<ChucNangModel> DanhSachChucNangTheoMaNhanVien(string maNV)
        {
            IEnumerable<ChucNangModel> danhSachChucNang = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@MaNV", maNV));
                danhSachChucNang = DBUtils.ExecuteSPList<ChucNangModel>("SP_NhanVien_DanhSachChucNang_Theo_MaNV", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return danhSachChucNang;
        }

        public ResponseResult CapNhatThongTinNhanVien(NhanVienModel model)
        {
            ResponseResult result = null;
            try
            {
                List<SqlParameter> listparameter = new List<SqlParameter>();               
                listparameter.Add(new SqlParameter("@MaNV", model.MaNV));
                listparameter.Add(new SqlParameter("@SoDienThoai", model.SoDienThoai));
                listparameter.Add(new SqlParameter("@NguoiTao", model.NguoiTao));
                
                result = DBUtils.ExecuteSP<ResponseResult>("SP_NhanVien_TaiKhoanNhanVien_Sua", listparameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public ResponseResult CapNhatDuongDanHinhDaiDienNhanVien(NhanVienModel model)
        {
            ResponseResult result = null;
            try
            {
                List<SqlParameter> listparameter = new List<SqlParameter>();
                listparameter.Add(new SqlParameter("@MaNVMoi", model.MaNVMoi));
                listparameter.Add(new SqlParameter("@DuongDanHinhDaiDien", model.DuongDanHinhDaiDien));
                listparameter.Add(new SqlParameter("@NguoiTao", model.NguoiTao));

                result = DBUtils.ExecuteSP<ResponseResult>("SP_NhanVien_TaiKhoanNhanVien_Sua_DuongDanHinhDaiDien", listparameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
    }
}
