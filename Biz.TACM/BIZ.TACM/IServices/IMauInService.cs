using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Biz.Lib.SettingData.Model;
using Biz.Lib.TACM.MauIn.Model;
using Biz.TACM.Models.ViewModel.MauIn;

namespace Biz.TACM.IServices
{
    public interface IMauInService
    {
        MauInSo24ViewModel ChiTietMauInSo24(int hoSoVuAnId);
        MauInSo30ViewModel ChiTietMauInSo30(int hoSoVuAnId);
        ResponseResult LuuLichSuIn(LichSuInViewModel viewModel);
        MauInSo29ViewModel DuLieuMauInSo29(int hoSoVuAnId);
        MauInSo47ViewModel DuLieuMauInSo47(int hoSoVuAnId);
        MauInSo47PTViewModel DuLieuMauInSo47PT(int hoSoVuAnId);
        MauInSo61ViewModel DuLieuMauInSo61(int hoSoVuAnId);
        MauInSo65ViewModel DuLieuMauInSo65(int hoSoVuAnId);
        MauInQuyetDinhPCTPViewModel DuLieuMauInQuyetDinhPCTP(int hoSoVuAnId, int giaiDoan);
        MauInQuyetDinhGiaHanCBXXViewModel ChiTietMauInGiaHanCBXX(int hoSoVuAnId, int giaiDoan);
        MauInQuyetDinhTamHoanViewModel ChiTietMauInTamHoan(int hoSoVuAnId, int giaiDoan);
        MauInQuyetDinhDinhChiViewModel ChiTietMauInQuyetDinhDinhChi(int hoSoVuAnId, string loai);
        MauInLenhTrichXuatViewModel ChiTietMauInLenhTrichXuat(int hoSoVuAnId, int giaiDoan);
        MauInGiayTrieuTapViewModel DuLieuMauInGiayTrieuTap(int hoSoVuAnId);
        MauInQDTamGiamViewModel ChiTietMauInTamGiam_4_5_9(int hoSoVuAnId, int giaiDoan, int congDoan, int mauInSo);
        MauInGXNKhangCaoViewModel ChiTietGXNKhangCao(int hoSoVuAnId);
        MauInBiaHoSoViewModel DuLieuMauInBiaHoSo(int hoSoVuAnId);
        MauInBBGiaoNhanModel ChiTietMauInBienBanGiaoNhan(int hoSoVuAnId);
        MauInBiaHoSoNhanDonViewModel ChiTietMauInBiaHoSoNhanDon(int hoSoVuAnId);
        IEnumerable<Lib.TACM.MauIn.Model.NhomAnModel> DanhSachNhomAnTheoToaAn(int toaAnId);
        IEnumerable<MauInModel> DanhSachMauInTheoGiaiDoanVaNhomAn(int? giaiDoan, string nhomAn);
        IEnumerable<MauInModel> DanhSachMauInSearchByKeyword(string keyword);
        
        string[] MauInHC02Doc(MauInSo24ViewModel mauInObject, string filePath, string templatePath);
        string[] MauInHC04Doc(MauInSo29ViewModel mauInObject, string filePath, string templatePath);
        string[] MauInHC06Doc(MauInSo30ViewModel mauInObject, string filePath, string templatePath);
        string[] MauInHCQuyetDinhPCTPHTDoc(MauInQuyetDinhPCTPViewModel mauInObject, string filePath, string templatePath);
        string[] MauInHCQuyetDinhDuaVuAnXetXuSoThamDoc(MauInSo47ViewModel mauInObject, string filePath, string templatePath);
        string[] MauInHCGiayTrieuTapDoc(MauInGiayTrieuTapViewModel mauInObject, string filePath, string templatePath);
        string[] MauInGiayXacNhanKhangCaoDoc(MauInGXNKhangCaoViewModel mauInObject, string filePath, string templatePath);
        string[] MauInHC31Doc(MauInSo61ViewModel mauInObject, string filePath, string templatePath);
        string[] MauInHC35Doc(MauInSo65ViewModel mauInObject, string filePath, string templatePath);
        string[] MauInHCQuyetDinhDuaVuAnXetXuPhucThamDoc(MauInSo47PTViewModel mauInObject, string filePath, string templatePath);
        string[] MauInHCBiaHoSoNhanDonDoc(MauInBiaHoSoNhanDonViewModel mauInObject, string filePath, string templatePath);
        string[] MauInHCBiaHoSoDoc(MauInBiaHoSoViewModel mauInObject, string filePath, string templatePath);

        string[] MauInHSQuyetDinhPCTPDoc(MauInQuyetDinhPCTPViewModel mauInObject, string filePath, string templatePath);
        string[] MauInHSQuyetDinhPCHTTKDoc(MauInQuyetDinhPCTPViewModel mauInObject, string filePath, string templatePath);
        string[] MauInHSQuyetDinhDuaVuAnXetXuSoThamDoc(MauInSo47ViewModel mauInObject, string filePath, string templatePath);
        string[] MauInHSGiayTrieuTapNguoiThamGiaTTDoc(MauInGiayTrieuTapViewModel mauInObject, string filePath, string templatePath);
        string[] MauInHSGiayTrieuTapBiCaoDoc(MauInGiayTrieuTapViewModel mauInObject, string filePath, string templatePath);
        string[] MauInHSQuyetDinhDuaVuAnXetXuPhucThamDoc(MauInSo47PTViewModel mauInObject, string filePath, string templatePath);
        string[] MauInHSBiaHoSoSTDoc(MauInBiaHoSoViewModel mauInObject, string filePath, string templatePath);
        string[] MauInHSBiaHoSoPTDoc(MauInBiaHoSoViewModel mauInObject, string filePath, string templatePath);

        string[] MauIn01BiaHoSoNhanDonDoc(MauInBiaHoSoNhanDonViewModel mauInObject, string filePath, string templatePath);
        string[] MauIn02GiayXacNhanDaNhanDonDoc(MauInSo24ViewModel mauInObject, string filePath, string templatePath);
        string[] MauIn03QuyetDinhPCTPGiaiQuyetDonDoc(MauInQuyetDinhPCTPViewModel mauInObject, string filePath, string templatePath);
        string[] MauIn04ThongBaoNopTienTamUngSTDoc(MauInSo29ViewModel mauInObject, string filePath, string templatePath);
        string[] MauIn05ThongBaoThuLySTDoc(MauInSo30ViewModel mauInObject, string filePath, string templatePath);
        string[] MauIn0613QuyetDinhPCTPDoc(MauInQuyetDinhPCTPViewModel mauInObject, string filePath, string templatePath, string pathChuKy);
        string[] MauIn0716BiaHoSoDoc(MauInBiaHoSoViewModel mauInObject, string filePath, string templatePath);
        string[] MauIn08QuyetDinhDuaVuAnXetXuSoThamDoc(MauInSo47ViewModel mauInObject, string filePath, string templatePath);
        string[] MauIn09GiayTrieuTapDoc(MauInGiayTrieuTapViewModel mauInObject, string filePath, string templatePath);
        string[] MauIn11ThongBaoNopTienTamUngPTDoc(MauInSo61ViewModel mauInObject, string filePath, string templatePath);
        string[] MauIn12ThongBaoThuLyPTDoc(MauInSo65ViewModel mauInObject, string filePath, string templatePath);
        string[] MauIn14QuyetDinhDuaVuAnXetXuPhucThamDoc(MauInSo47PTViewModel mauInObject, string filePath, string templatePath);

        string[] MauInKDBiaHoSoDoc(MauInBiaHoSoViewModel mauInObject, string filePath, string templatePath);

        string[] MauInAD01Doc(MauInSo30ViewModel mauInObject, string filePath, string templatePath);
        string[] MauInADQuyetDinhPCTPDoc(MauInQuyetDinhPCTPViewModel mauInObject, string filePath, string templatePath);
        string[] MauInADQuyetDinhMoPhienHopSoThamDoc(MauInSo47ViewModel mauInObject, string filePath, string templatePath);
        string[] MauInADGiayTrieuTapDoc(MauInGiayTrieuTapViewModel mauInObject, string filePath, string templatePath);
        string[] MauInADQuyetDinhDinhChiDoc(MauInQuyetDinhDinhChiViewModel mauInObject, string filePath, string templatePath);
        string[] MauInADBiaHoSoDoc(MauInBiaHoSoViewModel mauInObject, string filePath, string templatePath);
        string[] MauInADQuyetDinhMoPhienHopPhucThamDoc(MauInSo47PTViewModel mauInObject, string filePath, string templatePath);

        string converter(string filePath, string fromExtension, string toExtension);
    }
}
