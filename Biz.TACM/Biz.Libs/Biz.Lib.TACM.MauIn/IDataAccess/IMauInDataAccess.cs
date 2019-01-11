using System;
using System.Collections.Generic;
using Biz.Lib.TACM.MauIn.Model;

namespace Biz.Lib.TACM.MauIn.IDataAccess
{
    public interface IMauInDataAccess
    {
        //mau in so 24
        MauInSo24Model ChiTietMauInSo24(int hoSoVuAnId);
        IEnumerable<DuongSuModel> DanhSachDuongSuMauInSo24(int hoSoVuAnId);

        //mau in so 29
        MauInSo29Model DuLieuMauInSo29(int hoSoVuAnId);
        IEnumerable<DuongSuModel> DanhSachDuongSuMauInSo29(int hoSoVuAnId);

        //mau in so 30
        MauInSo30Model ChiTietMauInSo30(int hoSoVuAnId);
        IEnumerable<DuongSuModel> DanhSachDuongSuMauInSo30(int hoSoVuAnId);

        //mau in so 47
        MauInSo47Model DuLieuMauInSo47(int hoSoVuAnId);
        IEnumerable<DuongSuModel> DanhSachDuongSuMauInSo47(int hoSoVuAnId);
        IEnumerable<ThamPhanDuKhuyetMauInSo47Model> DanhSachThamPhanDuKhuyetMauInSo47(int hoSoVuAnId);
        IEnumerable<HoiThamNhanDanDuKhuyetMauInSo47Model> DanhSachHoiThamNhanDanDuKhuyetMauInSo47(int hoSoVuAnId);
        IEnumerable<KiemSatVienDuKhuyetMauInSo47Model> DanhSachKiemSatVienDuKhuyetMauInSo47(int hoSoVuAnId);
        MauInSo47PTModel DuLieuMauInSo47PhucTham(int hoSoVuAnId);
        IEnumerable<DuongSuModel> NguoiKhangCaoMauInSo47(int hoSoVuAnId);

        //mau in so 61
        MauInSo61Model DuLieuMauInSo61(int hoSoVuAnId);
        IEnumerable<NguoiKhangCaoMauInSo61Model> DanhSachNguoiKhangCaoMauInSo61(int hoSoVuAnId);

        //mau in so 65
        MauInSo65Model DuLieuMauInSo65(int hoSoVuAnId);
        IEnumerable<NguoiKhangCaoMauInSo65Model> NguoiKhangCao(int hoSoVuAnId);
   
        //mau in qd phan cong tham phan
        MauInQuyetDinhPCTPModel ChiTietMauInQuetDinhPCTP(int hoSoVuAnId, int giaiDoan);
        List<ThuKyDuKhuyetModel> DanhSachThuKyDuKhuyet(int hoSoVuAnId);
        MauInQDGiaHanCBXX ChiTietMauInGiaHanCBXX(int hoSoVuAnId, int giaiDoan);
        MauInQDTamHoanModel ChiTietMauInTamHoan(int hoSoVuAnId, int giaiDoan);
        MauInQuyetDinhDinhChiModel ChiTietMauInQuyetDinhDinhChi(int hoSoVuAnId, string loai);
        //mau in giay trieu tap
        MauInGiayTrieuTapModel ChiTietMauInGiayTrieuTap(int hoSoVuAnId);
        IEnumerable<DuongSuModel> DanhSachDuongSuMauInGiayTrieuTap(int hoSoVuAnId);
        MauInLenhTrichXuatModel ChiTietMauInLenhTrichXuat(int hoSoVuAnId, int giaiDoan);
        MauInTamGiamModel ChiTietMauInTamGiam_4_5_9(int hoSoVuAnId, int giaiDoan);
        List<DuongSuModel> DanhSachBiCaoHinhPhat(int hoSoVuAnId);

        //mau in bia ho so
        MauInBiaHoSoModel ChiTietMauInBiaHoSo(int hoSoVuAnId);
        MauInBiaHoSoNhanDonModel ChiTietMauInBiaHoSoNhanDon(int hoSoVuAnId);

        //gxn da nhan don khang cao
        MauInDaNhanKhangCaoModel ChiTietGXNKhangCao(int hoSoVuAnId);
        IEnumerable<NguoiKhangCaoGXNModel> DanhSachNguoiKhangCaoGXN(int hoSoVuAnId);

        //bienbangiaonhan
        MauInBBGiaoNhanModel ChiTietMauInBienBanGiaoNhan(int hoSoVuAnId);

        //luu lich su in
        ResponseResult LuuLichSuIn(LichSuInModel dbModel);

        //danh sach duong su
        IEnumerable<DuongSuModel> DanhSachDuongSuTheoHoSoVuAnID(int hoSoVuAnId);

        //danh sach nhom an
        IEnumerable<NhomAnModel> DanhSachNhomAnTheoToaAn(int toaAnId);
        IEnumerable<ToiDanhTruyToModel> DanhSachToiDanhTruyTo(int hoSoVuAnId);

        // danh sach mau in
        IEnumerable<MauInModel> DanhSachMauInTheoGiaiDoanVaNhomAn(int? giaiDoan, string nhomAn);
        IEnumerable<MauInModel> DanhSachMauInSearchByKeyword(string keyword);
    }
}