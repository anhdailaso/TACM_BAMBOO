using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Biz.Lib.Helpers;
using Biz.Lib.TACM.ThuLy.IDataAccess;
using Biz.Lib.TACM.ThuLy.Model;

namespace Biz.Lib.TACM.ThuLy.DataAccess
{
    public class ThuLyDataAccess : IThuLyDataAccess
    {
        #region HoSoVuAnThuLy       

        public IEnumerable<HoSoVuAnThuLyModel> GetDanhSachNgaySuaDoiHoSoVuAnThuLy(int hoSoVuAnId, int giaiDoan)
        {
            List<HoSoVuAnThuLyModel> listNgaySuaDoi = null;
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@HoSoVuAnID", hoSoVuAnId),
                    new SqlParameter("@GiaiDoan", giaiDoan)
                };
                listNgaySuaDoi = DBUtils.ExecuteSPList<HoSoVuAnThuLyModel>("SP_ThuLy_HoSoVuAn_DanhSachNgaySuaDoi", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listNgaySuaDoi;
        }

        public HoSoVuAnThuLyModel ChiTietHoSoVuAnThuLyTheoId(int hoSoVuAnLogId)
        {
            HoSoVuAnThuLyModel dbModel = null;
            try
            {
                var parameters = new List<SqlParameter> { new SqlParameter("@HoSoVuAnLogID", hoSoVuAnLogId) };
                dbModel = DBUtils.ExecuteSP<HoSoVuAnThuLyModel>("SP_ThuLy_HoSoVuAn_ChiTiet_Theo_HoSoVuAnLogID", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }

        public HoSoVuAnThuLyModel ChiTietHoSoVuAnThuLyTheoHoSoVuAnId(int hoSoVuAnId, int giaiDoan)
        {
            HoSoVuAnThuLyModel dbModel = null;
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@HoSoVuAnID", hoSoVuAnId),
                    new SqlParameter("@GiaiDoan", giaiDoan)
                };
                dbModel = DBUtils.ExecuteSP<HoSoVuAnThuLyModel>("SP_ThuLy_HoSoVuAn_ChiTiet_Theo_HoSoVuAnID", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }

        public ResponseResult SuaDoiHoSoVuAnThuLy(HoSoVuAnThuLyModel dbModel)
        {
            ResponseResult result = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoSoVuAnID", dbModel.HoSoVuAnID));
                listParameter.Add(new SqlParameter("@NgayLamDon", dbModel.NgayLamDon));
                listParameter.Add(new SqlParameter("@NgayNopDonTaiToaAn", dbModel.NgayNopDonTaiToaAn));
                listParameter.Add(new SqlParameter("@HinhThucGoiDon", dbModel.HinhThucGoiDon));
                listParameter.Add(new SqlParameter("@LoaiDon", dbModel.LoaiDon));
                listParameter.Add(new SqlParameter("@LoaiQuanHe", dbModel.LoaiQuanHe));
                listParameter.Add(new SqlParameter("@QuanHePhapLuat", dbModel.QuanHePhapLuat));
                listParameter.Add(new SqlParameter("@YeuToNuocNgoai", dbModel.YeuToNuocNgoai));
                listParameter.Add(new SqlParameter("@NguoiKyXacNhanDaNhanDon", dbModel.NguoiKyXacNhanDaNhanDon));
                listParameter.Add(new SqlParameter("@NguoiTao", dbModel.NguoiTao));

                result = DBUtils.ExecuteSP<ResponseResult>("SP_ThuLy_HoSoVuAn_CapNhatHoSo", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        #endregion

        #region ThongTinThuLy

        public IEnumerable<ThongTinThuLyModel> GetDanhSachNgaySuaDoiThongTinThuLy(int hoSoVuAnId, int giaiDoan)
        {
            List<ThongTinThuLyModel> listNgaySuaDoi = null;
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@HoSoVuAnID", hoSoVuAnId),
                    new SqlParameter("@GiaiDoan", giaiDoan)
                };
                listNgaySuaDoi = DBUtils.ExecuteSPList<ThongTinThuLyModel>("SP_ThuLy_ThongTinThuLy_DanhSachNgaySuaDoi", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listNgaySuaDoi;
        }

        public ThongTinThuLyModel ChiTietThongTinThuLyTheoId(int thongTinThuLyId, int hoSoVuAnId)
        {
            ThongTinThuLyModel dbModel = null;
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@ThongTinThuLyID", thongTinThuLyId) ,
                    new SqlParameter("@HoSoVuAnID", hoSoVuAnId) ,
                };
                dbModel = DBUtils.ExecuteSP<ThongTinThuLyModel>("SP_ThuLy_ThongTinThuLy_Get_ChiTietTheoId", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }

        public ResponseResult SuaDoiThongTinThuLy(ThongTinThuLyModel dbModel)
        {
            ResponseResult result = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoSoVuAnID", dbModel.HoSoVuAnID));
                listParameter.Add(new SqlParameter("@SoThuLy", dbModel.SoThuLy));
                listParameter.Add(new SqlParameter("@TruongHopThuLy", dbModel.TruongHopThuLy));
                listParameter.Add(new SqlParameter("@ThuLyTheoThuTuc", dbModel.ThuLyTheoThuTuc));
                listParameter.Add(new SqlParameter("@NgayThuLy", dbModel.NgayThuLy));
                listParameter.Add(new SqlParameter("@LoaiQuanHe", dbModel.LoaiQuanHe));
                listParameter.Add(new SqlParameter("@QuanHePhapLuat", dbModel.QuanHePhapLuat));
                listParameter.Add(new SqlParameter("@ThoiHanGiaiQuyet", dbModel.ThoiHanGiaiQuyet));
                listParameter.Add(new SqlParameter("@ThoiHanGiaiQuyetTuNgay", dbModel.ThoiHanGiaiQuyetTuNgay));
                listParameter.Add(new SqlParameter("@ThoiHanGiaiQuyetDenNgay", dbModel.ThoiHanGiaiQuyetDenNgay));
                listParameter.Add(new SqlParameter("@NoiDungYeuCau", dbModel.NoiDungYeuCau));
                listParameter.Add(new SqlParameter("@NoiDungKhangCao", dbModel.NoiDungKhangCao));
                listParameter.Add(new SqlParameter("@TaiLieuChungTuKemTheo", dbModel.TaiLieuChungTuKemTheo));
                listParameter.Add(new SqlParameter("@QuyetDinh", dbModel.QuyetDinh));
                listParameter.Add(new SqlParameter("@NguoiTao", dbModel.NguoiTao));
                listParameter.Add(new SqlParameter("@GhiChu", dbModel.GhiChu));

                result = DBUtils.ExecuteSP<ResponseResult>("SP_ThuLy_ThongTinThuLy_Them", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public int KiemTraTinhTrangNhapThongTinAnPhi(int hoSoVuAnId)
        {
            int trangThaiAnPhi;
            try
            {
                var parameters = new List<SqlParameter> { new SqlParameter("@HoSoVuAnID", hoSoVuAnId) };
                ResponseResult result = DBUtils.ExecuteSP<ResponseResult>("SP_ThuLy_ThongTinThuLy_KiemTraTrangThai_AnPhi", parameters, AppName.BizSecurity);
                trangThaiAnPhi = result.ResponseCode;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return trangThaiAnPhi;
        }

        public int SoThuLyMax(int hoSoVuAnId, DateTime? ngayThuLy)
        {
            int t = 0;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoSoVuAnID", hoSoVuAnId));
                listParameter.Add(new SqlParameter("@NgayThuLy", ngayThuLy));
                ResponseResult temp = DBUtils.ExecuteSP<ResponseResult>("SP_ThuLy_ThongTinThuLy_SoThuLy", listParameter, AppName.BizSecurity);
                t += temp.ResponseCode;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return t;
        }
        public int SoThuLyMaxChoADBPXLHC(int ToaAnID, string MaNhomAn, int giaiDoan)
        {
            int t = 0;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@ToaAnID", ToaAnID));
                listParameter.Add(new SqlParameter("@NhomAn", MaNhomAn));
                listParameter.Add(new SqlParameter("@GiaiDoan", giaiDoan));
                ResponseResult temp = DBUtils.ExecuteSP<ResponseResult>("SP_ThuLy_ThongTinThuLy_SoThuLyThuocBangHoSoVuAn", listParameter, AppName.BizSecurity);
                t += temp.ResponseCode;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return t;
        }
        //public ResponseResult CheckSoThuLy(string SoThuLy, int HoSoVuAnID, int ToaAnID, string MaNhomAn, int GiaiDoan, DateTime NgayThuLy)
        public ResponseResult CheckSoThuLy(string SoThuLy, int HoSoVuAnID, DateTime NgayThuLy)
        {
            ResponseResult result = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@SoThuLy", SoThuLy));
                //listParameter.Add(new SqlParameter("@ToaAnID", ToaAnID));
                //listParameter.Add(new SqlParameter("@NhomAn", MaNhomAn));
                //listParameter.Add(new SqlParameter("@GiaiDoan", GiaiDoan));
                listParameter.Add(new SqlParameter("@HoSoVuAnID", HoSoVuAnID));
                listParameter.Add(new SqlParameter("@NgayThuLy", NgayThuLy));
                result = DBUtils.ExecuteSP<ResponseResult>("SP_ThuLy_ThongTinThuLy_KiemtraTrung_SoThuLy", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public ThongTinThuLyModel ChiTietThongTinThuLyCopySangPhucTham(int hoSoVuAnId)
        {
            ThongTinThuLyModel dbModel = null;
            try
            {
                var parameters = new List<SqlParameter> { new SqlParameter("@HoSoVuAnID", hoSoVuAnId) };
                dbModel = DBUtils.ExecuteSP<ThongTinThuLyModel>("SP_ThuLy_ThongTinThuLy_Get_DuLieuCopySangPhucTham", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }

        public ThongTinThuLyModel ChiTietNoiDungKhangCaoTheoHoSoVuAnId(int hoSoVuAnId)
        {
            ThongTinThuLyModel dbModel = null;
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@HoSoVuAnID", hoSoVuAnId) ,
                };
                dbModel = DBUtils.ExecuteSP<ThongTinThuLyModel>("SP_ThuLy_ThongTinThuLy_Get_NoiDungKhangCao_PhucTham", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }

        #endregion

        #region PhanCongThamPhan 

        public IEnumerable<PhanCongThamPhanModel> GetDanhSachNgaySuaDoiPhanCongThamPhan(int hoSoVuAnId, int giaiDoan)
        {
            List<PhanCongThamPhanModel> listNgaySuaDoi = null;
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@HoSoVuAnID", hoSoVuAnId),
                    new SqlParameter("@GiaiDoan", giaiDoan)
                };
                listNgaySuaDoi = DBUtils.ExecuteSPList<PhanCongThamPhanModel>("SP_ThuLy_PhanCongThamPhan_DanhSachNgaySuaDoi", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listNgaySuaDoi;
        }

        public IEnumerable<ThamPhanDuKhuyetModel> DanhSachThamPhanDuKhuyetTheoThamPhanId(int thamPhanId)
        {
            List<ThamPhanDuKhuyetModel> listThamPhanDuKhuyet = null;
            try
            {
                var parameters = new List<SqlParameter> { new SqlParameter("@ThamPhanID", thamPhanId) };
                listThamPhanDuKhuyet = DBUtils.ExecuteSPList<ThamPhanDuKhuyetModel>("SP_ThuLy_PhanCongThamPhan_DanhSachThamPhanDuKhuyet", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listThamPhanDuKhuyet;
        }

        public IEnumerable<HoiThamNhanDanDuKhuyetModel> DanhSachHoiThamNhanDanDuKhuyetTheoThamPhanId(int thamPhanId)
        {
            List<HoiThamNhanDanDuKhuyetModel> listHoiThamNhanDanDuKhuyet = null;
            try
            {
                var parameters = new List<SqlParameter> { new SqlParameter("@ThamPhanID", thamPhanId) };
                listHoiThamNhanDanDuKhuyet = DBUtils.ExecuteSPList<HoiThamNhanDanDuKhuyetModel>("SP_ThuLy_PhanCongThamPhan_DanhSachHoiThamNhanDanDuKhuyet", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listHoiThamNhanDanDuKhuyet;
        }

        public IEnumerable<ThuKyDuKhuyetModel> DanhSachThuKyDuKhuyetTheoThamPhanId(int thamPhanId)
        {
            List<ThuKyDuKhuyetModel> listThuKyDuKhuyet = null;
            try
            {
                var parameters = new List<SqlParameter> { new SqlParameter("@ThamPhanID", thamPhanId) };
                listThuKyDuKhuyet = DBUtils.ExecuteSPList<ThuKyDuKhuyetModel>("SP_ThuLy_PhanCongThamPhan_DanhSachThuKyDuKhuyet", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listThuKyDuKhuyet;
        }

        public PhanCongThamPhanModel ChiTietPhanCongThamPhanTheoId(int thamPhanId)
        {
            PhanCongThamPhanModel dbModel = null;
            try
            {
                var parameters = new List<SqlParameter> { new SqlParameter("@ThamPhanID", thamPhanId) };
                dbModel = DBUtils.ExecuteSP<PhanCongThamPhanModel>("SP_ThuLy_PhanCongThamPhan_Get_ChiTietTheoId", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }

        public ResponseResult SuaDoiPhanCongThamPhan(PhanCongThamPhanModel dbModel)
        {
            ResponseResult resutl = null;
            try
            {
                string thamPhanDuKhuyet = "";
                if (dbModel.ThamPhanDuKhuyet != null)
                    thamPhanDuKhuyet = String.Join(",", dbModel.ThamPhanDuKhuyet);

                string hoiThamNhanDanDuKhuyet = "";
                if (dbModel.HoiThamNhanDanDuKhuyet != null)
                    hoiThamNhanDanDuKhuyet = String.Join(",", dbModel.HoiThamNhanDanDuKhuyet);

                string thuKyDuKhuyet = "";
                if (dbModel.ThuKyDuKhuyet != null)
                    thuKyDuKhuyet = String.Join(",", dbModel.ThuKyDuKhuyet);

                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoSoVuAnID", dbModel.HoSoVuAnID));
                listParameter.Add(new SqlParameter("@ThamPhan", dbModel.ThamPhan));
                listParameter.Add(new SqlParameter("@ThamPhan1", dbModel.ThamPhan1));
                listParameter.Add(new SqlParameter("@ThamPhan2", dbModel.ThamPhan2));
                listParameter.Add(new SqlParameter("@ThamPhanKhac", dbModel.ThamPhanKhac));
                listParameter.Add(new SqlParameter("@NgayPhanCong", dbModel.NgayPhanCong));
                listParameter.Add(new SqlParameter("@TenNguoiPhanCong", dbModel.TenNguoiPhanCong));
                listParameter.Add(new SqlParameter("@HoiThamNhanDan", dbModel.HoiThamNhanDan));
                listParameter.Add(new SqlParameter("@HoiThamNhanDan2", dbModel.HoiThamNhanDan2));
                listParameter.Add(new SqlParameter("@HoiThamNhanDan3", dbModel.HoiThamNhanDan3));
                listParameter.Add(new SqlParameter("@ThuKy", dbModel.ThuKy));
                listParameter.Add(new SqlParameter("@ThamPhanDuKhuyet", thamPhanDuKhuyet));
                listParameter.Add(new SqlParameter("@HoiThamNhanDanDuKhuyet", hoiThamNhanDanDuKhuyet));
                listParameter.Add(new SqlParameter("@ThuKyDuKhuyet", thuKyDuKhuyet));
                listParameter.Add(new SqlParameter("@HoiDong", dbModel.HoiDong));
                listParameter.Add(new SqlParameter("@NguoiTao", dbModel.NguoiTao));
                listParameter.Add(new SqlParameter("@GhiChu", dbModel.GhiChu));

                resutl = DBUtils.ExecuteSP<ResponseResult>("SP_ThuLy_PhanCongThamPhan_Them", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return resutl;
        }

        #endregion

        #region AnPhiLePhi

        public IEnumerable<AnPhiModel> GetDanhSachNgaySuaDoiAnPhi(int hoSoVuAnId, int giaiDoan, int congDoan)
        {
            List<AnPhiModel> listNgaySuaDoi = null;
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@HoSoVuAnID", hoSoVuAnId),
                    new SqlParameter("@GiaiDoan", giaiDoan),
                    new SqlParameter("@CongDoanHoSo", congDoan)
                };
                listNgaySuaDoi = DBUtils.ExecuteSPList<AnPhiModel>("SP_ThuLy_AnPhiLePhi_Get_DanhSachNgaySuaDoi", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listNgaySuaDoi;
        }

        public AnPhiModel ChiTietAnPhiTheoId(int hoSoVuAnId, int anPhiId, int giaiDoan, int congDoan)
        {
            AnPhiModel dbModel = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoSoVuAnID", hoSoVuAnId));
                listParameter.Add(new SqlParameter("@AnPhiID", anPhiId));
                listParameter.Add(new SqlParameter("@GiaiDoan", giaiDoan));
                listParameter.Add(new SqlParameter("@CongDoanHoSo", congDoan));
                dbModel = DBUtils.ExecuteSP<AnPhiModel>("SP_ThuLy_AnPhiLePhi_Get_ChiTietTheoId", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }

        public ResponseResult SuaYeuCauDuNopAnPhi(YeuCauDuNopAnPhiModel dbModel)
        {
            ResponseResult resutl = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoSoVuAnID", dbModel.HoSoVuAnID));
                listParameter.Add(new SqlParameter("@NopAnPhi", dbModel.NopAnPhi));
                listParameter.Add(new SqlParameter("@GiaTriTranhChap", dbModel.GiaTriTranhChap));
                listParameter.Add(new SqlParameter("@TongAnPhi", dbModel.TongAnPhi));
                listParameter.Add(new SqlParameter("@PhanTramDuNop", dbModel.PhanTramDuNop));
                listParameter.Add(new SqlParameter("@GiaTriDuNop", dbModel.GiaTriDuNop));
                listParameter.Add(new SqlParameter("@HanNopAnPhi", dbModel.HanNopAnPhi));
                listParameter.Add(new SqlParameter("@CoQuanThiHanhAnThu", dbModel.CoQuanThiHanhAnThu));
                listParameter.Add(new SqlParameter("@DiaChiCoQuanThiHanhAnThu", dbModel.DiaChiCoQuanThiHanhAnThu));
                listParameter.Add(new SqlParameter("@NgayRaThongBao", dbModel.NgayRaThongBao));
                listParameter.Add(new SqlParameter("@NgayGiaoThongBao", dbModel.NgayGiaoThongBao));
                listParameter.Add(new SqlParameter("@NguoiTao", dbModel.NguoiTao));
                listParameter.Add(new SqlParameter("@GhiChu", dbModel.GhiChu));
                listParameter.Add(new SqlParameter("@NguoiDuNop", dbModel.NguoiDuNop));
                resutl = DBUtils.ExecuteSP<ResponseResult>("SP_ThuLy_AnPhiLePhi_YeuCauDuNop_PhaiDuNop_Them", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return resutl;
        }

        public ResponseResult SuaMienDuNopAnPhi(MienDuNopModel dbModel)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@HoSoVuAnID", dbModel.HoSoVuAnID),
                new SqlParameter("@NopAnPhi", dbModel.NopAnPhi),
                new SqlParameter("@NguoiTao", dbModel.NguoiTao),
                new SqlParameter("@GhiChu", dbModel.GhiChu)
            };

            return DBUtils.ExecuteSP<ResponseResult>("SP_ThuLy_AnPhiLePhi_YeuCauDuNop_MienDuNop_Them", parameters, AppName.BizSecurity);
        }

        public ResponseResult SuaKetQuaDuNopAnPhi(AnPhiModel dbModel)
        {
            ResponseResult resutl = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoSoVuAnID", dbModel.HoSoVuAnID));
                listParameter.Add(new SqlParameter("@NopAnPhi", dbModel.NopAnPhi));
                listParameter.Add(new SqlParameter("@GiaTriTranhChap", dbModel.GiaTriTranhChap));
                listParameter.Add(new SqlParameter("@TongAnPhi", dbModel.TongAnPhi));
                listParameter.Add(new SqlParameter("@PhanTramDuNop", dbModel.PhanTramDuNop));
                listParameter.Add(new SqlParameter("@GiaTriDuNop", dbModel.GiaTriDuNop));
                listParameter.Add(new SqlParameter("@HanNopAnPhi", dbModel.HanNopAnPhi));
                listParameter.Add(new SqlParameter("@CoQuanThiHanhAnThu", dbModel.CoQuanThiHanhAnThu));
                listParameter.Add(new SqlParameter("@DiaChiCoQuanThiHanhAnThu", dbModel.DiaChiCoQuanThiHanhAnThu));
                listParameter.Add(new SqlParameter("@NgayRaThongBao", dbModel.NgayRaThongBao));
                listParameter.Add(new SqlParameter("@NgayGiaoThongBao", dbModel.NgayGiaoThongBao));
                listParameter.Add(new SqlParameter("@NgayNopAnPhi", dbModel.NgayNopAnPhi));
                listParameter.Add(new SqlParameter("@SoBienLai", dbModel.SoBienLai));
                listParameter.Add(new SqlParameter("@NgayNopBienLaiChoToaAn", dbModel.NgayNopBienLaiChoToaAn));
                listParameter.Add(new SqlParameter("@NguoiNhanBienLai", dbModel.NguoiNhanBienLai));
                listParameter.Add(new SqlParameter("@NguoiTao", dbModel.NguoiTao));
                listParameter.Add(new SqlParameter("@GhiChu", dbModel.GhiChu));

                resutl = DBUtils.ExecuteSP<ResponseResult>("SP_ThuLy_AnPhiLePhi_KetQuaDuNop_Them", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return resutl;
        }

        #endregion

        #region ghichuphancong

        public IEnumerable<GhiChuPhanCongModel> GetDanhSachNgaySuaDoiGhiChuPhanCong(int hoSoVuAnId, int giaiDoan)
        {
            List<GhiChuPhanCongModel> listNgaySuaDoi = null;
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@HoSoVuAnID", hoSoVuAnId),
                    new SqlParameter("@GiaiDoan", giaiDoan)
                };
                listNgaySuaDoi = DBUtils.ExecuteSPList<GhiChuPhanCongModel>("SP_ThuLy_GhiChuPhanCong_Get_Danh_Sach_Ngay_Sua_Doi", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listNgaySuaDoi;
        }

        public GhiChuPhanCongModel ChiTietGhiChuPhanCongId(int ghichuid)
        {
            GhiChuPhanCongModel dbModel = null;
            try
            {
                var parameters = new List<SqlParameter> { new SqlParameter("@GhiChuPhanCongID", ghichuid) };
                dbModel = DBUtils.ExecuteSP<GhiChuPhanCongModel>("SP_ThuLy_GhiChuPhanCong_Get_ChiTietTheoId", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }

        public ResponseResult ThemGhiChuPhanCong(GhiChuPhanCongModel dbModel)
        {
            ResponseResult result = null;
            try
            {
                var parameters = new List<SqlParameter> {
                    new SqlParameter("@HoSoVuAnID", dbModel.HoSoVuAnID),
                    new SqlParameter("@NoiDung", dbModel.NoiDungGhiChu),
                    new SqlParameter("@NguoiTao", dbModel.NguoiTao),
                    new SqlParameter("@GiaiDoan", dbModel.GiaiDoan)
                };
                result = DBUtils.ExecuteSP<ResponseResult>("SP_ThuLy_GhiChuPhanCong_Them", parameters, AppName.BizSecurity);
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
