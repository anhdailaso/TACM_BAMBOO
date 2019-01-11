using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biz.Lib.SettingData.Model;
using Biz.Lib.TACM.NhanDon.Model;
using CongDoanHoSoModel = Biz.Lib.TACM.NhanDon.Model.CongDoanHoSoModel;
using HoSoVuAnModel = Biz.Lib.TACM.NhanDon.Model.HoSoVuAnModel;

namespace Biz.Lib.TACM.NhanDon.IDataAccess
{
    public interface INhanDonDataAccess
    {
        //HoSoVuAn
        IEnumerable<HoSoVuAnModel> DanhSachNgayHoSoVuAn(int hoSoVuAnID, int giaiDoan);
        NhanVienModel ThongTinNhanVien(string maNV);
        IEnumerable<NhanVienModel> DanhSachNhanVien(string nhomChucNang);
        IEnumerable<DuongSuModel> NguyenDonVaBiDon(int hoSoVuAnID);
        IEnumerable<DuongSuModel> DanhSachDuongSu(int ToaAnID);
        HoSoVuAnModel ChiTietHoSoVuAnTheoLog(int id);
        HoSoVuAnModel ChiTietHoSoVuAn(int hoSoVuAnID);
        HoSoVuAnModel ChiTietHoSoVuAnTheoGiaiDoan(int hoSoVuAnID, int giaiDoan);
        IEnumerable<CongDoanHoSoModel> TongSoCongDoanHoSo();
        IEnumerable<CongDoanHoSoModel> CongDoanHoSo(HoSoVuAnModel model, string maNV);
        IEnumerable<HoSoVuAnModel> DanhSachHoSoVuAn(HoSoVuAnModel model, string maNV);
        ResponseResult ThemHoSoVuAn(HoSoVuAnModel model);
        ResponseResult SuaHoSoVuAn(HoSoVuAnModel model);      
        ResponseResult ChuyenTrangThai(HoSoVuAnModel model);
        ResponseResult ChuyenCongDoanHoSo(HoSoVuAnModel model);

        //Duong su
        IEnumerable<DuongSuModel> GetDanhSachDuongSu(int hoSoVuAnId, string duongSuLa = null, string tuCachThamGiaToTung = null);
        DuongSuModel ChiTietDuongSuTheoId(int duongSuId);
        ResponseResult TaoDuongSu(DuongSuModel dbModel);
        ResponseResult SuaDuongSu(DuongSuModel dbModel);
        ResponseResult XoaDuongSu(int duongSuId);
        ResponseResult CapNhatBiCanBiCaoHinhSu(int hoSoVuAnId);

        //Noi dung don
        IEnumerable<NoiDungDonModel> GetDanhSachNgaySuaDoiNoiDungDon(int hoSoVuAnId, int giaiDoan);
        NoiDungDonModel ChiTietNoiDungDonTheoId(int noiDungDonId);
        ResponseResult SuaDoiNoiDungDon(NoiDungDonModel dbModel);

        //Chuyen don
        ChuyenDonModel ChiTietChuyenDonTheoHoSoVuAnID(int hoSoVuAnID, int giaiDoan, int congDoan);
        IEnumerable<ChuyenDonModel> GetDanhSachNgaySuaDoiChuyenDon(int hoSoVuAnId, int giaiDoan, int congDoan);
        ChuyenDonModel ChiTietChuyenDonTheoId(int chuyenDonId);
        ResponseResult SuaDoiChuyenDon(ChuyenDonModel dbModel);

        //Tham phan
        ThamPhanModel ChiTietThamPhanTheoHoSoVuAnID(int hoSoVuAnID);
        IEnumerable<ThamPhanModel> GetDanhSachNgaySuaDoiThamPhan(int hoSoVuAnId, int giaiDoan);
        ThamPhanModel ChiTietThamPhanTheoId(int thamPhanId);
        ResponseResult SuaDoiThamPhan(ThamPhanModel dbModel);

        //Sua doi bo sung don
        IEnumerable<SuaDoiBoSungDonModel> DanhSachNgaySuaDoiBoSungDon(int hoSoVuAnID, int giaiDoan);
        SuaDoiBoSungDonModel ThongTinSuaDoiBoSungDonTheoHoSoVuAnID(int hoSoVuAnID);
        SuaDoiBoSungDonModel ThongTinSuaDoiBoSungDonTheoSuaDoiBoSungDonID(int id);
        ResponseResult ThemSuaDoiBoSungDon(SuaDoiBoSungDonModel viewModel);

		//Tra lai don
        IEnumerable<TraLaiDonModel> DanhSachNgayTraLaiDon(int hoSoVuAnID, int giaiDoan, int CongDoan);
        TraLaiDonModel ThongTinTraLaiDonTheoHoSoVuAnID(int hoSoVuAnID, int CongDoan);
        TraLaiDonModel ThongTinTraLaiDonTheoTraLaiDonID(int TraLaiDonID);
        ResponseResult ThemTraLaiDon(TraLaiDonModel model);

		//Kieu nai viec tra don
        IEnumerable<KhieuNaiTraDonModel> DanhSachNgayKhieuNaiTraDon(int hoSoVuAnID, int giaiDoan);
        KhieuNaiTraDonModel ThongTinKhieuNaiTraDonTheoHoSoVuAnID(int hoSoVuAnID);
        KhieuNaiTraDonModel ThongTinKhieuNaiTraDonTheoKhieuNaiTraDonID(int id);
        ResponseResult ThemKhieuNaiTraDon(KhieuNaiTraDonModel model);

        //Kien nghi viec tra don
        IEnumerable<KienNghiTraDonModel> DanhSachNgayKienNghiTraDon(int hoSoVuAnID, int giaiDoan);
        KienNghiTraDonModel ThongTinKienNghiTraDonTheoHoSoVuAnID(int hoSoVuAnID);
        KienNghiTraDonModel ThongTinKienNghiTraDonTheoKienNghiTraDonID(int id);
        ResponseResult ThemKienNghiTraDon(KienNghiTraDonModel model);

        //Searching on Header by Keyword
        IEnumerable<HoSoVuAnModel> DanhSachHoSoVuAnSearchByKeyword(string keyword);

        //Nhan Ho So Phuc Tham
        IEnumerable<NhanHoSoPhucThamModel> DanhSachHoSoChoPhucTham(string maNhomAn);
        int GetSoLuongHoSoChoPhucTham(string maNhomAn);
        ResponseResult CapNhatNhanHoSoTuToaAnKhac(ChuyenDonModel dbModel);

        //Nhan ho so
        IEnumerable<NhanHoSoModel> GetDanhSachNgaySuaDoiNhanHoSo(int hoSoVuAnId, int giaiDoan);
        NhanHoSoModel ChiTietNhanHoSoTheoId(int nhanHoSoId);
        ResponseResult SuaNhanHoSo(NhanHoSoModel dbModel);
        ResponseResult XacNhanNhanHoSoPhucTham(HoSoVuAnModel dbModel);

        //HoSoVuAn ApDungBPXLHC
        HoSoVuAnApDungModel ChiTietHoSoVuAnApDungBPXLHC(int hoSoVuAnID);
        HoSoVuAnApDungModel ChiTietHoSoVuAnApDungBPXLHCTheoGiaiDoan(int hoSoVuAnID, int giaiDoan);
        HoSoVuAnApDungModel ChiTietHoSoVuAnApDungBPXLHCTheoLog(int id);
        ResponseResult ThemHoSoVuAnApDungBPXLHC(HoSoVuAnApDungModel model);
        ResponseResult SuaHoSoVuAnApDungBPXLHC(HoSoVuAnApDungModel model);

        //HoSoVuAn HinhSu
        HoSoVuAnHinhSuModel ChiTietHoSoVuAnHinhSu(int hoSoVuAnID);
        HoSoVuAnHinhSuModel ChiTietHoSoVuAnHinhSuTheoGiaiDoan(int hoSoVuAnID, int giaiDoan);
        HoSoVuAnHinhSuModel ChiTietHoSoVuAnHinhSuTheoLog(int id);
        ResponseResult ThemHoSoVuAnHinhSu(HoSoVuAnHinhSuModel model);
        ResponseResult SuaHoSoVuAnHinhSu(HoSoVuAnHinhSuModel model);

        //ToiDanhTruyTo
        IEnumerable<ToiDanhTruyToModel> GetDanhSachToiDanhTruyTo(int hoSoVuAnId);
        ToiDanhTruyToModel ChiTietToiDanhTruyToTheoId(int toiDanhTruyToId);
        ResponseResult ThemToiDanhTruyTo(ToiDanhTruyToModel dbModel);
        ResponseResult SuaToiDanhTruyTo(ToiDanhTruyToModel dbModel);
        ResponseResult XoaToiDanhTruyTo(int toiDanhTruyToId, string nguoiTao);

        List<ToiDanhTruyTo_KhoanDiemModel> KhoanDiemTheoToiDanhTruyToId(int toiDanhTruyToId);
        ResponseResult ThemToiDanhTruyTo_KhoanDiem(ToiDanhTruyTo_KhoanDiemModel dbModel);
        ResponseResult SuaToiDanhTruyTo_KhoanDiem(ToiDanhTruyTo_KhoanDiemModel dbModel);
        ResponseResult XoaToiDanhTruyTo_KhoanDiem(int khoanDiemID, string nguoiTao);
    }    
}
