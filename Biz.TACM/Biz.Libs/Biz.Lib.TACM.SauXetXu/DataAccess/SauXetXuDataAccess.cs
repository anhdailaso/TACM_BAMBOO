using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Biz.Lib.Helpers;
using Biz.Lib.TACM.SauXetXu.IDataAccess;
using Biz.Lib.TACM.SauXetXu.Model.KhangCao;
using Biz.Lib.TACM.SauXetXu.Model.KhangCao.DataRequest;
using Biz.Lib.TACM.SauXetXu.Model.KhangCaoQuaHan;
using Biz.Lib.TACM.SauXetXu.Model.KhangCaoQuaHan.DataRequest;
using Biz.Lib.TACM.SauXetXu.Model.KhangNghi;
using Biz.Lib.TACM.SauXetXu.Model.KhangNghi.DataRequest;
using Biz.Lib.TACM.SauXetXu.Model.LuuKho;
using Biz.Lib.TACM.SauXetXu.Model.LuuKho.DataRequest;
using Biz.Lib.TACM.SauXetXu.Model.Shared;
using Biz.Lib.TACM.SauXetXu.Model.Shared.DataRequest;
using Biz.Lib.TACM.SauXetXu.Model.SuaChuaBanAn;
using Biz.Lib.TACM.SauXetXu.Model.SuaChuaBanAn.DataRequest;
using Biz.Lib.TACM.SauXetXu.Model.TamUngAnPhi;


namespace Biz.Lib.TACM.SauXetXu.DataAccess
{
    public class SauXetXuDataAccess : ISauXetXuDataAccess
    {
        #region PhatHanhBanAn

        public IEnumerable<NgayPhatHanhBanAnModel> DanhSachNgayPhatHanhBanAn(int hoSoVuAnId, int giaiDoan)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@HoSoVuAnID", hoSoVuAnId),
                new SqlParameter("@GiaiDoan", giaiDoan)
            };
            return DBUtils.ExecuteSPList<NgayPhatHanhBanAnModel>("SP_SauXetXu_PhatHanhBanAn_DanhSachNgayPhatHanhBanAn", parameters, AppName.BizSecurity);
        }

        public ThongTinPhatHanhBanAnModel ThongTinPhatHanhBanAnTheoId(int phatHanhBanAnId)
        {
            var parameters = new List<SqlParameter> { new SqlParameter("@PhatHanhBanAnId", phatHanhBanAnId) };
            return DBUtils.ExecuteSP<ThongTinPhatHanhBanAnModel>("SP_SauXetXu_PhatHanhBanAn_ThongTinPhatHanhBanAnTheoId", parameters, AppName.BizSecurity);
        }

        public IEnumerable<DuongSuModel> DanhSachDuongSuNhanPhatHanhBanAn(int phatHanhBanAnId)
        {
            var parameters = new List<SqlParameter> { new SqlParameter("@PhatHanhBanAnId", phatHanhBanAnId) };
            return DBUtils.ExecuteSPList<DuongSuModel>("SP_SauXetXu_PhatHanhBanAn_DanhSachDuongSuNhanPhatHanhBanAnTheoId", parameters, AppName.BizSecurity);
        }

        public IEnumerable<DuongSuModel> DanhSachDuongSuTheoHoSo(int hoSoVuAnId)
        {
            var parameters = new List<SqlParameter> { new SqlParameter("@HoSoVuAnId", hoSoVuAnId) };
            return DBUtils.ExecuteSPList<DuongSuModel>("SP_SauXetXu_DanhSachDuongSuTheoHoSoVuAn", parameters, AppName.BizSecurity);
        }

        public bool PhatHanhBanAn(PhatHanhBanAnDataRequest request)
        {
            var duongSuNhanPhatHanhBanAnIds = string.Join(",", request.DuongSuNhanPhatHanhBanAnIds);
            var duongSuNgayPhatHanh = string.Join(",", request.DuongSuNgayPhatHanh);
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@NgayPhatHanh", request.NgayPhatHanh),
                new SqlParameter("@HieuLuc", request.HieuLuc),
                new SqlParameter("@HoSoVuAnId", request.HoSoVuAnId),
                new SqlParameter("@NguoiTao", request.NguoiTao),
                new SqlParameter("@NgayTao", request.NgayTao),
                new SqlParameter("@GhiChu", request.GhiChu),
                new SqlParameter("@DuongSuNhanPhatHanhBanAnIds", duongSuNhanPhatHanhBanAnIds),
                new SqlParameter("@NgayDuongSuNhanPhatHanhBanAn", duongSuNgayPhatHanh),
                new SqlParameter {Direction = ParameterDirection.ReturnValue}
            };

            var returnValue = DBUtils.ExecNonQuerySP("SP_SauXetXu_PhatHanhBanAn_TaoPhatHanhBanAn", parameters, AppName.BizSecurity);
            return Protect.ToInt16(returnValue, 0) == ResponseCode.Success.GetHashCode();
        }
        #endregion

        #region SuaChuaBanAn
        public IEnumerable<NgaySuaChuaBanAnModel> DanhSachNgaySuaChuaBanAn(int hoSoVuAnId, int giaiDoan)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@HoSoVuAnID", hoSoVuAnId),
                new SqlParameter("@GiaiDoan", giaiDoan)
            };
            return DBUtils.ExecuteSPList<NgaySuaChuaBanAnModel>("SP_SauXetXu_SuaChuaBanAn_DanhSachNgaySuaChuaBanAn", parameters, AppName.BizSecurity);
        }

        public ThongTinSuaChuaBanAnModel ThongTinSuaChuaBanAnTheoNgay(DateTime ngaySuaChua)
        {
            var parameters = new List<SqlParameter> { new SqlParameter("@NgaySuaChuaBanAn", ngaySuaChua) };
            return DBUtils.ExecuteSP<ThongTinSuaChuaBanAnModel>("SP_SauXetXu_SuaChuaBanAn_ThongTinSuaChuaBanAnTheoNgay", parameters, AppName.BizSecurity);
        }

        public ThongTinSuaChuaBanAnModel ThongTinSuaChuaBanAnTheoId(int suaChuaBanAnId)
        {
            var parameters = new List<SqlParameter> { new SqlParameter("@SuaChuaBanAnId", suaChuaBanAnId) };
            return DBUtils.ExecuteSP<ThongTinSuaChuaBanAnModel>("SP_SauXetXu_SuaChuaBanAn_ThongTinSuaChuaBanAnTheoId", parameters, AppName.BizSecurity);
        }

        public bool SuaChuaBanAn(SuaChuaBanAnDataRequest request)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@NgaySuaChuaBanAn", request.NgaySuaChua),
                new SqlParameter("@HoSoVuAnId", request.HoSoVuAnId),
                new SqlParameter("@LyDo", request.LyDo),
                new SqlParameter("@NguoiKy", request.NguoiKy),
                new SqlParameter("@NguoiTao", request.NguoiTao),
                new SqlParameter("@NgayTao", request.NgayTao),
                new SqlParameter("@NoiDungSuaChua", request.NoiDungSuaChua),
                new SqlParameter("@DinhKemFile", request.DinhKemFile),
                new SqlParameter("@GhiChu", request.GhiChu),
                new SqlParameter {Direction = ParameterDirection.ReturnValue}
            };

            var returnValue = DBUtils.ExecNonQuerySP("SP_SauXetXu_SuaChuaBanAn_TaoSuaChuaBanAn", parameters, AppName.BizSecurity);
            return Protect.ToInt16(returnValue, 0) == ResponseCode.Success.GetHashCode();
        }
        #endregion

        #region KhangNghi
        public IEnumerable<NgayKhangNghiModel> DanhSachNgayKhangNghi(int hoSoVuAnId, int giaiDoan)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@HoSoVuAnID", hoSoVuAnId),
                new SqlParameter("@GiaiDoan", giaiDoan)
            };
            return DBUtils.ExecuteSPList<NgayKhangNghiModel>("SP_SauXetXu_KhangNghi_DanhSachNgayKhangNghi", parameters, AppName.BizSecurity);
        }

        public ThongTinKhangNghiModel ThongTinKhangNghiTheoId(int khangNghiId)
        {
            var parameters = new List<SqlParameter> { new SqlParameter("@KhangNghiId", khangNghiId) };
            return DBUtils.ExecuteSP<ThongTinKhangNghiModel>("SP_SauXetXu_KhangNghi_ThongTinKhangNghiBanAnTheoId", parameters, AppName.BizSecurity);
        }

        public bool KhangNghi(KhangNghiDataRequest request)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@NgayToaAnNhanVanBan", request.NgayToaAnNhanVanBan),
                new SqlParameter("@HoSoVuAnId", request.HoSoVuAnId),
                new SqlParameter("@VienKiemSatKhangNghi", request.VienKiemSatKhangNghi),
                new SqlParameter("@NguoiBiKhangNghi", request.NguoiBiKhangNghi),
                new SqlParameter("@VanBanKhangNghi", request.VanBanKhangNghi),
                new SqlParameter("@HinhThuc", request.HinhThuc),
                new SqlParameter("@NguoiTao", request.NguoiTao),
                new SqlParameter("@NgayTao", request.NgayTao),
                new SqlParameter("@NoiDungKhangNghi", request.NoiDungKhangNghi),
                new SqlParameter("@GhiChu", request.GhiChu),
                new SqlParameter {Direction = ParameterDirection.ReturnValue}
            };

            var returnValue = DBUtils.ExecNonQuerySP("SP_SauXetXu_KhangNghi_TaoKhangNghi", parameters, AppName.BizSecurity);
            return Protect.ToInt16(returnValue, 0) == ResponseCode.Success.GetHashCode();
        }
        #endregion

        #region KhangCaoQuaHan
        public IEnumerable<NgayKhangCaoQuaHanModel> DanhSachNgayKhangCaoQuanHan(int hoSoVuAnId, int giaiDoan)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@HoSoVuAnID", hoSoVuAnId),
                new SqlParameter("@GiaiDoan", giaiDoan)
            };
            return DBUtils.ExecuteSPList<NgayKhangCaoQuaHanModel>("SP_SauXetXu_KhangCaoQuaHan_DanhSachNgayKhangCaoQuaHan", parameters, AppName.BizSecurity);
        }

        public ThongTinKhangCaoQuaHanModel ThongTinKhangCaoQuaHanTheoId(int khangCaoQuaHanId)
        {
            var parameters = new List<SqlParameter> { new SqlParameter("@KhangCaoQuaHanId", khangCaoQuaHanId) };
            return DBUtils.ExecuteSP<ThongTinKhangCaoQuaHanModel>("SP_SauXetXu_KhangCaoQuaHan_ThongTinKhangCaoQuaHanTheoId", parameters, AppName.BizSecurity);
        }

        public bool KhangCaoQuaHan(KhangCaoQuaHanDataRequest request)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@HoSoVuAnId", request.HoSoVuAnId),
                new SqlParameter("@KhangCaoHayKhangNghi", request.KhangCaoHayKhangNghi),
                new SqlParameter("@LyDo", request.LyDo),
                new SqlParameter("@KetQua", request.KetQua),
                new SqlParameter("@NguoiTao", request.NguoiTao),
                new SqlParameter("@NgayTao", request.NgayTao),
                new SqlParameter("@GhiChu", request.GhiChu),
                new SqlParameter {Direction = ParameterDirection.ReturnValue}
            };

            var returnValue = DBUtils.ExecNonQuerySP("SP_SauXetXu_KhangCaoQuaHan_TaoKhangCaoQuaHan", parameters, AppName.BizSecurity);
            return Protect.ToInt16(returnValue, 0) == ResponseCode.Success.GetHashCode();
        }
        #endregion

        #region LuuKho
        public IEnumerable<NgayLuuKhoModel> DanhSachNgayLuuKho(int hoSoVuAnId, int giaiDoan)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@HoSoVuAnID", hoSoVuAnId),
                new SqlParameter("@GiaiDoan", giaiDoan)
            };
            return DBUtils.ExecuteSPList<NgayLuuKhoModel>("SP_SauXetXu_LuuKho_DanhSachNgayLuuKho", parameters, AppName.BizSecurity);
        }

        public ThongTinLuuKhoModel ThongTinLuuKhoTheoId(int luuKhoId)
        {
            var parameters = new List<SqlParameter> { new SqlParameter("@LuuKhoId", luuKhoId) };
            return DBUtils.ExecuteSP<ThongTinLuuKhoModel>("SP_SauXetXu_LuuKho_ThongTinLuuKhoTheoId", parameters, AppName.BizSecurity);
        }

        public bool LuuKho(LuuKhoDataRequest request)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@HoSoVuAnId", request.HoSoVuAnId),
                new SqlParameter("@NgayLuu", request.NgayLuu),
                new SqlParameter("@NguoiGiao", request.NguoiGiao),
                new SqlParameter("@NguoiNhanLuu", request.NguoiNhanLuu),
                new SqlParameter("@GhiChuTinhTrangHoSo", request.GhiChuTinhTrangHoSo),
                new SqlParameter("@NguoiTao", request.NguoiTao),
                new SqlParameter("@NgayTao", request.NgayTao),
                new SqlParameter("@GhiChu", request.GhiChu),
                new SqlParameter {Direction = ParameterDirection.ReturnValue}
            };

            var returnValue = DBUtils.ExecNonQuerySP("SP_SauXetXu_LuuKho_TaoLuuKho", parameters, AppName.BizSecurity);
            return Protect.ToInt16(returnValue, 0) == ResponseCode.Success.GetHashCode();
        }
        #endregion

        #region KhangCao
        public IEnumerable<KhangCaoModel> DanhSachKhangCao(int hoSoVuAnId, int giaiDoan)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@HoSoVuAnID", hoSoVuAnId),
                new SqlParameter("@GiaiDoan", giaiDoan)
            };
            return DBUtils.ExecuteSPList<KhangCaoModel>("SP_SauXetXu_KhangCao_DanhSachKhangCao", parameters, AppName.BizSecurity);           
        }

        public ThongTinKhangCaoModel ThongTinKhangCaoTheoId(int khangCaoId)
        {
            var parameters = new List<SqlParameter> { new SqlParameter("@KhangCaoId", khangCaoId) };
            return DBUtils.ExecuteSP<ThongTinKhangCaoModel>("SP_SauXetXu_KhangCao_ThongTinKhangCaoTheoId", parameters, AppName.BizSecurity);
        }

        public bool KhangCao(KhangCaoDataRequest request)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@HoSoVuAnId", request.HoSoVuAnId),
                new SqlParameter("@DuongSuId", request.NguoiKhangCao),
                new SqlParameter("@NgayLamDon", request.NgayLamDon),
                new SqlParameter("@NgayNopDon", request.NgayNopDon),
                new SqlParameter("@HinhThucNopDon", request.HinhThucNopDon),
                new SqlParameter("@NguoiBiKhangCao", request.NguoiBiKhangCao),
                new SqlParameter("@TinhTrangKhangCao", request.TinhTrangKhangCao),
                new SqlParameter("@NoiDungKhangCao", request.NoiDungKhangCao),
                new SqlParameter("@TaiLieuChungTuKemTheo", request.TaiLieuChungTuKemTheo),
                new SqlParameter("@NguoiKhieuNai", request.NguoiKhieuNai), // án ADBPXLHC
                new SqlParameter("@NguoiNhanKhieuNai", request.NguoiNhanKhieuNai), // án ADBPXLHC
                new SqlParameter("@LyDo", request.LyDo), // án ADBPXLHC
                new SqlParameter("@NguoiTao", request.NguoiTao),
                new SqlParameter("@NgayTao", request.NgayTao),
                new SqlParameter("@GhiChu", request.GhiChu),
                new SqlParameter {Direction = ParameterDirection.ReturnValue}
            };

            var returnValue = DBUtils.ExecNonQuerySP("SP_SauXetXu_KhangCao_TaoKhangCao", parameters, AppName.BizSecurity);
            return Protect.ToInt16(returnValue, 0) == ResponseCode.Success.GetHashCode();
        }

        public bool SuaKhangCao(SuaKhangCaoDataRequest request)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@KhangCaoId", request.KhangCaoId),
                new SqlParameter("@DuongSuId", request.NguoiKhangCao),
                new SqlParameter("@NgayNopDon", request.NgayNopDon),
                new SqlParameter("@NgayLamDon", request.NgayLamDon),
                new SqlParameter("@HinhThucNopDon", request.HinhThucNopDon),
                new SqlParameter("@NguoiBiKhangCao", request.NguoiBiKhangCao),
                new SqlParameter("@TinhTrangKhangCao", request.TinhTrangKhangCao),
                new SqlParameter("@NoiDungKhangCao", request.NoiDungKhangCao),
                new SqlParameter("@TaiLieuChungTuKemTheo", request.TaiLieuChungTuKemTheo),
                new SqlParameter("@NguoiKhieuNai", request.NguoiKhieuNai), // án ADBPXLHC
                new SqlParameter("@NguoiNhanKhieuNai", request.NguoiNhanKhieuNai), // án ADBPXLHC
                new SqlParameter("@LyDo", request.LyDo), // án ADBPXLHC
                new SqlParameter("@NguoiTao", request.NguoiTao),
                new SqlParameter("@NgayTao", request.NgayTao),
                new SqlParameter("@GhiChu", request.GhiChu),
                new SqlParameter {Direction = ParameterDirection.ReturnValue}
            };

            var returnValue = DBUtils.ExecNonQuerySP("SP_SauXetXu_KhangCao_SuaKhangCao", parameters, AppName.BizSecurity);
            return Protect.ToInt16(returnValue, 0) == ResponseCode.Success.GetHashCode();
        }

        public bool XoaKhangCao(int khangCaoId)
        {
            var parameters = new List<SqlParameter> { new SqlParameter("@KhangCaoId", khangCaoId), new SqlParameter { Direction = ParameterDirection.ReturnValue } };
            var returnValue = DBUtils.ExecNonQuerySP("SP_SauXetXu_KhangCao_XoaKhangCao", parameters, AppName.BizSecurity);
            return Protect.ToInt16(returnValue, 0) == ResponseCode.Success.GetHashCode();
        }
        #endregion

        #region TamUngAnPhi

        public IEnumerable<TamUngAnPhiModel> DanhSachNgaySuaDoiTamUngAnPhi(int hoSoVuAnId, int giaiDoan, int congDoan)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@HoSoVuAnID", hoSoVuAnId),
                new SqlParameter("@GiaiDoan", giaiDoan),
                new SqlParameter("@CongDoanHoSo", congDoan)
            };
            return DBUtils.ExecuteSPList<TamUngAnPhiModel>("SP_SauXetXu_TamUngAnPhi_Get_DanhSachNgaySuaDoi", parameters, AppName.BizSecurity);
        }

        public TamUngAnPhiModel ChiTietTamUngAnPhiTheoId(int hoSoVuAnId, int anPhiId, int giaiDoan, int congDoan)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@HoSoVuAnID", hoSoVuAnId),
                new SqlParameter("@AnPhiID", anPhiId),
                new SqlParameter("@GiaiDoan", giaiDoan),
                new SqlParameter("@CongDoanHoSo", congDoan)
            };
            return DBUtils.ExecuteSP<TamUngAnPhiModel>("SP_SauXetXu_TamUngAnPhi_Get_ChiTietTheoId", parameters, AppName.BizSecurity);
        }

        public ResponseResult SuaYeuCauDuNopTamUngAnPhi(YeuCauDuNopAnPhiModel dbModel)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@HoSoVuAnID", dbModel.HoSoVuAnID),
                new SqlParameter("@NopAnPhi", dbModel.NopAnPhi),
                new SqlParameter("@TongAnPhi", dbModel.TongAnPhi),
                new SqlParameter("@PhanTramDuNop", dbModel.PhanTramDuNop),
                new SqlParameter("@GiaTriDuNop", dbModel.GiaTriDuNop),
                new SqlParameter("@HanNopAnPhi", dbModel.HanNopAnPhi),
                new SqlParameter("@CoQuanThiHanhAnThu", dbModel.CoQuanThiHanhAnThu),
                new SqlParameter("@DiaChiCoQuanThiHanhAnThu", dbModel.DiaChiCoQuanThiHanhAnThu),
                new SqlParameter("@NgayRaThongBao", dbModel.NgayRaThongBao),
                new SqlParameter("@NgayGiaoThongBao", dbModel.NgayGiaoThongBao),
                new SqlParameter("@NguoiTao", dbModel.NguoiTao),
                new SqlParameter("@GhiChu", dbModel.GhiChu)
            };

            return DBUtils.ExecuteSP<ResponseResult>("SP_SauXetXu_TamUngAnPhi_YeuCauDuNop_PhaiDuNop_Them", parameters, AppName.BizSecurity);
        }

        public ResponseResult SuaMienDuNopTamUngAnPhi(MienDuNopModel dbModel)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@HoSoVuAnID", dbModel.HoSoVuAnID),
                new SqlParameter("@NopAnPhi", dbModel.NopAnPhi),
                new SqlParameter("@NguoiTao", dbModel.NguoiTao),
                new SqlParameter("@GhiChu", dbModel.GhiChu)
            };

            return DBUtils.ExecuteSP<ResponseResult>("SP_SauXetXu_TamUngAnPhi_YeuCauDuNop_MienDuNop_Them", parameters, AppName.BizSecurity);
        }

        public ResponseResult SuaKetQuaDuNopTamUngAnPhi(TamUngAnPhiModel dbModel)
        {
            List<SqlParameter> listParameter = new List<SqlParameter>();
            listParameter.Add(new SqlParameter("@HoSoVuAnID", dbModel.HoSoVuAnID));
            listParameter.Add(new SqlParameter("@NopAnPhi", dbModel.NopAnPhi));
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

            return DBUtils.ExecuteSP<ResponseResult>("SP_SauXetXu_TamUngAnPhi_KetQuaDuNop_Them", listParameter, AppName.BizSecurity);
        }
        #endregion

        #region KiemTraDinhKemFile ChuyenHoSo

        public int KiemTraDinhKemFileQuyetDinh(int hoSoVuAnId, int giaiDoan)
        {
            int trangThaiDinhKemFile;
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@HoSoVuAnID", hoSoVuAnId),
                    new SqlParameter("@GiaiDoan", giaiDoan),
                };
                ResponseResult result = DBUtils.ExecuteSP<ResponseResult>("SP_SauXetXu_ChuyenHoSo_KiemTra_FileDinhKemQuyetDinh", parameters, AppName.BizSecurity);
                trangThaiDinhKemFile = result.ResponseCode;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return trangThaiDinhKemFile;
        }

        public int KiemTraDinhKemFileBanAn(int hoSoVuAnId, int giaiDoan)
        {
            int trangThaiDinhKemFile;
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@HoSoVuAnID", hoSoVuAnId),
                    new SqlParameter("@GiaiDoan", giaiDoan),
                };
                ResponseResult result = DBUtils.ExecuteSP<ResponseResult>("SP_SauXetXu_ChuyenHoSo_KiemTra_FileDinhKemBanAn", parameters, AppName.BizSecurity);
                trangThaiDinhKemFile = result.ResponseCode;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return trangThaiDinhKemFile;
        }
        #endregion
    }
}
