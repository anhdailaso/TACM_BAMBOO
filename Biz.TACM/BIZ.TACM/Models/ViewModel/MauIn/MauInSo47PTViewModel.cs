using System;
using System.Collections.Generic;
using Biz.Lib.TACM.MauIn.Model;
using System.Linq;
using System.Web;

namespace Biz.TACM.Models.ViewModel.MauIn
{
    public class MauInSo47PTViewModel
    {
        public string MaToaAn { get; set; }
        public string MaHoSo { get; set; }
        public string SoHoSo { get; set; }
        public string SoThuLy { get; set; }
        public string NhomAn { get; set; }
        public string QuanHePhapLuat { get; set; }
        public string ThamPhanChuToa { get; set; }
        public string GioiTinhThamPhanChuToa { get; set; }
        public string ThamPhan1 { get; set; }
        public string GioiTinhThamPhan1 { get; set; }
        public string ThamPhan2 { get; set; }
        public string GioiTinhThamPhan2 { get; set; }
        public string HoiThamNhanDan { get; set; }
        public string GioiTinhHoiThamNhanDan { get; set; }
        public string ThuKy { get; set; }
        public string GioiTinhThuKy { get; set; }
        public string KiemSatVien { get; set; }
        public string GioiTinhKiemSatVien { get; set; }
        public string TenToaAn { get; set; }
        public string DiaChiToaAn { get; set; }
        public DateTime NgayThuLy { get; set; }
        public DateTime NgayRaQuyetDinhXetXu { get; set; }
        public DateTime ThoiGianMoPhienToa { get; set; }
        public string DiaDiemMoPhienToa { get; set; }
        public string LoaiQuanHe { get; set; }
        public string VuAnDuocXetXu { get; set; }
        public string ThuTuc { get; set; }
        public string VienKiemSatKhangNghi { get; set; }
        public int HoiDong { get; set; }
        public string VatChung { get; set; }
        public string ToiDanh { get; set; }
        public string DieuLuat { get; set; }
        public string HinhPhat { get; set; }
        public List<DuongSuModel> DanhSachDuongSu { get; set; }
        public List<DuongSuModel> NguoiKhangCao { get; set; }
        public List<ThamPhanDuKhuyetMauIn47ViewModel> DanhSachThamPhanDuKhuyet { get; set; }
        public class ThamPhanDuKhuyetMauIn47ViewModel
        {
            public string ThamPhanDuKhuyet { get; set; }
            public string GioiTinh { get; set; }
        }
        public List<HoiThamNhanDanDuKhuyetMauInSo47ViewModel> DanhSachHoiThamNhanDanDuKhuyet { get; set; }
        public class HoiThamNhanDanDuKhuyetMauInSo47ViewModel
        {
            public string HoiThamNhanDanDuKhuyet { get; set; }
            public string GioiTinh { get; set; }
        }
        public List<KiemSatVienDuKhuyetMauInSo47ViewModel> DanhSachKiemSatVienDuKhuyet { get; set; }
        public class KiemSatVienDuKhuyetMauInSo47ViewModel
        {
            public string KiemSatVienDuKhuyet { get; set; }
            public string GioiTinh { get; set; }
        }
        public List<ThuKyDuKhuyetModel> DanhSachThuKyDuKhuyet { get; set; }
        public List<DuongSuModel> DanhSachBiCao { get; set; }
        public string SoQuyetDinh { get; set; }
        public DateTime NgayRaQuyetDinh { get; set; }
        public string ToaAnSoTham { get; set; }
        public string TenCoQuanDeNghi { get; set; }
        public string BienPhapXuLyHanhChinh { get; set; }
    }
}