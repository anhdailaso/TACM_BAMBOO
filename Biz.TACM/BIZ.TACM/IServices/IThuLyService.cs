using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Biz.Lib.SettingData.Model;
using Biz.Lib.TACM.ThuLy.Model;
using Biz.TACM.Models.ViewModel.ThuLy;

namespace Biz.TACM.IServices
{
    public interface IThuLyService
    {
        //ChiTietHoSoVuAn        
       
        SelectList GetDanhSachNgaySuaDoiHoSoVuAnThuLy(int hoSoVuAnId, int giaiDoan, int selected);
        HoSoVuAnThuLyViewModel ChiTietHoSoVuAnThuLyTheoId(int hoSoVuAnLogId);
        HoSoVuAnThuLyViewModel ChiTietHoSoVuAnThuLyTheoHoSoVuAnId(int hoSoVuAnId, int giaiDoan);
        ResponseResult SuaDoiHoSoVuAnThuLy(HoSoVuAnThuLyViewModel viewModel, int giaiDoan);

        //AnPhiLePhi
        SelectList GetDanhSachNgaySuaDoiAnPhi(int hoSoVuAnId, int giaiDoan, int congDoan, int selected);
        AnPhiViewModel ChiTietAnPhiTheoId(int hoSoVuAnId, int anPhiId, int giaiDoan, int congDoan);
        ResponseResult SuaYeuCauDuNopAnPhi(AnPhiViewModel viewModel);
        ResponseResult SuaKetQuaDuNopAnPhi(AnPhiViewModel viewModel);
        ResponseResult SuaMienDuNopAnPhi(AnPhiViewModel viewModel);

        //ThongTinThuLy
        SelectList GetDanhSachNgaySuaDoiThongTinThuLy(int hoSoVuAnId, int giaiDoan, int selected);
        ThongTinThuLyViewModel ChiTietThongTinThuLyTheoId(int thongTinThuLyId, int hoSoVuAnId);
        ResponseResult SuaDoiThongTinThuLy(ThongTinThuLyViewModel viewModel);
        int KiemTraTinhTrangNhapThongTinAnPhi(int hoSoVuAnId);
        int SoThuLyMax(int hoSoVuAnId, DateTime? ngayThuLy);
        int SoThuLyMaxChoADBPXLHC(int ToaAnID, string NhomAn, int giaiDoan); // chỉ Sơ thẩm
        //ResponseResult CheckSoThuLy(string SoThuLy, int HoSoVuAnID, int ToaAnID, string NhomAn, int giaiDoan);
        ResponseResult CheckSoThuLy(string SoThuLy, int HoSoVuAnID, DateTime NgayThuLy);
        ThongTinThuLyViewModel ChiTietThongTinThuLyCopySangPhucTham(int hoSoVuAnId);
        ThongTinThuLyViewModel ChiTietNoiDungKhangCaoTheoHoSoVuAnId(int hoSoVuAnId);

        //PhanCongThamPhan
        SelectList GetDanhSachNgaySuaDoiPhanCongThamPhan(int hoSoVuAnId, int giaiDoan, int selected);
        PhanCongThamPhanViewModel ChiTietPhanCongThamPhanTheoId(int thamPhanId);
        ResponseResult SuaDoiPhanCongThamPhan(PhanCongThamPhanViewModel viewModel);
        List<ThamPhanDuKhuyetModel> DanhSachThamPhanDuKhuyetTheoThamPhanId(int thamPhanId);
        List<HoiThamNhanDanDuKhuyetModel> DanhSachHoiThamNhanDanDuKhuyetTheoThamPhanId(int thamPhanId);
        List<ThuKyDuKhuyetModel> DanhSachThuKyDuKhuyetTheoThamPhanId(int thamPhanId);
        List<NhanVienModel> DanhSachThamPhanDuKhuyetSelected(string chucDanh, int toaAnId, List<ThamPhanDuKhuyetModel> selected);
        List<NhanVienModel> DanhSachHoiThamNhanDanDuKhuyetSelected(string chucDanh, int toaAnId, List<HoiThamNhanDanDuKhuyetModel> selected);
        List<NhanVienModel> DanhSachThuKyDuKhuyetSelected(string chucDanh, int toaAnId, List<ThuKyDuKhuyetModel> selected);

        //NhomAn ADBPXLHC
        ResponseResult SuaDoiThongTinThuLyADBPXLHC(ThongTinThuLyViewModel viewModel);

        //ghichuphancong
        SelectList GetDanhSachNgaySuaDoiGhiChuPhanCong(int hoSoVuAnId, int giaiDoan);
        GhiChuPhanCongModel ChiTietGhiChuPhanCongTheoId(int Id);
        ResponseResult SuaDoiGhiChuPhanCong(GhiChuPhanCongModel viewModel);
    }
}
