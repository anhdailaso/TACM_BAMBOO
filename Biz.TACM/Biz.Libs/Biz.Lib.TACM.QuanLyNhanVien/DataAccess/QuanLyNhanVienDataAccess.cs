using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biz.Lib.Helpers;
using Biz.Lib.TACM.QuanLyNhanVien.IDataAccess;
using Biz.Lib.TACM.QuanLyNhanVien.Model;
using Microsoft.SqlServer.Server;

namespace Biz.Lib.TACM.QuanLyNhanVien.DataAccess
{
    public class QuanLyNhanVienDataAccess : IQuanLyNhanVienDataAccess
    {

        public IEnumerable<NhanVienModel> DanhsachNhanVien()
        {
            List<NhanVienModel> list = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                list = DBUtils.ExecuteSPList<NhanVienModel>("SP_NhanVien_DanhSachNhanVien", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }
        public NhanVienModel ChitietNhanVien(int nhanVienId)
        {
            NhanVienModel nhanvien = new NhanVienModel();
            try
            {
                var parameters = new List<SqlParameter> { new SqlParameter("@NhanvienID", nhanVienId) };
                nhanvien = DBUtils.ExecuteSP<NhanVienModel>("SP_NhanVien_ChiTiet_Theo_NhanVenID", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return nhanvien;
        }
        public string GetMaNVtheoEmail(string email)
        {
            try
            {
                var parameters = new List<SqlParameter> {
                    new SqlParameter("@Email", email)};
                var manv = DBUtils.ExecuteSP<NhanVienModel>("SP_NhanVien_Get_MaNV_Theo_Email", parameters, AppName.BizSecurity);
                if (manv != null)
                    return manv.MaNV.ToString().Trim();
                else return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IEnumerable<ChucNangModel> ChiTietChucNang(int NhanVienid)
        {
            List<ChucNangModel> dschucnang = null;
            try
            {
                var parameters = new List<SqlParameter> { new SqlParameter("@NhanvienID", NhanVienid) };
                dschucnang = DBUtils.ExecuteSPList<ChucNangModel>("SP_NhanVienChucNang_ChiTiet_Theo_NhanVenID", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;

            }
            return dschucnang;
        }
        public ResponseResult ThemNhanVien(NhanVienModel model)
        {
            ResponseResult result = null;
            try
            {
                List<SqlParameter> listparameter = new List<SqlParameter>();
                listparameter.Add(new SqlParameter("@ToaAnID", model.ToaAnID));
                listparameter.Add(new SqlParameter("@MaNV", model.MaNV));
                listparameter.Add(new SqlParameter("@HoNV", model.HoNV));
                listparameter.Add(new SqlParameter("@TenNV", model.TenNV));
                listparameter.Add(new SqlParameter("@TenLotNV", model.TenLotNV));
                listparameter.Add(new SqlParameter("@GioiTinh", model.GioiTinh));
                listparameter.Add(new SqlParameter("@NgaySinh", model.NgaySinh));
                listparameter.Add(new SqlParameter("@SoDienThoai", model.SoDienThoai));
                listparameter.Add(new SqlParameter("@Email", model.Email));
                listparameter.Add(new SqlParameter("@DuongDanHinnhDaiDien", model.DuongDanHinhDaiDien));
                listparameter.Add(new SqlParameter("@SoDoToChucID", model.SoDoToChucID));
                listparameter.Add(new SqlParameter("@SoDoToChucChucVuID", model.SoDoToChucChucVuID));
                listparameter.Add(new SqlParameter("@TaoTaiKhoan", model.TaoTaiKhoan));
                listparameter.Add(new SqlParameter("@TrangThai", model.TrangThai));
                listparameter.Add(new SqlParameter("@NguoiTao", model.NguoiTao));
                listparameter.Add(new SqlParameter("@GhiChu", model.GhiChu));
                result = DBUtils.ExecuteSP<ResponseResult>("SP_NhanVien_ThemNhanVien", listparameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            { throw ex; }
            return result;
        }
        public ResponseResult SuaNhanVien(NhanVienModel model)
        {
            ResponseResult result = null;
            try
            {
                List<SqlParameter> listparameter = new List<SqlParameter>();
                listparameter.Add(new SqlParameter("@NhanVienID", model.NhanvienID));
                listparameter.Add(new SqlParameter("@MaNV", model.MaNV));
                listparameter.Add(new SqlParameter("@MaNVMoi", model.MaNVMoi));
                listparameter.Add(new SqlParameter("@HoNV", model.HoNV));
                listparameter.Add(new SqlParameter("@TenNV", model.TenNV));
                listparameter.Add(new SqlParameter("@TenLotNV", model.TenLotNV));
                listparameter.Add(new SqlParameter("@GioiTinh", model.GioiTinh));
                listparameter.Add(new SqlParameter("@NgaySinh", model.NgaySinh));
                listparameter.Add(new SqlParameter("@SoDienThoai", model.SoDienThoai));
                listparameter.Add(new SqlParameter("@Email", model.Email));
                listparameter.Add(new SqlParameter("@SoDoToChucID", model.SoDoToChucID));
                listparameter.Add(new SqlParameter("@SoDoToChucChucVuID", model.SoDoToChucChucVuID));
                listparameter.Add(new SqlParameter("@TrangThai", model.TrangThai));
                listparameter.Add(new SqlParameter("@NguoiTao", model.NguoiTao));
                listparameter.Add(new SqlParameter("@GhiChu", model.GhiChu));
                result = DBUtils.ExecuteSP<ResponseResult>("SP_NhanVien_Sua", listparameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public ResponseResult SuaToaAnTrucThuoc(int nhanVienID, int toaAnID)
        {
            ResponseResult result = null;
            try
            {
                List<SqlParameter> listparameter = new List<SqlParameter>();
                listparameter.Add(new SqlParameter("@NhanVienID", nhanVienID));
                listparameter.Add(new SqlParameter("@ToaAnID", toaAnID));
                
                result = DBUtils.ExecuteSP<ResponseResult>("SP_NhanVien_Sua_ToaAn", listparameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public ResponseResult XoaNhanVien(int NhanVienid)
        {
            ResponseResult result = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@NhanVienID", NhanVienid));
                result = DBUtils.ExecuteSP<ResponseResult>("SP_NhanVien_Xoa", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public IEnumerable<SelectListChucDanhModel> DanhSachChucDanh(int toaAnid)
        {
            List<SelectListChucDanhModel> list = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter> { new SqlParameter("@ToaAnID", toaAnid) };

                list = DBUtils.ExecuteSPList<SelectListChucDanhModel>("SP_NhanVien_DanhSachChucDanh", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;

        }
        public IEnumerable<NhanVienModel> DanhsachNhanVienTheoToaAn(int ToaAnID)
        {
            List<NhanVienModel> list = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@ToaAnID", ToaAnID));
                list = DBUtils.ExecuteSPList<NhanVienModel>("SP_NhanVien_DanhSachNhanVien_Theo_ToaAn",
                listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;

        }
        public IEnumerable<NhanVienModel> ImportNhanVien(string nguoitao, string nhanvienDataXml, int ToaAnID)
        {
            List<NhanVienModel> list = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@nguoiTao", nguoitao));
                listParameter.Add((new SqlParameter("@nhanVienDataXml", nhanvienDataXml)));
                listParameter.Add(new SqlParameter("@toaAnID", ToaAnID));
                list = DBUtils.ExecuteSPList<NhanVienModel>("SP_NhanVien_Import", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }
        public ResponseResult Capnhathinhdaidien(int NhanVienID, string img_url)
        {
            ResponseResult result = null;
            try
            {

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public IEnumerable<NhanVienToaAnModel> DanhSachNhanVienToaAn(int NhanVienID)
        {
            List<NhanVienToaAnModel> list = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@NhanVienID", NhanVienID));
                list = DBUtils.ExecuteSPList<NhanVienToaAnModel>("SP_NhanVienToaAn_DanhSach", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }
        public ResponseResult ThemNhanVienToaAn(int NhanVienID, int ToaAnID, string nguoiTao)
        {
            ResponseResult result = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@NhanVienID", NhanVienID));
                listParameter.Add(new SqlParameter("@ToaAnID", ToaAnID));
                listParameter.Add(new SqlParameter("@NguoiTao", nguoiTao));
                listParameter.Add(new SqlParameter("@flag", "return"));
                result = DBUtils.ExecuteSP<ResponseResult>("SP_NhanVienToaAn_Them", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public ResponseResult XoaNhanVienToaAn(int NhanVienToaAnID, int NhanVienID, int ToaAnID, string nguoiTao)
        {
            ResponseResult result = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@NhanVienToaAnID", NhanVienToaAnID));
                listParameter.Add(new SqlParameter("@NhanVienID", NhanVienID));
                listParameter.Add(new SqlParameter("@ToaAnID", ToaAnID));
                listParameter.Add(new SqlParameter("@NguoiTao", nguoiTao));
                result = DBUtils.ExecuteSP<ResponseResult>("SP_NhanVienToaAn_Xoa", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public ResponseResult CheckMaNV(string MaNV)
        {
            ResponseResult result = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@MaNV", MaNV));
                result = DBUtils.ExecuteSP<ResponseResult>("SP_NhanVien_CheckMaNV", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        #region Searching Nhan Vien by Keyword
        public IEnumerable<NhanVienModel> DanhSachNhanVienSearchByKeyword(string keyword, int? toaAnID)
        {
            List<NhanVienModel> list = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@keyword", keyword));
                listParameter.Add(new SqlParameter("@ToaAnID", toaAnID));
                list = DBUtils.ExecuteSPList<NhanVienModel>("SP_TimKiem_NhanVien_Theo_Keyword", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }
        #endregion

        #region So Do To Chuc Quan Ly Nhan Vien
        public IEnumerable<NhanVienModel> DanhSachNhanVienTheoChucDanh(int? toaAnID, string chucDanh)
        {
            List<NhanVienModel> list = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@ToaAnID", toaAnID));
                listParameter.Add(new SqlParameter("@ChucDanh", chucDanh));
                list = DBUtils.ExecuteSPList<NhanVienModel>("SP_SoDoToChuc_DanhSachNhanVien_Theo_ChucDanh", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }
        public IEnumerable<NhanVienModel> DanhSachNhanVienTheoChucVu(int? toaAnID, string chucDanh)
        {
            List<NhanVienModel> list = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@ToaAnID", toaAnID));
                listParameter.Add(new SqlParameter("@ChucDanh", chucDanh));
                list = DBUtils.ExecuteSPList<NhanVienModel>("SP_SoDoToChuc_DanhSachNhanVien_Theo_ChucVu", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }
        #endregion

        #region Cap Nhat Chuc Nang Nhan Vien
        public ResponseResult CapNhatChucNangNhanVien(List<ChucNangModel> danhSachChucNang)
        {
            ResponseResult result = null;
            try
            {
                var pList = new SqlParameter("@DanhSachChucNang", SqlDbType.Structured);
                pList.TypeName = "dbo.DanhSachEditedChucNang";
                pList.Value = GetAddChucNangList(danhSachChucNang);
                List<SqlParameter> listparameter = new List<SqlParameter>();
                listparameter.Add(pList);

                result = DBUtils.ExecuteSP<ResponseResult>("SP_NhanVien_DanhSachChucNang_CapNhat", listparameter, AppName.BizSecurity);
            }
            catch (Exception ex) 
            {
                throw ex;
            }
            return result;
        }
        private List<SqlDataRecord> GetAddChucNangList(List<ChucNangModel> danhSachChucNang)
        {
            List<SqlDataRecord> datatable = new List<SqlDataRecord>();
            SqlMetaData[] sqlMetaData = new SqlMetaData[6];
            sqlMetaData[0] = new SqlMetaData("NhanVienID", SqlDbType.Int);
            sqlMetaData[1] = new SqlMetaData("MaChucNang", SqlDbType.NVarChar, 50);
            sqlMetaData[2] = new SqlMetaData("TenChucNang", SqlDbType.NVarChar, 200);
            sqlMetaData[3] = new SqlMetaData("ChucNangChinh", SqlDbType.Int);
            sqlMetaData[4] = new SqlMetaData("NguoiTao", SqlDbType.NVarChar, 100);
            sqlMetaData[5] = new SqlMetaData("GhiChu", SqlDbType.NVarChar, SqlMetaData.Max); //or use -1 to explicitly set nvarchar(max)
            SqlDataRecord row = new SqlDataRecord(sqlMetaData);
            foreach (var item in danhSachChucNang)
            {
                row = new SqlDataRecord(sqlMetaData);
                row.SetValues(new object[] {
                    item.NhanVienID,
                    item.MaChucNang,
                    item.TenChucNang,
                    item.ChucNangChinh,
                    item.NguoiTao,
                    item.GhiChu
                });
                datatable.Add(row);
            }
            return datatable;
        }
        #endregion
       
        #region Cap Nhat Thu Ky Cho Tham Phan
        public ResponseResult CapNhatThuKyThamPhan(List<NhanVienModel> danhsachthuky)
        {
            ResponseResult result = null;
            try
            {
                var pList = new SqlParameter("@DanhSachThuKy", SqlDbType.Structured);
                pList.TypeName = "dbo.DanhSachEditedThuKy";
                pList.Value = GetAddThuKyList(danhsachthuky);

                List<SqlParameter> listparameter = new List<SqlParameter>();
                listparameter.Add(pList);

                result = DBUtils.ExecuteSP<ResponseResult>("SP_NhanVien_DuLieuThuKyTheoThamPhan_CapNhat", listparameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        private List<SqlDataRecord> GetAddThuKyList(List<NhanVienModel> danhsachthuky)
        {
            List<SqlDataRecord> datatable = new List<SqlDataRecord>();
            SqlMetaData[] sqlMetaData = new SqlMetaData[2];
            sqlMetaData[0] = new SqlMetaData("MaNVThamPhan", SqlDbType.NVarChar, 50);
            sqlMetaData[1] = new SqlMetaData("MaNVThuKy", SqlDbType.NVarChar, 50);
            SqlDataRecord row = new SqlDataRecord(sqlMetaData);
            foreach (var item in danhsachthuky)
            {
                row = new SqlDataRecord(sqlMetaData);
                row.SetValues(new object[] {
                    item.MaNV,
                    item.MaNVThuKy,
                });
                datatable.Add(row);
            }

            return datatable;
        }
        #endregion

        public IEnumerable<NhanVienModel> DanhsachThuKyThamPhan(string ManV)
        {
            List<NhanVienModel> list = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter> { new SqlParameter("@MaNV", ManV) };

                list = DBUtils.ExecuteSPList<NhanVienModel>("SP_NhanVien_DanhSachThuKy_Theo_MaNV", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;

        }
    }
}
