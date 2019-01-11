using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biz.Lib.Helpers;
using Biz.Lib.SettingData.Model;
using Biz.Lib.TACM.NhanDon.IDataAccess;
using Biz.Lib.TACM.NhanDon.Model;
using CongDoanHoSoModel = Biz.Lib.TACM.NhanDon.Model.CongDoanHoSoModel;
using HoSoVuAnModel = Biz.Lib.TACM.NhanDon.Model.HoSoVuAnModel;

namespace Biz.Lib.TACM.NhanDon.DataAccess
{
    public class NhanDonDataAccess : INhanDonDataAccess
    {
        #region HoSoVuAn
        public IEnumerable<HoSoVuAnModel> DanhSachNgayHoSoVuAn(int hoSoVuAnID, int giaiDoan)
        {
            List<HoSoVuAnModel> danhSachNgay = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoSoVuAnID", hoSoVuAnID));
                listParameter.Add(new SqlParameter("@GiaiDoan", giaiDoan));

                danhSachNgay = DBUtils.ExecuteSPList<HoSoVuAnModel>("SP_NhanDon_HoSoVuAn_Get_Danh_Sach_Ngay_Sua_Doi", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return danhSachNgay;
        }

        public NhanVienModel ThongTinNhanVien(string maNV)
        {
            NhanVienModel dbModel = null;
            try
            {
                var parameters = new List<SqlParameter> { new SqlParameter("@MaNV", maNV) };
                dbModel = DBUtils.ExecuteSP<NhanVienModel>("SP_NhanVien_ChiTiet", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (dbModel == null)
                return new NhanVienModel();

            return dbModel;
        }

        public IEnumerable<NhanVienModel> DanhSachNhanVien(string nhomChucNang)
        {
            List<NhanVienModel> danhSachNhanVien = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@NhomChucNang", nhomChucNang));

                danhSachNhanVien = DBUtils.ExecuteSPList<NhanVienModel>("SP_XetXu_NoiDung_DanhSachNhanVien_Theo_NhomChucNang", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return danhSachNhanVien;
        }

        public IEnumerable<DuongSuModel> NguyenDonVaBiDon(int hoSoVuAnID)
        {
            List<DuongSuModel> danhSachDuongSu = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoSoVuAnID", hoSoVuAnID));

                danhSachDuongSu = DBUtils.ExecuteSPList<DuongSuModel>("SP_NhanDon_HoSoVuAn_NguyenDon_BiDon", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return danhSachDuongSu;
        }

        public IEnumerable<DuongSuModel> DanhSachDuongSu(int ToaAnID)
        {
            List<DuongSuModel> danhSachDuongSu = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@ToaAnID", ToaAnID));

                danhSachDuongSu = DBUtils.ExecuteSPList<DuongSuModel>("SP_NhanDon_HoSoVuAn_DanhSachDuongSu", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return danhSachDuongSu;
        }

        public HoSoVuAnModel ChiTietHoSoVuAn(int hoSoVuAnID)
        {
            HoSoVuAnModel dbModel = null;
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@HoSoVuAnID", hoSoVuAnID)
                };
                dbModel = DBUtils.ExecuteSP<HoSoVuAnModel>("SP_NhanDon_HoSoVuAn_ChiTiet", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }

        public HoSoVuAnModel ChiTietHoSoVuAnTheoGiaiDoan(int hoSoVuAnID, int giaiDoan)
        {
            HoSoVuAnModel dbModel = null;
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@HoSoVuAnID", hoSoVuAnID),
                    new SqlParameter("@GiaiDoan", giaiDoan)
                };
                dbModel = DBUtils.ExecuteSP<HoSoVuAnModel>("SP_NhanDon_HoSoVuAn_ChiTiet_TheoGiaiDoan", parameters, AppName.BizSecurity);
			}
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }

        public HoSoVuAnModel ChiTietHoSoVuAnTheoLog(int id)
        {
            HoSoVuAnModel dbModel = null;
            try
            {
                var parameters = new List<SqlParameter> { new SqlParameter("@HoSoVuAnLogID", id) };
                dbModel = DBUtils.ExecuteSP<HoSoVuAnModel>("SP_NhanDon_HoSoVuAn_ChiTiet_Theo_Log", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }

        public IEnumerable<CongDoanHoSoModel> TongSoCongDoanHoSo()
        {
            List<CongDoanHoSoModel> danhSach = null;

            List<SqlParameter> listParameter = new List<SqlParameter>();
            danhSach = DBUtils.ExecuteSPList<CongDoanHoSoModel>("SP_NhanDon_HoSoVuAn_Count", listParameter, AppName.BizSecurity);

            return danhSach;
        }

        public IEnumerable<CongDoanHoSoModel> CongDoanHoSo(HoSoVuAnModel model, string maNV)
        {
            List<CongDoanHoSoModel> danhSach = null;

            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@Keyword", model.MaHoSo));
                listParameter.Add(new SqlParameter("@NgayTaoTu", model.NgayTao));
                listParameter.Add(new SqlParameter("@NgayTaoDen", model.NgayCapNhat));
                listParameter.Add(new SqlParameter("@TrangThai", model.TrangThai));
                listParameter.Add(new SqlParameter("@LoaiDon", model.LoaiDon));
                listParameter.Add(new SqlParameter("@LoaiQuanHe", model.LoaiQuanHe));
                listParameter.Add(new SqlParameter("@CanBoNhanDon", model.CanBoNhanDon));
                listParameter.Add(new SqlParameter("@YeuToNuocNgoai", model.YeuToNuocNgoai));
                listParameter.Add(new SqlParameter("@DuongSu", model.HinhThucGoiDon == null ? 0 : int.Parse(model.HinhThucGoiDon)));
                listParameter.Add(new SqlParameter("@ToaAnID", model.ToaAnID));
                listParameter.Add(new SqlParameter("@NhomAn", model.NhomAn));
                listParameter.Add(new SqlParameter("@GiaiDoan", model.GiaiDoan));
                listParameter.Add(new SqlParameter("@CongDoanHoSo", model.CongDoanHoSo));
                listParameter.Add(new SqlParameter("@MaNV", maNV));
                danhSach = DBUtils.ExecuteSPList<CongDoanHoSoModel>("SP_NhanDon_HoSoVuAn_Count_TheoCongDoan", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return danhSach;
        }

        public IEnumerable<HoSoVuAnModel> DanhSachHoSoVuAn(HoSoVuAnModel model, string maNV)
        {
            List<HoSoVuAnModel> danhSach = null;
            
            List<SqlParameter> listParameter = new List<SqlParameter>();
            listParameter.Add(new SqlParameter("@Keyword", model.MaHoSo));
            listParameter.Add(new SqlParameter("@NgayTaoTu", model.NgayTao));
            listParameter.Add(new SqlParameter("@NgayTaoDen", model.NgayCapNhat));
            listParameter.Add(new SqlParameter("@TrangThai", model.TrangThai));
            listParameter.Add(new SqlParameter("@LoaiDon", model.LoaiDon));
            listParameter.Add(new SqlParameter("@LoaiQuanHe", model.LoaiQuanHe));
            listParameter.Add(new SqlParameter("@CanBoNhanDon", model.CanBoNhanDon));
            listParameter.Add(new SqlParameter("@YeuToNuocNgoai", model.YeuToNuocNgoai));
            listParameter.Add(new SqlParameter("@DuongSu", model.HinhThucGoiDon == null ? 0 : int.Parse(model.HinhThucGoiDon) ) );
            listParameter.Add(new SqlParameter("@ToaAnID", model.ToaAnID));
            listParameter.Add(new SqlParameter("@NhomAn", model.NhomAn));
            listParameter.Add(new SqlParameter("@GiaiDoan", model.GiaiDoan));
            listParameter.Add(new SqlParameter("@CongDoanHoSo", model.CongDoanHoSo));
            listParameter.Add(new SqlParameter("@MaNV", maNV));
            danhSach = DBUtils.ExecuteSPList<HoSoVuAnModel>("SP_NhanDon_HoSoVuAn_DanhSach", listParameter, AppName.BizSecurity);
            
            return danhSach;
        }

        public ResponseResult ThemHoSoVuAn(HoSoVuAnModel model)
        {
            ResponseResult result = null;

            List<SqlParameter> listParameter = new List<SqlParameter>();
            listParameter.Add(new SqlParameter("@NhomAn", model.NhomAn));
            listParameter.Add(new SqlParameter("@ToaAnID", model.ToaAnID));
            listParameter.Add(new SqlParameter("@TrangThaiCongDoan", model.TrangThaiCongDoan));
            listParameter.Add(new SqlParameter("@NgayLamDon", model.NgayLamDon));
            listParameter.Add(new SqlParameter("@NgayNopDonTaiToaAn", model.NgayNopDonTaiToaAn));
            listParameter.Add(new SqlParameter("@HinhThucGoiDon", model.HinhThucGoiDon));
            listParameter.Add(new SqlParameter("@LoaiDon", model.LoaiDon));
            listParameter.Add(new SqlParameter("@LoaiQuanHe", model.LoaiQuanHe));
            listParameter.Add(new SqlParameter("@QuanHePhapLuat", model.QuanHePhapLuat));
            listParameter.Add(new SqlParameter("@YeuToNuocNgoai", model.YeuToNuocNgoai));
            listParameter.Add(new SqlParameter("@NguoiKyXacNhanDaNhanDon", model.NguoiKyXacNhanDaNhanDon));
            listParameter.Add(new SqlParameter("@CanBoNhanDon", model.CanBoNhanDon));
            listParameter.Add(new SqlParameter("@NguoiCapNhat", model.NguoiCapNhat));
            listParameter.Add(new SqlParameter("@NguoiTao", model.NguoiTao));
            listParameter.Add(new SqlParameter("@NoiDungDon", model.NoiDungDon));

            result = DBUtils.ExecuteSP<ResponseResult>("SP_NhanDon_HoSoVuAn_Them", listParameter, AppName.BizSecurity);
            

            return result;
        }

        public ResponseResult SuaHoSoVuAn(HoSoVuAnModel model)
        {
            ResponseResult result = null;

            List<SqlParameter> listParameter = new List<SqlParameter>();
            listParameter.Add(new SqlParameter("@HoSoVuAnID", model.HoSoVuAnID));
            listParameter.Add(new SqlParameter("@NgayLamDon", model.NgayLamDon));
            listParameter.Add(new SqlParameter("@NgayNopDonTaiToaAn", model.NgayNopDonTaiToaAn));
            listParameter.Add(new SqlParameter("@HinhThucGoiDon", model.HinhThucGoiDon));
            listParameter.Add(new SqlParameter("@LoaiDon", model.LoaiDon));
            listParameter.Add(new SqlParameter("@LoaiQuanHe", model.LoaiQuanHe));
            listParameter.Add(new SqlParameter("@QuanHePhapLuat", model.QuanHePhapLuat));            
            listParameter.Add(new SqlParameter("@CanBoNhanDon", model.CanBoNhanDon));
            listParameter.Add(new SqlParameter("@YeuToNuocNgoai", model.YeuToNuocNgoai));
            listParameter.Add(new SqlParameter("@NguoiKyXacNhanDaNhanDon", model.NguoiKyXacNhanDaNhanDon));
            listParameter.Add(new SqlParameter("@NguoiCapNhat", model.NguoiCapNhat));

            result = DBUtils.ExecuteSP<ResponseResult>("SP_NhanDon_HoSoVuAn_Sua", listParameter, AppName.BizSecurity);
			
			return result;
        }       

        public ResponseResult ChuyenTrangThai(HoSoVuAnModel model)
        {
            ResponseResult result = null;

            List<SqlParameter> listParameter = new List<SqlParameter>();
            listParameter.Add(new SqlParameter("@HoSoVuAnID", model.HoSoVuAnID));
            listParameter.Add(new SqlParameter("@TrangThai", model.TrangThai));

            result = DBUtils.ExecuteSP<ResponseResult>("SP_NhanDon_HoSoVuAn_ChuyenTrangThai", listParameter, AppName.BizSecurity);

            return result;
        }

        public ResponseResult ChuyenCongDoanHoSo(HoSoVuAnModel model)
        {
            ResponseResult result = null;

            List<SqlParameter> listParameter = new List<SqlParameter>();
            listParameter.Add(new SqlParameter("@HoSoVuAnID", model.HoSoVuAnID));
            listParameter.Add(new SqlParameter("@CongDoanHoSo", model.CongDoanHoSo));
            listParameter.Add(new SqlParameter("@NguoiTao", model.NguoiTao));

            result = DBUtils.ExecuteSP<ResponseResult>("SP_NhanDon_HoSoVuAn_ChuyenCongDoanHoSo", listParameter, AppName.BizSecurity);

            return result;
        }
        #endregion

        #region DuongSu
        
        public IEnumerable<DuongSuModel> GetDanhSachDuongSu(int hoSoVuAnId, string duongSuLa = null, string tuCachThamGiaToTung = null)
        {
            List<DuongSuModel> listDuongSu = null;
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@HoSoVuAnID", hoSoVuAnId),
                    new SqlParameter("@DuongSuLa", duongSuLa),
                    new SqlParameter("@TuCachThamGiaToTung", tuCachThamGiaToTung)
                };
                listDuongSu = DBUtils.ExecuteSPList<DuongSuModel>("SP_NhanDon_DuongSu_DanhSachDuongSu", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listDuongSu;
        }

        public DuongSuModel ChiTietDuongSuTheoId(int duongSuId)
        {
            DuongSuModel dbModel = null;
            try
            {
                var parameters = new List<SqlParameter> { new SqlParameter("@DuongSuID", duongSuId) };
                dbModel = DBUtils.ExecuteSP<DuongSuModel>("SP_NhanDon_DuongSu_ThongTinChiTietTheoID", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }
		
        public ResponseResult TaoDuongSu(DuongSuModel dbModel)
        {
            ResponseResult result = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoSoVuAnID", dbModel.HoSoVuAnID));
                listParameter.Add(new SqlParameter("@TenCoQuanToChuc", dbModel.TenCoQuanToChuc));
                listParameter.Add(new SqlParameter("@HoVaTen", dbModel.HoVaTen));
                listParameter.Add(new SqlParameter("@SoCMND", dbModel.SoCMND));
                listParameter.Add(new SqlParameter("@DanToc", dbModel.DanToc));
                listParameter.Add(new SqlParameter("@NgayThangNamSinh", dbModel.NgayThangNamSinh));
                listParameter.Add(new SqlParameter("@GioiTinh", dbModel.GioiTinh));
                listParameter.Add(new SqlParameter("@SoDienThoai", dbModel.SoDienThoai));
                listParameter.Add(new SqlParameter("@QuocTich", dbModel.QuocTich));
                listParameter.Add(new SqlParameter("@NoiDKHKTT", dbModel.NoiDKHKTT));
                listParameter.Add(new SqlParameter("@NoiTamTru", dbModel.NoiTamTru));
                listParameter.Add(new SqlParameter("@TuCachThamGiaToTung", dbModel.TuCachThamGiaToTung));
                listParameter.Add(new SqlParameter("@DuongSuLa", dbModel.DuongSuLa));
                listParameter.Add(new SqlParameter("@NhomNghiepVu", dbModel.NhomNghiepVu));
                listParameter.Add(new SqlParameter("@CongDoanHoSo", dbModel.CongDoanHoSo));
                listParameter.Add(new SqlParameter("@NguoiTao", dbModel.NguoiTao));
                listParameter.Add(new SqlParameter("@GhiChu", dbModel.GhiChu));

                listParameter.Add(new SqlParameter("@TonGiao", dbModel.TonGiao));
                listParameter.Add(new SqlParameter("@NgheNghiep", dbModel.NgheNghiep));
                listParameter.Add(new SqlParameter("@TrinhDoVanHoa", dbModel.TrinhDoVanHoa));
                listParameter.Add(new SqlParameter("@NgayCapCMND", dbModel.NgayCapCMND));
                listParameter.Add(new SqlParameter("@NoiCapCMND", dbModel.NoiCapCMND));
                listParameter.Add(new SqlParameter("@HoTenNguoiGiamHo", dbModel.HoTenNguoiGiamHo));
                listParameter.Add(new SqlParameter("@DiaChiNguoiGiamHo", dbModel.DiaChiNguoiGiamHo));
                listParameter.Add(new SqlParameter("@HoTenCha", dbModel.HoTenCha));
                listParameter.Add(new SqlParameter("@DiaChiCha", dbModel.DiaChiCha));
                listParameter.Add(new SqlParameter("@HoTenMe", dbModel.HoTenMe));
                listParameter.Add(new SqlParameter("@DiaChiMe", dbModel.DiaChiMe));
                listParameter.Add(new SqlParameter("@TienAn", dbModel.TienAn));
                listParameter.Add(new SqlParameter("@TienSu", dbModel.TienSu));
                listParameter.Add(new SqlParameter("@NamSinhCha", dbModel.NamSinhCha));
                listParameter.Add(new SqlParameter("@NamSinhMe", dbModel.NamSinhMe));
                listParameter.Add(new SqlParameter("@ChaDaChetHayChua", dbModel.ChaDaChetHayChua));
                listParameter.Add(new SqlParameter("@MeDaChetHayChua", dbModel.MeDaChetHayChua));
                listParameter.Add(new SqlParameter("@TenGoiKhac", dbModel.TenGoiKhac));
                listParameter.Add(new SqlParameter("@DacDiemNhanThanBiCao", dbModel.DacDiemNhanThanBiCao));
                listParameter.Add(new SqlParameter("@LuuYVeChucVu", dbModel.LuuYVeChucVu));
                listParameter.Add(new SqlParameter("@TinhTrangGiamGiu", dbModel.TinhTrangGiamGiu));
                listParameter.Add(new SqlParameter("@NgayBatTamGiam", dbModel.NgayBatTamGiam));
                listParameter.Add(new SqlParameter("@ToiDanhTruyTo", dbModel.ToiDanhTruyTo));
                listParameter.Add(new SqlParameter("@DieuLuatTruyTo", dbModel.DieuLuatTruyTo));
                listParameter.Add(new SqlParameter("@VanPhongLuatSu", dbModel.VanPhongLuatSu));
                listParameter.Add(new SqlParameter("@DoanLuatSu", dbModel.DoanLuatSu));
                listParameter.Add(new SqlParameter("@NoiCongTac", dbModel.NoiCongTac));
                listParameter.Add(new SqlParameter("@NguoiDaiDienCua", dbModel.NguoiDaiDienCua));
                listParameter.Add(new SqlParameter("@QuanHeVoiNguoiThamGiaToTung", dbModel.QuanHeVoiNguoiThamGiaToTung));
                listParameter.Add(new SqlParameter("@ChucVu", dbModel.ChucVu));
                listParameter.Add(new SqlParameter("@DacDiemNhanThanBiHai", dbModel.DacDiemNhanThanBiHai));
                listParameter.Add(new SqlParameter("@NguoiBaoChuaLa", dbModel.NguoiBaoChuaLa));
                listParameter.Add(new SqlParameter("@NoiSinh", dbModel.NoiSinh));

                result = DBUtils.ExecuteSP<ResponseResult>("SP_NhanDon_DuongSu_TaoDuongSu", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }
       
        public ResponseResult SuaDuongSu(DuongSuModel dbModel)
        {
            ResponseResult result = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoSoVuAnID", dbModel.HoSoVuAnID));
                listParameter.Add(new SqlParameter("@TenCoQuanToChuc", dbModel.TenCoQuanToChuc));
                listParameter.Add(new SqlParameter("@DuongSuID", dbModel.DuongSuID));
                listParameter.Add(new SqlParameter("@HoVaTen", dbModel.HoVaTen));
                listParameter.Add(new SqlParameter("@SoCMND", dbModel.SoCMND));
                listParameter.Add(new SqlParameter("@DanToc", dbModel.DanToc));
                listParameter.Add(new SqlParameter("@NgayThangNamSinh", dbModel.NgayThangNamSinh));
                listParameter.Add(new SqlParameter("@GioiTinh", dbModel.GioiTinh));
                listParameter.Add(new SqlParameter("@SoDienThoai", dbModel.SoDienThoai));
                listParameter.Add(new SqlParameter("@QuocTich", dbModel.QuocTich));
                listParameter.Add(new SqlParameter("@NoiDKHKTT", dbModel.NoiDKHKTT));
                listParameter.Add(new SqlParameter("@NoiTamTru", dbModel.NoiTamTru));
                listParameter.Add(new SqlParameter("@TuCachThamGiaToTung", dbModel.TuCachThamGiaToTung));
                listParameter.Add(new SqlParameter("@DuongSuLa", dbModel.DuongSuLa));
                listParameter.Add(new SqlParameter("@NguoiTao", dbModel.NguoiTao));
                listParameter.Add(new SqlParameter("@GhiChu", dbModel.GhiChu));

                listParameter.Add(new SqlParameter("@TonGiao", dbModel.TonGiao));
                listParameter.Add(new SqlParameter("@NgheNghiep", dbModel.NgheNghiep));
                listParameter.Add(new SqlParameter("@TrinhDoVanHoa", dbModel.TrinhDoVanHoa));
                listParameter.Add(new SqlParameter("@NgayCapCMND", dbModel.NgayCapCMND));
                listParameter.Add(new SqlParameter("@NoiCapCMND", dbModel.NoiCapCMND));
                listParameter.Add(new SqlParameter("@HoTenNguoiGiamHo", dbModel.HoTenNguoiGiamHo));
                listParameter.Add(new SqlParameter("@DiaChiNguoiGiamHo", dbModel.DiaChiNguoiGiamHo));
                listParameter.Add(new SqlParameter("@HoTenCha", dbModel.HoTenCha));
                listParameter.Add(new SqlParameter("@DiaChiCha", dbModel.DiaChiCha));
                listParameter.Add(new SqlParameter("@HoTenMe", dbModel.HoTenMe));
                listParameter.Add(new SqlParameter("@DiaChiMe", dbModel.DiaChiMe));
                listParameter.Add(new SqlParameter("@TienAn", dbModel.TienAn));
                listParameter.Add(new SqlParameter("@TienSu", dbModel.TienSu));
                listParameter.Add(new SqlParameter("@NamSinhCha", dbModel.NamSinhCha));
                listParameter.Add(new SqlParameter("@NamSinhMe", dbModel.NamSinhMe));
                listParameter.Add(new SqlParameter("@ChaDaChetHayChua", dbModel.ChaDaChetHayChua));
                listParameter.Add(new SqlParameter("@MeDaChetHayChua", dbModel.MeDaChetHayChua));
                listParameter.Add(new SqlParameter("@TenGoiKhac", dbModel.TenGoiKhac));
                listParameter.Add(new SqlParameter("@DacDiemNhanThanBiCao", dbModel.DacDiemNhanThanBiCao));
                listParameter.Add(new SqlParameter("@LuuYVeChucVu", dbModel.LuuYVeChucVu));
                listParameter.Add(new SqlParameter("@TinhTrangGiamGiu", dbModel.TinhTrangGiamGiu));
                listParameter.Add(new SqlParameter("@NgayBatTamGiam", dbModel.NgayBatTamGiam));
                listParameter.Add(new SqlParameter("@ToiDanhTruyTo", dbModel.ToiDanhTruyTo));
                listParameter.Add(new SqlParameter("@DieuLuatTruyTo", dbModel.DieuLuatTruyTo));
                listParameter.Add(new SqlParameter("@VanPhongLuatSu", dbModel.VanPhongLuatSu));
                listParameter.Add(new SqlParameter("@DoanLuatSu", dbModel.DoanLuatSu));
                listParameter.Add(new SqlParameter("@NoiCongTac", dbModel.NoiCongTac));
                listParameter.Add(new SqlParameter("@NguoiDaiDienCua", dbModel.NguoiDaiDienCua));
                listParameter.Add(new SqlParameter("@QuanHeVoiNguoiThamGiaToTung", dbModel.QuanHeVoiNguoiThamGiaToTung));
                listParameter.Add(new SqlParameter("@ChucVu", dbModel.ChucVu));
                listParameter.Add(new SqlParameter("@DacDiemNhanThanBiHai", dbModel.DacDiemNhanThanBiHai));
                listParameter.Add(new SqlParameter("@NguoiBaoChuaLa", dbModel.NguoiBaoChuaLa));
                listParameter.Add(new SqlParameter("@NoiSinh", dbModel.NoiSinh));

                result = DBUtils.ExecuteSP<ResponseResult>("SP_NhanDon_DuongSu_SuaDuongSu", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }
      
        public ResponseResult XoaDuongSu(int duongSuId)
        {
            ResponseResult result = null;
            try
            {
                var parameters = new List<SqlParameter> { new SqlParameter("@DuongSuID", duongSuId) };
                result = DBUtils.ExecuteSP<ResponseResult>("SP_NhanDon_DuongSu_XoaDuongSu", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public ResponseResult CapNhatBiCanBiCaoHinhSu(int hoSoVuAnId)
        {
            ResponseResult result = null;
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@HoSoVuAnID", hoSoVuAnId)
                };
                result = DBUtils.ExecuteSP<ResponseResult>("SP_NhanDon_DuongSu_CapNhatBiCanBiCaoHinhSu", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        #endregion

        #region NoiDungDon
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hoSoVuAnId"></param>
        /// <param name="giaiDoan"></param>
        /// <returns></returns>
        public IEnumerable<NoiDungDonModel> GetDanhSachNgaySuaDoiNoiDungDon(int hoSoVuAnId, int giaiDoan)
        {
            List<NoiDungDonModel> listNgaySuaDoi = null;
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@HoSoVuAnID", hoSoVuAnId),
                    new SqlParameter("@GiaiDoan", giaiDoan)
                };
                listNgaySuaDoi = DBUtils.ExecuteSPList<NoiDungDonModel>("SP_NhanDon_NoiDungDon_Get_DanhSachNgaySuaDoi", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listNgaySuaDoi;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="noiDungDonId"></param>
        /// <returns></returns>
        public NoiDungDonModel ChiTietNoiDungDonTheoId(int noiDungDonId)
        {
            NoiDungDonModel dbModel = null;
            try
            {
                var parameters = new List<SqlParameter> { new SqlParameter("@NoiDungDonID", noiDungDonId) };
                dbModel = DBUtils.ExecuteSP<NoiDungDonModel>("SP_NhanDon_NoiDungDon_Get_ThongTinChiTietTheoId", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbModel"></param>
        /// <returns></returns>
        public ResponseResult SuaDoiNoiDungDon(NoiDungDonModel dbModel)
        {
            ResponseResult result = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoSoVuAnID", dbModel.HoSoVuAnID));
                listParameter.Add(new SqlParameter("@NoiDungDon", dbModel.NoiDungDon));
                listParameter.Add(new SqlParameter("@NhomNghiepVu", dbModel.NhomNghiepVu));
                listParameter.Add(new SqlParameter("@GiaiDoan", dbModel.GiaiDoan));
                listParameter.Add(new SqlParameter("@CongDoanHoSo", dbModel.CongDoanHoSo));
                listParameter.Add(new SqlParameter("@NguoiTao", dbModel.NguoiTao));
                listParameter.Add(new SqlParameter("@GhiChu", dbModel.GhiChu));

                result = DBUtils.ExecuteSP<ResponseResult>("SP_NhanDon_NoiDungDon_Them", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        #endregion

        #region Chuyen Don
        public ChuyenDonModel ChiTietChuyenDonTheoHoSoVuAnID(int hoSoVuAnID, int giaiDoan, int congDoan)
        {
            ChuyenDonModel dbModel = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoSoVuAnID", hoSoVuAnID));
                listParameter.Add(new SqlParameter("@GiaiDoan", giaiDoan));
                listParameter.Add(new SqlParameter("@CongDoanHoSo", congDoan));

                dbModel = DBUtils.ExecuteSP<ChuyenDonModel>("SP_NhanDon_ChuyenDon_ChiTiet_Theo_HoSoVuAnID", listParameter,
                    AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }
        
        public IEnumerable<ChuyenDonModel> GetDanhSachNgaySuaDoiChuyenDon(int hoSoVuAnId, int giaiDoan, int congDoan)
        {
            List<ChuyenDonModel> listNgaySuaDoi = null;
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@HoSoVuAnID", hoSoVuAnId),
                    new SqlParameter("@GiaiDoan", giaiDoan),
                    new SqlParameter("@CongDoanHoSo", congDoan)
                };
                listNgaySuaDoi = DBUtils.ExecuteSPList<ChuyenDonModel>("SP_NhanDon_ChuyenDon_Get_DanhSachNgaySuaDoi_TheoCongDoan", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listNgaySuaDoi;
        }
        
        public ChuyenDonModel ChiTietChuyenDonTheoId(int chuyenDonId)
        {
            ChuyenDonModel dbModel = null;
            try
            {
                var parameters = new List<SqlParameter> { new SqlParameter("@ChuyenDonID", chuyenDonId) };
                dbModel = DBUtils.ExecuteSP<ChuyenDonModel>("SP_NhanDon_ChuyenDon_Get_ThongTinChiTietTheoId", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }
        
        public ResponseResult SuaDoiChuyenDon(ChuyenDonModel dbModel)
        {
            ResponseResult result = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoSoVuAnID", dbModel.HoSoVuAnID));
                listParameter.Add(new SqlParameter("@NgayRaThongBao", dbModel.NgayRaThongBao));
                listParameter.Add(new SqlParameter("@NgayChuyenDon", dbModel.NgayChuyenDon));
                listParameter.Add(new SqlParameter("@ToaAnChuyenDi", dbModel.ToaAnChuyenDi));
                listParameter.Add(new SqlParameter("@ToaAnChuyenDen", dbModel.ToaAnChuyenDen));
                listParameter.Add(new SqlParameter("@LyDoChuyenDon", dbModel.LyDoChuyenDon));
                listParameter.Add(new SqlParameter("@TruongHopChuyen", dbModel.TruongHopChuyen));
                listParameter.Add(new SqlParameter("@NhomNghiepVu", dbModel.NhomNghiepVu));
                listParameter.Add(new SqlParameter("@GiaiDoan", dbModel.GiaiDoan));
                listParameter.Add(new SqlParameter("@CongDoanHoSo", dbModel.CongDoanHoSo));
                listParameter.Add(new SqlParameter("@NguoiTao", dbModel.NguoiTao));
                listParameter.Add(new SqlParameter("@GhiChu", dbModel.GhiChu));
                listParameter.Add(new SqlParameter("@TrangThaiCongDoan", dbModel.TrangThaiCongDoan));

                result = DBUtils.ExecuteSP<ResponseResult>("SP_NhanDon_ChuyenDon_Them", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }
        #endregion

        #region Tham Phan
        public ThamPhanModel ChiTietThamPhanTheoHoSoVuAnID(int hoSoVuAnID)
        {
            ThamPhanModel dbModel = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoSoVuAnID", hoSoVuAnID));

                dbModel = DBUtils.ExecuteSP<ThamPhanModel>("SP_NhanDon_ThamPhan_ChiTiet_Theo_HoSoVuAnID", listParameter,
                    AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }
        
        public IEnumerable<ThamPhanModel> GetDanhSachNgaySuaDoiThamPhan(int hoSoVuAnId, int giaiDoan)
        {
            List<ThamPhanModel> listNgaySuaDoi = null;
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@HoSoVuAnID", hoSoVuAnId),
                    new SqlParameter("@GiaiDoan", giaiDoan)
                };
                listNgaySuaDoi = DBUtils.ExecuteSPList<ThamPhanModel>("SP_NhanDon_ThamPhan_Get_DanhSachNgaySuaDoi", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listNgaySuaDoi;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thamPhanId"></param>
        /// <returns></returns>
        public ThamPhanModel ChiTietThamPhanTheoId(int thamPhanId)
        {
            ThamPhanModel dbModel = null;
            try
            {
                var parameters = new List<SqlParameter> { new SqlParameter("@ThamPhanID", thamPhanId) };
                dbModel = DBUtils.ExecuteSP<ThamPhanModel>("SP_NhanDon_ThamPhan_Get_ThongTinChiTietTheoId", parameters,
                    AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }

        public ResponseResult SuaDoiThamPhan(ThamPhanModel dbModel)
        {
            ResponseResult resutl = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoSoVuAnID",dbModel.HoSoVuAnID));
                listParameter.Add(new SqlParameter("@ThamPhan", dbModel.ThamPhan));
                listParameter.Add(new SqlParameter("@NgayPhanCong", dbModel.NgayPhanCong));
                listParameter.Add(new SqlParameter("@TenNguoiPhanCong", dbModel.TenNguoiPhanCong));
                listParameter.Add(new SqlParameter("@HoiThamNhanDan2", dbModel.HoiThamNhanDan2));
                listParameter.Add(new SqlParameter("@NguoiTao", dbModel.NguoiTao));
                listParameter.Add(new SqlParameter("@GhiChu", dbModel.GhiChu));

                resutl = DBUtils.ExecuteSP<ResponseResult>("SP_NhanDon_ThamPhan_Them", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return resutl;
        }
        #endregion

        #region SuaDoiBoSungDon
        public IEnumerable<SuaDoiBoSungDonModel> DanhSachNgaySuaDoiBoSungDon(int hoSoVuAnID, int giaiDoan)
        {
            List<SuaDoiBoSungDonModel> danhSachNgay = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoSoVuAnID", hoSoVuAnID));
                listParameter.Add(new SqlParameter("@GiaiDoan", giaiDoan));

                danhSachNgay = DBUtils.ExecuteSPList<SuaDoiBoSungDonModel>("SP_NhanDon_SuaDoiBoSungDon_Get_Danh_Sach_Ngay_Sua_Doi", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return danhSachNgay;
        }

        public SuaDoiBoSungDonModel ThongTinSuaDoiBoSungDonTheoHoSoVuAnID(int hoSoVuAnID)
        {
            SuaDoiBoSungDonModel dbModel = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoSoVuAnID", hoSoVuAnID));

                dbModel = DBUtils.ExecuteSP<SuaDoiBoSungDonModel>("SP_NhanDon_SuaDoiBoSungDon_ChiTiet_Theo_HoSoVuAn", listParameter,
                    AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }

        public SuaDoiBoSungDonModel ThongTinSuaDoiBoSungDonTheoSuaDoiBoSungDonID(int id)
        {
            SuaDoiBoSungDonModel dbModel = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@SuaDoiBoSungDonID", id));

                dbModel = DBUtils.ExecuteSP<SuaDoiBoSungDonModel>("SP_NhanDon_SuaDoiBoSungDon_ChiTiet_Theo_ID", listParameter,
                    AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }

        public ResponseResult ThemSuaDoiBoSungDon(SuaDoiBoSungDonModel model)
        {
            ResponseResult result = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoSoVuAnID", model.HoSoVuAnID));
                listParameter.Add(new SqlParameter("@NgayYeuCau", model.NgayYeuCau));
                listParameter.Add(new SqlParameter("@ThoiHanSuaDoi", model.ThoiHanSuaDoi));
                listParameter.Add(new SqlParameter("@NoiDungYeuCau", model.NoiDungYeuCau));
                listParameter.Add(new SqlParameter("@NguoiTao", model.NguoiTao));

                result = DBUtils.ExecuteSP<ResponseResult>("SP_NhanDon_SuaDoiBoSungDon_Them", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }
        #endregion

        #region TraLaiDon
        public IEnumerable<TraLaiDonModel> DanhSachNgayTraLaiDon(int hoSoVuAnID, int giaiDoan, int CongDoan)
        {
            List<TraLaiDonModel> danhSachNgay = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoSoVuAnID", hoSoVuAnID));
                listParameter.Add(new SqlParameter("@GiaiDoan", giaiDoan));
                listParameter.Add(new SqlParameter("@CongDoan", CongDoan));
                danhSachNgay = DBUtils.ExecuteSPList<TraLaiDonModel>("SP_NhanDon_TraLaiDon_Get_Danh_Sach_Ngay_Sua_Doi", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return danhSachNgay;
        }
        
        public TraLaiDonModel ThongTinTraLaiDonTheoHoSoVuAnID(int hoSoVuAnID, int CongDoan)
        {
            TraLaiDonModel dbModel = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoSoVuAnID", hoSoVuAnID));
                listParameter.Add(new SqlParameter("@CongDoan", CongDoan));
                dbModel = DBUtils.ExecuteSP<TraLaiDonModel>("SP_NhanDon_TraLaiDon_ChiTiet_Theo_HoSoVuAnID", listParameter, 
                    AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }

        public TraLaiDonModel ThongTinTraLaiDonTheoTraLaiDonID(int traLaiDonID)
        {
            TraLaiDonModel dbModel = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@TraLaiDonID", traLaiDonID));

                dbModel = DBUtils.ExecuteSP<TraLaiDonModel>("SP_NhanDon_TraLaiDon_ChiTiet_Theo_TraLaiDonID", listParameter,
                    AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }

        public ResponseResult ThemTraLaiDon(TraLaiDonModel model)
        {
            ResponseResult result = null; 
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoSoVuAnID", model.HoSoVuAnID));
                listParameter.Add(new SqlParameter("@NgayTraDon", model.NgayTraDon));
                listParameter.Add(new SqlParameter("@LyDoTraDon", model.LyDoTraDon));
                listParameter.Add(new SqlParameter("@GiaiDoan", model.GiaiDoan));
                listParameter.Add(new SqlParameter("@CongDoanHoSo", model.CongDoanHoSo));
                listParameter.Add(new SqlParameter("@NguoiTao", model.NguoiTao));
                listParameter.Add(new SqlParameter("@GhiChu", model.GhiChu));

                result = DBUtils.ExecuteSP<ResponseResult>("SP_NhanDon_TraLaiDon_Them", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        #endregion
		
		#region Khieu nai viec tra don
        public IEnumerable<KhieuNaiTraDonModel> DanhSachNgayKhieuNaiTraDon(int hoSoVuAnID, int giaiDoan)
        {
            List<KhieuNaiTraDonModel> danhSachNgay = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoSoVuAnID", hoSoVuAnID));
                listParameter.Add(new SqlParameter("@GiaiDoan", giaiDoan));

                danhSachNgay = DBUtils.ExecuteSPList<KhieuNaiTraDonModel>("SP_NhanDon_KhieuNaiTraDon_Get_Danh_Sach_Ngay_Sua_Doi", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return danhSachNgay;
        }

        public KhieuNaiTraDonModel ThongTinKhieuNaiTraDonTheoHoSoVuAnID(int hoSoVuAnID)
        {
            KhieuNaiTraDonModel dbModel = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoSoVuAnID", hoSoVuAnID));

                dbModel = DBUtils.ExecuteSP<KhieuNaiTraDonModel>("SP_NhanDon_KhieuNaiTraDon_ChiTiet_Theo_HoSoVuAnID", listParameter,
                    AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }

        public KhieuNaiTraDonModel ThongTinKhieuNaiTraDonTheoKhieuNaiTraDonID(int id)
        {
            KhieuNaiTraDonModel dbModel = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@KhieuNaiViecTraDonID", id));

                dbModel = DBUtils.ExecuteSP<KhieuNaiTraDonModel>("SP_NhanDon_KhieuNaiTraDon_ChiTiet_Theo_KhieuNaiTraDonID", listParameter,
                    AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }

        public ResponseResult ThemKhieuNaiTraDon(KhieuNaiTraDonModel model)
        {
            ResponseResult result = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoSoVuAnID", model.HoSoVuAnID));
                listParameter.Add(new SqlParameter("@Nhom", model.Nhom));
                listParameter.Add(new SqlParameter("@LanThu", model.LanThu));
                listParameter.Add(new SqlParameter("@NguoiKhieuNai", model.NguoiKhieuNai));
                listParameter.Add(new SqlParameter("@NgayKhieuNai", model.NgayKhieuNai));
                listParameter.Add(new SqlParameter("@NoiDungKhieuNai", model.NoiDungKhieuNai));
                listParameter.Add(new SqlParameter("@KetQuaGiaiQuyet", model.KetQuaGiaiQuyet));
                listParameter.Add(new SqlParameter("@GhiChu", model.GhiChu));
                listParameter.Add(new SqlParameter("@NguoiTao", model.NguoiTao));

                result = DBUtils.ExecuteSP<ResponseResult>("SP_NhanDon_KhieuNaiTraDon_Them", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }
        #endregion

        #region Kien nghi viec tra don

        public IEnumerable<KienNghiTraDonModel> DanhSachNgayKienNghiTraDon(int hoSoVuAnID, int giaiDoan)
        {
            List<KienNghiTraDonModel> danhSachNgay = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoSoVuAnID", hoSoVuAnID));
                listParameter.Add(new SqlParameter("@GiaiDoan", giaiDoan));

                danhSachNgay = DBUtils.ExecuteSPList<KienNghiTraDonModel>("SP_NhanDon_KienNghiTraDon_Get_Danh_Sach_Ngay_Sua_Doi", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return danhSachNgay;
        }

        public KienNghiTraDonModel ThongTinKienNghiTraDonTheoHoSoVuAnID(int hoSoVuAnID)
        {
            KienNghiTraDonModel dbModel;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoSoVuAnID", hoSoVuAnID));

                dbModel = DBUtils.ExecuteSP<KienNghiTraDonModel>("SP_NhanDon_KienNghiTraDon_ChiTiet_Theo_HoSoVuAnID", listParameter,
                    AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }

        public KienNghiTraDonModel ThongTinKienNghiTraDonTheoKienNghiTraDonID(int id)
        {
            KienNghiTraDonModel dbModel = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@KienNghiViecTraDonID", id));

                dbModel = DBUtils.ExecuteSP<KienNghiTraDonModel>("SP_NhanDon_KienNghiTraDon_ChiTiet_Theo_KienNghiTraDonID", listParameter,
                    AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }

        public ResponseResult ThemKienNghiTraDon(KienNghiTraDonModel model)
        {
            ResponseResult result = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoSoVuAnID", model.HoSoVuAnID));
                listParameter.Add(new SqlParameter("@Nhom", model.Nhom));
                listParameter.Add(new SqlParameter("@LanThu", model.LanThu));
                listParameter.Add(new SqlParameter("@VKSKienNghi", model.VKSKienNghi));
                listParameter.Add(new SqlParameter("@NgayKienNghi", model.NgayKienNghi));
                listParameter.Add(new SqlParameter("@NoiDungKienNghi", model.NoiDungKienNghi));
                listParameter.Add(new SqlParameter("@KetQuaGiaiQuyet", model.KetQuaGiaiQuyet));
                listParameter.Add(new SqlParameter("@GhiChu", model.GhiChu));
                listParameter.Add(new SqlParameter("@NguoiTao", model.NguoiTao));

                result = DBUtils.ExecuteSP<ResponseResult>("SP_NhanDon_KienNghiTraDon_Them", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }
        #endregion

        #region Searching on Header by Keyword
        public IEnumerable<HoSoVuAnModel> DanhSachHoSoVuAnSearchByKeyword(string keyword)
        {
            List<HoSoVuAnModel> danhSach = null;

            List<SqlParameter> listParameter = new List<SqlParameter>();
            listParameter.Add(new SqlParameter("@keyword", keyword));
            danhSach = DBUtils.ExecuteSPList<HoSoVuAnModel>("SP_TimKiem_HoSoVuAn_Theo_Keyword", listParameter, AppName.BizSecurity);

            return danhSach;
        }
        #endregion

        #region Nhan Ho So Phuc Tham
        public IEnumerable<NhanHoSoPhucThamModel> DanhSachHoSoChoPhucTham(string maNhomAn)
        {
            List<NhanHoSoPhucThamModel> danhSach = null;

            List<SqlParameter> listParameter = new List<SqlParameter>{ new SqlParameter("@NhomAn", maNhomAn) };
            danhSach = DBUtils.ExecuteSPList<NhanHoSoPhucThamModel>("SP_NhanHoSoPhucTham_DanhSachHoSoChoPhucTham", listParameter, AppName.BizSecurity);

            return danhSach;
        }
        public int GetSoLuongHoSoChoPhucTham(string maNhomAn)
        {
            List<SqlParameter> listParameter = new List<SqlParameter>
            {
                new SqlParameter("@NhomAn", maNhomAn),
                new SqlParameter { Direction = ParameterDirection.ReturnValue }
            };

            var SoLuongHoSo = DBUtils.ExecNonQuerySP("SP_NhanHoSoPhucTham_DanhSachHoSoChoPhucTham_Count", listParameter, AppName.BizSecurity);

            return Protect.ToInt16(SoLuongHoSo, 0);
        }
        public ResponseResult CapNhatNhanHoSoTuToaAnKhac(ChuyenDonModel dbModel)
        {
            ResponseResult result = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@ChuyenDonID", dbModel.ChuyenDonID));
                listParameter.Add(new SqlParameter("@HoSoVuAnID", dbModel.HoSoVuAnID));
                listParameter.Add(new SqlParameter("@GiaiDoan", dbModel.GiaiDoan));
                listParameter.Add(new SqlParameter("@CongDoanHoSo", dbModel.CongDoanHoSo));
                listParameter.Add(new SqlParameter("@NguoiTao", dbModel.NguoiTao));
                listParameter.Add(new SqlParameter("@GhiChu", dbModel.GhiChu));
                listParameter.Add(new SqlParameter("@TrangThaiCongDoan", dbModel.TrangThaiCongDoan));

                result = DBUtils.ExecuteSP<ResponseResult>("SP_NhanHoSo_CapNhatNhanHoSoTuToaAnKhac", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        #endregion

        #region NhanHoSo

        public IEnumerable<NhanHoSoModel> GetDanhSachNgaySuaDoiNhanHoSo(int hoSoVuAnId, int giaiDoan)
        {
            List<NhanHoSoModel> listNgaySuaDoi = null;
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@HoSoVuAnID", hoSoVuAnId),
                    new SqlParameter("@GiaiDoan", giaiDoan)
                };
                listNgaySuaDoi = DBUtils.ExecuteSPList<NhanHoSoModel>("SP_NhanHoSo_DanhSachNgaySuaDoi", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listNgaySuaDoi;
        }

        public NhanHoSoModel ChiTietNhanHoSoTheoId(int nhanHoSoId)
        {
            NhanHoSoModel dbModel = null;
            try
            {
                var parameters = new List<SqlParameter> { new SqlParameter("@NhanHoSoID", nhanHoSoId) };
                dbModel = DBUtils.ExecuteSP<NhanHoSoModel>("SP_NhanHoSo_ChiTiet_TheoId", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }

        public ResponseResult SuaNhanHoSo(NhanHoSoModel dbModel)
        {
            ResponseResult result = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoSoVuAnID", dbModel.HoSoVuAnID));
                listParameter.Add(new SqlParameter("@NgayNhanHoSo", dbModel.NgayNhanHoSo));
                listParameter.Add(new SqlParameter("@NguoiNhanHoSo", dbModel.NguoiNhanHoSo));
                listParameter.Add(new SqlParameter("@NhomNghiepVu", dbModel.NhomNghiepVu));
                listParameter.Add(new SqlParameter("@GiaiDoan", dbModel.GiaiDoan));
                listParameter.Add(new SqlParameter("@CongDoanHoSo", dbModel.CongDoanHoSo));
                listParameter.Add(new SqlParameter("@NguoiTao", dbModel.NguoiTao));
                listParameter.Add(new SqlParameter("@GhiChu", dbModel.GhiChu));

                result = DBUtils.ExecuteSP<ResponseResult>("SP_NhanHoSo_ChiTietNhanHoSo_Them", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }


        public ResponseResult XacNhanNhanHoSoPhucTham(HoSoVuAnModel dbModel)
        {
            ResponseResult result = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoSoVuAnID", dbModel.HoSoVuAnID));
                listParameter.Add(new SqlParameter("@ToaAnID", dbModel.ToaAnID));
                listParameter.Add(new SqlParameter("@CanBoNhanDon", dbModel.CanBoNhanDon));
                listParameter.Add(new SqlParameter("@TrangThaiCongDoan", dbModel.TrangThaiCongDoan));
                listParameter.Add(new SqlParameter("@NguoiTao", dbModel.NguoiTao));

                result = DBUtils.ExecuteSP<ResponseResult>("SP_NhanHoSoPhucTham_XacNhanNhanHoSo", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        #endregion

        #region HoSoVuAn NhomAnADBPXLHC

        public HoSoVuAnApDungModel ChiTietHoSoVuAnApDungBPXLHC(int hoSoVuAnID)
        {
            HoSoVuAnApDungModel dbModel = null;
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@HoSoVuAnID", hoSoVuAnID)
                };
                dbModel = DBUtils.ExecuteSP<HoSoVuAnApDungModel>("SP_NhanDon_HoSoVuAn_ChiTiet", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }

        public HoSoVuAnApDungModel ChiTietHoSoVuAnApDungBPXLHCTheoGiaiDoan(int hoSoVuAnID, int giaiDoan)
        {
            HoSoVuAnApDungModel dbModel = null;
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@HoSoVuAnID", hoSoVuAnID),
                    new SqlParameter("@GiaiDoan", giaiDoan)
                };
                dbModel = DBUtils.ExecuteSP<HoSoVuAnApDungModel>("SP_NhanDon_HoSoVuAn_ChiTiet_TheoGiaiDoan", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }

        public HoSoVuAnApDungModel ChiTietHoSoVuAnApDungBPXLHCTheoLog(int id)
        {
            HoSoVuAnApDungModel dbModel = null;
            try
            {
                var parameters = new List<SqlParameter> { new SqlParameter("@HoSoVuAnLogID", id) };
                dbModel = DBUtils.ExecuteSP<HoSoVuAnApDungModel>("SP_NhanDon_HoSoVuAn_ChiTiet_Theo_Log", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }

        public ResponseResult ThemHoSoVuAnApDungBPXLHC(HoSoVuAnApDungModel model)
        {
            ResponseResult result = null;

            List<SqlParameter> listParameter = new List<SqlParameter>();
            listParameter.Add(new SqlParameter("@ToaAnID", model.ToaAnID));
            listParameter.Add(new SqlParameter("@SoThuLy", model.SoThuLy));
            listParameter.Add(new SqlParameter("@NgayNhanHoSo", model.NgayNopDonTaiToaAn));
            listParameter.Add(new SqlParameter("@HinhThucNhanHoSo", model.HinhThucGoiDon));
            listParameter.Add(new SqlParameter("@NguoiNhanHoSo", model.CanBoNhanDon));
            listParameter.Add(new SqlParameter("@TenCoQuanDeNghi", model.TenCoQuanDeNghi));
            listParameter.Add(new SqlParameter("@HoSoDeNghi", model.HoSoDeNghi));
            listParameter.Add(new SqlParameter("@BienPhapXuLyHanhChinh", model.BienPhapXuLyHanhChinh));
            listParameter.Add(new SqlParameter("@ThoiHanApDung", model.ThoiHanApDung));
            listParameter.Add(new SqlParameter("@NguoiCapNhat", model.NguoiCapNhat));
            listParameter.Add(new SqlParameter("@NguoiTao", model.NguoiTao));
            listParameter.Add(new SqlParameter("@GhiChu", model.GhiChu));

            result = DBUtils.ExecuteSP<ResponseResult>("SP_NhanDon_HoSoVuAn_ApDungBPXLHC_Them", listParameter, AppName.BizSecurity);
            return result;
        }

        public ResponseResult SuaHoSoVuAnApDungBPXLHC(HoSoVuAnApDungModel model)
        {
            ResponseResult result = null;

            List<SqlParameter> listParameter = new List<SqlParameter>();
            listParameter.Add(new SqlParameter("@HoSoVuAnID", model.HoSoVuAnID));
            listParameter.Add(new SqlParameter("@SoThuLy", model.SoThuLy));
            listParameter.Add(new SqlParameter("@NgayNhanHoSo", model.NgayNopDonTaiToaAn));
            listParameter.Add(new SqlParameter("@HinhThucNhanHoSo", model.HinhThucGoiDon));
            listParameter.Add(new SqlParameter("@NguoiNhanHoSo", model.CanBoNhanDon));
            listParameter.Add(new SqlParameter("@TenCoQuanDeNghi", model.TenCoQuanDeNghi));
            listParameter.Add(new SqlParameter("@HoSoDeNghi", model.HoSoDeNghi));
            listParameter.Add(new SqlParameter("@BienPhapXuLyHanhChinh", model.BienPhapXuLyHanhChinh));
            listParameter.Add(new SqlParameter("@ThoiHanApDung", model.ThoiHanApDung));
            listParameter.Add(new SqlParameter("@GhiChu", model.GhiChu));
            listParameter.Add(new SqlParameter("@NguoiCapNhat", model.NguoiCapNhat));

            result = DBUtils.ExecuteSP<ResponseResult>("SP_NhanDon_HoSoVuAn_ApDungBPXLHC_Sua", listParameter, AppName.BizSecurity);

            return result;
        }

        #endregion

        #region HoSoVuAn NhomAn HinhSu

        public HoSoVuAnHinhSuModel ChiTietHoSoVuAnHinhSu(int hoSoVuAnID)
        {
            HoSoVuAnHinhSuModel dbModel = null;
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@HoSoVuAnID", hoSoVuAnID)
                };
                dbModel = DBUtils.ExecuteSP<HoSoVuAnHinhSuModel>("SP_NhanDon_HoSoVuAn_ChiTiet", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }

        public HoSoVuAnHinhSuModel ChiTietHoSoVuAnHinhSuTheoGiaiDoan(int hoSoVuAnID, int giaiDoan)
        {
            HoSoVuAnHinhSuModel dbModel = null;
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@HoSoVuAnID", hoSoVuAnID),
                    new SqlParameter("@GiaiDoan", giaiDoan)
                };
                dbModel = DBUtils.ExecuteSP<HoSoVuAnHinhSuModel>("SP_NhanDon_HoSoVuAn_ChiTiet_TheoGiaiDoan", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }

        public HoSoVuAnHinhSuModel ChiTietHoSoVuAnHinhSuTheoLog(int id)
        {
            HoSoVuAnHinhSuModel dbModel = null;
            try
            {
                var parameters = new List<SqlParameter> { new SqlParameter("@HoSoVuAnLogID", id) };
                dbModel = DBUtils.ExecuteSP<HoSoVuAnHinhSuModel>("SP_NhanDon_HoSoVuAn_ChiTiet_Theo_Log", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }

        public ResponseResult ThemHoSoVuAnHinhSu(HoSoVuAnHinhSuModel model)
        {
            ResponseResult result = null;

            List<SqlParameter> listParameter = new List<SqlParameter>();
            listParameter.Add(new SqlParameter("@ToaAnID", model.ToaAnID));
            listParameter.Add(new SqlParameter("@TenVuAn", model.TenVuAn));
            listParameter.Add(new SqlParameter("@SoBiCan", model.SoBiCan));
            listParameter.Add(new SqlParameter("@TongSoButLuc", model.TongSoButLuc));
            listParameter.Add(new SqlParameter("@KhongCoCacButLuc", model.KhongCoCacButLuc));
            listParameter.Add(new SqlParameter("@TruyToBang", model.TruyToBang));
            listParameter.Add(new SqlParameter("@SoTruyTo", model.SoTruyTo));
            listParameter.Add(new SqlParameter("@NgayTruyTo", model.NgayTruyTo));
            listParameter.Add(new SqlParameter("@VienKiemSatTruyTo", model.VienKiemSatTruyTo));
            listParameter.Add(new SqlParameter("@NguoiCapNhat", model.NguoiCapNhat));
            listParameter.Add(new SqlParameter("@NguoiTao", model.NguoiTao));
            listParameter.Add(new SqlParameter("@GhiChu", model.GhiChu));

            result = DBUtils.ExecuteSP<ResponseResult>("SP_NhanDon_HoSoVuAn_HinhSu_Them", listParameter, AppName.BizSecurity);
            return result;
        }

        public ResponseResult SuaHoSoVuAnHinhSu(HoSoVuAnHinhSuModel model)
        {
            ResponseResult result = null;

            List<SqlParameter> listParameter = new List<SqlParameter>();
            listParameter.Add(new SqlParameter("@HoSoVuAnID", model.HoSoVuAnID));
            listParameter.Add(new SqlParameter("@TenVuAn", model.TenVuAn));
            listParameter.Add(new SqlParameter("@SoBiCan", model.SoBiCan));
            listParameter.Add(new SqlParameter("@TongSoButLuc", model.TongSoButLuc));
            listParameter.Add(new SqlParameter("@KhongCoCacButLuc", model.KhongCoCacButLuc));
            listParameter.Add(new SqlParameter("@TruyToBang", model.TruyToBang));
            listParameter.Add(new SqlParameter("@SoTruyTo", model.SoTruyTo));
            listParameter.Add(new SqlParameter("@NgayTruyTo", model.NgayTruyTo));
            listParameter.Add(new SqlParameter("@VienKiemSatTruyTo", model.VienKiemSatTruyTo));
            listParameter.Add(new SqlParameter("@GhiChu", model.GhiChu));
            listParameter.Add(new SqlParameter("@NguoiCapNhat", model.NguoiCapNhat));

            result = DBUtils.ExecuteSP<ResponseResult>("SP_NhanDon_HoSoVuAn_HinhSu_Sua", listParameter, AppName.BizSecurity);

            return result;
        }

        #endregion

        #region ToiDanhTruyTo

        public IEnumerable<ToiDanhTruyToModel> GetDanhSachToiDanhTruyTo(int hoSoVuAnId)
        {
            List<ToiDanhTruyToModel> list = null;
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@HoSoVuAnID", hoSoVuAnId)
                };
                list = DBUtils.ExecuteSPList<ToiDanhTruyToModel>("SP_NhanDon_ToiDanhTruyTo_DanhSachToiDanhTruyTo", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }

        public ToiDanhTruyToModel ChiTietToiDanhTruyToTheoId(int toiDanhTruyToId)
        {
            ToiDanhTruyToModel dbModel = null;
            try
            {
                var parameters = new List<SqlParameter> { new SqlParameter("@ToiDanhTruyToID", toiDanhTruyToId) };
                dbModel = DBUtils.ExecuteSP<ToiDanhTruyToModel>("SP_NhanDon_ToiDanhTruyTo_ChiTietTheoID", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }

        public List<ToiDanhTruyTo_KhoanDiemModel> KhoanDiemTheoToiDanhTruyToId(int toiDanhTruyToId)
        {
            List<ToiDanhTruyTo_KhoanDiemModel> dbModel = new List<ToiDanhTruyTo_KhoanDiemModel>();
            try
            {
                var parameters = new List<SqlParameter> { new SqlParameter("@ToiDanhTruyToID", toiDanhTruyToId) };
                dbModel = DBUtils.ExecuteSPList<ToiDanhTruyTo_KhoanDiemModel>("SP_NhanDon_ToiDanhTruyTo_KhoanDiem_ChiTietTheoID", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }

        public ResponseResult ThemToiDanhTruyTo_KhoanDiem(ToiDanhTruyTo_KhoanDiemModel dbModel)
        {
            ResponseResult result = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@ToiDanhTruyToID", dbModel.ToiDanhTruyToID));
                listParameter.Add(new SqlParameter("@Khoan", dbModel.Khoan));
                listParameter.Add(new SqlParameter("@Diem", dbModel.Diem));
                listParameter.Add(new SqlParameter("@NguoiTao", dbModel.NguoiTao));

                result = DBUtils.ExecuteSP<ResponseResult>("SP_NhanDon_ToiDanhTruyTo_KhoanDiem_Them", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public ResponseResult ThemToiDanhTruyTo(ToiDanhTruyToModel dbModel)
        {
            ResponseResult result = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoSoVuAnID", dbModel.HoSoVuAnID));
                listParameter.Add(new SqlParameter("@ToiDanh", dbModel.ToiDanh));
                listParameter.Add(new SqlParameter("@Dieu", dbModel.Dieu));
                listParameter.Add(new SqlParameter("@BoLuatHinhSu", dbModel.BoLuatHinhSu));
                listParameter.Add(new SqlParameter("@NoiDungDieuLuat", dbModel.NoiDungDieuLuat));
                listParameter.Add(new SqlParameter("@NguoiTao", dbModel.NguoiTao));
                listParameter.Add(new SqlParameter("@GhiChu", dbModel.GhiChu));

                result = DBUtils.ExecuteSP<ResponseResult>("SP_NhanDon_ToiDanhTruyTo_Them", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public ResponseResult SuaToiDanhTruyTo(ToiDanhTruyToModel dbModel)
        {
            ResponseResult result = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@ToiDanhTruyToID", dbModel.ToiDanhTruyToID));
                listParameter.Add(new SqlParameter("@HoSoVuAnID", dbModel.HoSoVuAnID));
                listParameter.Add(new SqlParameter("@ToiDanh", dbModel.ToiDanh));
                listParameter.Add(new SqlParameter("@Dieu", dbModel.Dieu));
                listParameter.Add(new SqlParameter("@BoLuatHinhSu", dbModel.BoLuatHinhSu));
                listParameter.Add(new SqlParameter("@NoiDungDieuLuat", dbModel.NoiDungDieuLuat));
                listParameter.Add(new SqlParameter("@NguoiTao", dbModel.NguoiTao));
                listParameter.Add(new SqlParameter("@GhiChu", dbModel.GhiChu));

                result = DBUtils.ExecuteSP<ResponseResult>("SP_NhanDon_ToiDanhTruyTo_Sua", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public ResponseResult SuaToiDanhTruyTo_KhoanDiem(ToiDanhTruyTo_KhoanDiemModel dbModel)
        {
            ResponseResult result = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@KhoanDiemID", dbModel.KhoanDiemID));
                listParameter.Add(new SqlParameter("@Khoan", dbModel.Khoan));
                listParameter.Add(new SqlParameter("@Diem", dbModel.Diem));
                listParameter.Add(new SqlParameter("@NguoiTao", dbModel.NguoiTao));
                result = DBUtils.ExecuteSP<ResponseResult>("SP_NhanDon_ToiDanhTruyTo_KhoanDiem_Sua", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public ResponseResult XoaToiDanhTruyTo(int toiDanhTruyToId, string NguoiTao)
        {
            ResponseResult result = null;
            try
            {
                var parameters = new List<SqlParameter> {
                    new SqlParameter("@ToiDanhTruyToID", toiDanhTruyToId),
                    new SqlParameter("@NguoiTao", NguoiTao)
            };
                result = DBUtils.ExecuteSP<ResponseResult>("SP_NhanDon_ToiDanhTruyTo_Xoa", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public ResponseResult XoaToiDanhTruyTo_KhoanDiem(int khoanDiemID, string NguoiTao)
        {
            ResponseResult result = null;
            try
            {
                var parameters = new List<SqlParameter> {
                    new SqlParameter("@KhoanDiemID", khoanDiemID),
                    new SqlParameter("@NguoiTao", NguoiTao)
                };
                result = DBUtils.ExecuteSP<ResponseResult>("SP_NhanDon_ToiDanhTruyTo_KhoanDiem_Xoa", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        #endregion
    }
}