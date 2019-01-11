using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Biz.Lib.TACM.SauXetXu.Model.TamUngAnPhi;
using Biz.TACM.Models.Model.SauXetXu.KhangCao;
using Biz.TACM.Models.Model.SauXetXu.KhangCaoQuaHan;
using Biz.TACM.Models.Model.SauXetXu.KhangNghi;
using Biz.TACM.Models.Model.SauXetXu.LuuKho;
using Biz.TACM.Models.Model.SauXetXu.PhatHanhBanAn;
using Biz.TACM.Models.Model.SauXetXu.SuaChuaBanAn;
using Biz.TACM.Models.ViewModel.SauXetXu.TamUngAnPhi;
using Biz.TACM.Models.ViewModel.SauXetXu.ChuyenHoSo;
using Biz.TACM.Models.ViewModel.SauXetXu.Shared;
using Biz.Lib.TACM.SauXetXu.Model.LuuKho.DataRequest;

namespace Biz.TACM.IServices
{
    public interface ISauXetXuService
    {
        #region PhatHanhBanAn
        IEnumerable<NgayPhatHanhBanAnModel> DanhSachNgayPhatHanhBanAn(int hoSoVuAnId, int giaiDoan);
        ThongTinPhatHanhBanAnModel ThongTinPhatHanhBanAnTheoId(int phatHanhBanAnId);
        IEnumerable<DuongSuViewModel> DanhSachDuongSuNhanPhatHanhBanAn(int phatHanhBanAnId);
        IEnumerable<DuongSuViewModel> DanhSachDuongSuTheoHoSo(int hoSoVuAnId, int? phatHanhBanAnId);
        bool PhatHanhBanAn(int hoSoVuAnId, DateTime ngayPhatHanhBanAn, DateTime? hieuLuc, IEnumerable<int> duongSuNhanPhatHanhBanAnIds, IEnumerable<string> duongSuNgayPhatHanh);
        #endregion

        #region SuaChuaBanAn
        IEnumerable<NgaySuaChuaBanAnModel> DanhSachNgaySuaChuaBanAn(int hoSoVuAnId, int giaiDoan);
        ThongTinSuaChuaBanAnModel ThongTinSuaChuaBanAnTheoId(int suaChuaBanAnId);
        bool SuaChuaBanAn(int hoSoVuAnId, DateTime ngaySuaChua, string lyDo, string nguoiKy, string noiDung, string dinhKemFile);
        #endregion

        #region KhangNghi
        IEnumerable<NgayKhangNghiModel> DanhSachNgayKhangNghi(int hoSoVuAnId, int giaiDoan);
        ThongTinKhangNghiModel ThongTinKhangNghiTheoId(int khangNghiId);
        bool KhangNghi(int hoSoVuAnId, DateTime ngayToaAnNhanVanBan, string vienKiemSatKhangNghi, string vanBanKhangNghi, string hinhThuc, string noiDung, List<string> nguoiBiKhangNghi);
        #endregion

        #region Khang Cao / Khang Nghi Qua Han
        IEnumerable<NgayKhangCaoQuaHanModel> DanhSachNgayKhangCaoQuaHan(int hoSoVuAnId, int giaiDoan);
        ThongTinKhangCaoQuaHanModel ThongTinKhangCaoQuaHanTheoId(int khangCaoQuaHanId);
        bool KhangCaoQuaHan(int hoSoVuAnId, string khangCaoHayKhangNghi, string lyDo, string ketQua);
        SelectList KhangCaoHayKhangNghi(string selectedValue);

        #endregion

        #region LuuKho
        IEnumerable<NgayLuuKhoModel> DanhSachNgayLuuKho(int hoSoVuAnId, int giaiDoan);
        ThongTinLuuKhoModel ThongTinLuuKhoTheoId(int luuKhoId);
        bool LuuKho(LuuKhoDataRequest model);
        #endregion

        #region KhangCao
        IEnumerable<KhangCaoModel> DanhSachKhangCao(int hoSoVuAnId, int giaiDoan);
        ThongTinKhangCaoModel ThongTinKhangCaoTheoId(int khangCaoId);
        IEnumerable<DuongSuViewModel> DanhSachDuongSuTheoHoSo(int hoSoVuAnId);
        IEnumerable<DuongSuViewModel> DanhSachBiCao(int hoSoVuAnId);
        IEnumerable<DuongSuViewModel> DanhSachBiCaoSelected(int hoSoVuAnId, List<string> selected);
        bool KhangCao(int hoSoVuAnId, int nguoiKhangCao, DateTime ngayLamDon, DateTime ngayNopDon, string hinhThucNop, string tinhTrangKhangcao, string noiDungKhangCao, string taiLieuChungTuKemTheo, string nguoiKhieuNai, string lyDo, string ghiChu, List<string> nguoiBiKhangCao);
        bool SuaKhangCao(int khangCaoId, int nguoiKhangCao, DateTime ngayLamDon, DateTime ngayNopDon, string hinhThucNop, string tinhTrangKhangcao, string noiDungKhangCao, string taiLieuChungTuKemTheo, string nguoiKhieuNai, string lyDo, string ghiChu, List<string> nguoiBiKhangCao);
        bool XoaKhangCao(int khangCaoId);
        IEnumerable<DuongSuViewModel> DanhSachDuongSuKhacBiCaoTheoHoSo(int hoSoVuAnId);

        #endregion

        #region TamUngAnPhi
        SelectList DanhSachNgaySuaDoiTamUngAnPhi(int hoSoVuAnId, int giaiDoan, int congDoan, int selected);
        TamUngAnPhiViewModel ChiTietTamUngAnPhiTheoId(int hoSoVuAnId, int anPhiId, int giaiDoan, int congDoan);
        ResponseResult SuaMienDuNopTamUngAnPhi(TamUngAnPhiViewModel viewModel);
        ResponseResult SuaYeuCauDuNopTamUngAnPhi(TamUngAnPhiViewModel viewModel);
        ResponseResult SuaKetQuaDuNopTamUngAnPhi(TamUngAnPhiViewModel viewModel);
        #endregion

        #region ChuyenHoSo
        ChuyenHoSoViewModel ChiTietChuyenHoSoTheoId(int chuyenDonId);
        Lib.TACM.NhanDon.Model.ResponseResult SuaDoiChuyenHoSo(ChuyenHoSoViewModel viewModel);
        int KiemTraTinhTrangDinhKemFileQuyetDinh(int hoSoVuAnId, int giaiDoan);
        int KiemTraTinhTrangDinhKemFileBanAn(int hoSoVuAnId, int giaiDoan);

        #endregion

    }
}