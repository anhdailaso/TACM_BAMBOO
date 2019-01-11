using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biz.Lib.TACM.NhanDon.Model;
using Biz.TACM.Models.ViewModel.NhanDon;
using System.Web.Mvc;
using Biz.Lib.SettingData.Model;
using HoSoVuAnModel = Biz.Lib.TACM.NhanDon.Model.HoSoVuAnModel;

namespace Biz.TACM.IServices
{
    public interface INhanDonService
    {
        //HoSoVuAn
        SelectList DanhSachNgayHoSoVuAn(int hoSoVuAnID, int giaiDoan, int selected);
        IEnumerable<DuongSuModel> NguyenDonVaBiDon(int hoSoVuAnID);
        SelectList SelectListDuongSu(int ToaAnID, int selected);
        SelectList SelectListNguoiKhieuNai(int hoSoVuAnId, int selected);
        HoSoVuAnModel ChiTietHoSoVuAn(int hoSoVuAnID);
        HoSoVuAnModel ChiTietHoSoVuAnTheoGiaiDoan(int hoSoVuAnID, int giaiDoan);
        HoSoVuAnModel ChiTietHoSoVuAnTheoLog(int id);
        HoSoVuAnModel ChiTietHoSoVuAnTheoId(int hoSoVuAnID);
        IEnumerable<HoSoVuAnModel> DanhSachHoSoVuAn(HoSoVuAnModel model, string maNV);
        ResponseResult ThemHoSoVuAn(HoSoVuAnModel model);
        ResponseResult SuaHoSoVuAn(HoSoVuAnModel model);
        SelectList SelectListCongDoanHoSo(int selectedValue, int giaiDoan);
        //SelectList SelectListNhanVien(string nhomChucNang, int selected);
        //ResponseResult ChuyenTrangThai(HoSoVuAnModel model);
        ResponseResult ChuyenCongDoanHoSo(HoSoVuAnModel model);
        Dictionary<int, int> CongDoanHoSo(HoSoVuAnModel model, string maNV);

        //Duong su
        IEnumerable<DuongSuViewModel> GetDanhSachDuongSuCaNhan(int hoSoVuAnId);
        IEnumerable<DuongSuViewModel> GetDanhSachDuongSuToChuc(int hoSoVuAnId);
        IEnumerable<DuongSuViewModel> GetDanhSachDuongSu(int hoSoVuAnId, string duongSuLa = null, string tuCachThamGiaToTung = null);
        DuongSuViewModel ChiTietDuongSuTheoId(int duongSuId);
        ResponseResult TaoDuongSu(DuongSuViewModel viewModel);
        ResponseResult SuaDuongSu(DuongSuViewModel viewModel);
        ResponseResult XoaDuongSu(int duongSuId);
        SelectList GetDanhSachDuongSuChoNguoiDaiDien(int hoSoVuAnId);
        SelectList GetDanhSachDuongSuChoNguoiBaoVe(int hoSoVuAnId);
        ResponseResult CapNhatBiCanBiCaoHinhSu(int hoSoVuAnId);
        IEnumerable<DuongSuViewModel.DacDiemNhanThan> DanhSachDacDiemSelected(SelectList selectListDacDiem, List<string> selected);

        //Noi dung don
        SelectList GetDanhSachNgaySuaDoiNoiDungDon(int hoSoVuAnId, int giaiDoan, int selected);
        NoiDungDonViewModel ChiTietNoiDungDonTheoId(int noiDungDonId);
        ResponseResult SuaDoiNoiDungDon(NoiDungDonViewModel viewModel);

        //Chuyen don
        SelectList SelectListVungChuyenDon(string selectedValue);
        SelectList GetDanhSachNgaySuaDoiChuyenDon(int hoSoVuAnId, int giaiDoan, int congDoan, int selected);
        ChuyenDonModel ChiTietChuyenDonTheoHoSoVuAnID(int hoSoVuAnID, int giaiDoan, int congDoan);
        ChuyenDonViewModel ChiTietChuyenDonTheoId(int chuyenDonId);
        ResponseResult SuaDoiChuyenDon(ChuyenDonViewModel viewModel);

        //Tham phan
        SelectList GetDanhSachNgaySuaDoiThamPhan(int hoSoVuAnId, int giaiDoan, int selected);
        ThamPhanModel ChiTietThamPhanTheoHoSoVuAnID(int hoSoVuAnID);
        ThamPhanViewModel ChiTietThamPhanTheoId(int thamPhanId);
        ResponseResult SuaDoiThamPhan(ThamPhanViewModel viewModel);

        //Sua doi bo sung don
        SelectList DanhSachNgaySuaDoiBoSungDon(int hoSoVuAnID, int giaiDoan, int selected);
        SuaDoiBoSungDonModel ChiTietSuaDoiBoSungDonTheoHoSoVuAnID(int hoSoVuAnID);
        SuaDoiBoSungDonModel ChiTietSuaDoiBoSungDonTheoSuaDoiBoSungDonID(int suaDoiBoSungDonID);
        ResponseResult ThemSuaDoiBoSungDon(SuaDoiBoSungDonModel viewModel);

        //Tra lai don
        SelectList SelectListLyDo(string selectedValue);
        SelectList DanhSachNgayTraLaiDon(int hoSoVuAnID, int giaiDoan, int CongDoan, int selected);
        TraLaiDonModel ChiTietTraLaiDonTheoHoSoVuAnID(int hoSoVuAnID, int CongDoan);
        TraLaiDonModel ChiTietTraLaiDonTheoTraLaiDonID(int traLaiDonID);
        ResponseResult ThemTraLaiDon(TraLaiDonModel model);

        //Khieu nai viec tra lai don
        SelectList SelectListNhom(string selectedValue);
        SelectList SelectListLanThu(int lanThu);
        SelectList DanhSachNgayKhieuNaiTraDon(int hoSoVuAnID, int giaiDoan, int selected);
        KhieuNaiTraDonModel ChiTietKhieuNaiTraDonTheoHoSoVuAnID(int hoSoVuAnID);
        KhieuNaiTraDonModel ChiTietKhieuNaiTraDonTheoKhieuNaiTraDonID(int suaDoiBoSungDonID);
        ResponseResult ThemKhieuNaiTraDon(KhieuNaiTraDonModel viewModel);

        //Khieu nai viec tra lai don
        SelectList SelectListKienNghiLanThu(int lanThu);
        SelectList DanhSachNgayKienNghiTraDon(int hoSoVuAnID, int giaiDoan, int selected);
        KienNghiTraDonModel ChiTietKienNghiTraDonTheoHoSoVuAnID(int hoSoVuAnID);
        KienNghiTraDonModel ChiTietKienNghiTraDonTheoKienNghiTraDonID(int kienNghiTraDonId);
        ResponseResult ThemKienNghiTraDon(KienNghiTraDonModel viewModel);

        //Searching on Header by Keyword
        IEnumerable<HoSoVuAnModel> DanhSachHoSoVuAnSearchByKeyword(string keyword);

        //Nhan Ho So Phuc Tham
        IEnumerable<NhanHoSoPhucThamViewModel> GetDanhSachHoSoChoPhucTham(string maNhomAn);
        int GetSoLuongHoSoChoPhucTham(string maNhomAn);
        ResponseResult CapNhatNhanHoSoTuToaAnKhac(ChuyenDonViewModel viewModel);
        ResponseResult XacNhanNhanHoSoPhucTham(HoSoVuAnModel viewModel);
        string GetTenCuaCacDuongSu(int hoSoVuAnID, string nhomAn);

        //Nhan ho so
        //ResponseResult XacNhanNhanHoSo(HoSoVuAnModel viewModel);
        SelectList DanhSachNgaySuaNhanHoSo(int hoSoVuAnId, int giaiDoan, int selected);
        NhanHoSoModel ChiTietNhanHoSoTheoId(int nhanHoSoId);
        ResponseResult SuaChiTietNhanHoSo(NhanHoSoModel viewModel);

        //HoSoVuAn ApDungBPXLHC
        HoSoVuAnApDungModel ChiTietHoSoVuAnApDungBPXLHC(int hoSoVuAnID);
        HoSoVuAnApDungModel ChiTietHoSoVuAnApDungBPXLHCTheoGiaiDoan(int hoSoVuAnID, int giaiDoan);
        HoSoVuAnApDungModel ChiTietHoSoVuAnApDungBPXLHCTheoLog(int id);
        HoSoVuAnApDungModel ChiTietHoSoVuAnApDungBPXLHCTheoId(int hoSoVuAnID);
        ResponseResult ThemHoSoVuAnApDungBPXLHC(HoSoVuAnApDungModel model);
        ResponseResult SuaHoSoVuAnApDungBPXLHC(HoSoVuAnApDungModel model);

        //HoSoVuAn HinhSu
        HoSoVuAnHinhSuModel ChiTietHoSoVuAnHinhSu(int hoSoVuAnID);
        HoSoVuAnHinhSuModel ChiTietHoSoVuAnHinhSuTheoGiaiDoan(int hoSoVuAnID, int giaiDoan);
        HoSoVuAnHinhSuModel ChiTietHoSoVuAnHinhSuTheoLog(int id);
        HoSoVuAnHinhSuModel ChiTietHoSoVuAnHinhSuTheoId(int hoSoVuAnID);
        ResponseResult ThemHoSoVuAnHinhSu(HoSoVuAnHinhSuModel model);
        ResponseResult SuaHoSoVuAnHinhSu(HoSoVuAnHinhSuModel model);

        //ToiDanhTruyTo
        
        IEnumerable<ToiDanhTruyToModel> GetDanhSachToiDanhTruyTo(int hoSoVuAnId);
        ToiDanhTruyToModel ChiTietToiDanhTruyToTheoId(int toiDanhTruyToId);
        ResponseResult ThemToiDanhTruyTo(ToiDanhTruyToModel model);
        ResponseResult SuaToiDanhTruyTo(ToiDanhTruyToModel model);
        ResponseResult XoaToiDanhTruyTo(int toiDanhTruyToId);

        List<ToiDanhTruyTo_KhoanDiemModel> DanhSachToiDanhTruyToKhoanDiemTheoId(int toiDanhTruyToId);
        ResponseResult ThemToiDanhTruyTo_KhoanDiem(ToiDanhTruyTo_KhoanDiemModel model);
        ResponseResult SuaToiDanhTruyTo_KhoanDiem(ToiDanhTruyTo_KhoanDiemModel model);
        ResponseResult XoaToiDanhTruyTo_KhoanDiem(int khoanDiemID);
    }
}
