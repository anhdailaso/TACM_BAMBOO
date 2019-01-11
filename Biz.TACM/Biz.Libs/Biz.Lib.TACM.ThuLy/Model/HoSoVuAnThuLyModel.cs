using System;

namespace Biz.Lib.TACM.ThuLy.Model
{
    public class HoSoVuAnThuLyModel
    {
        public int HoSoVuAnID { get; set; }
        public int HoSoVuAnLogID { get; set; }
        public string MaHoSo { get; set; }
        public int SoHoSo { get; set; }
        public string NhomAn { get; set; }
        public int GiaiDoan { get; set; }
        public int CongDoanHoSo { get; set; }
        public string TrangThaiCongDoan { get; set; }
        public DateTime? NgayLamDon { get; set; }
        public DateTime? NgayNopDonTaiToaAn { get; set; }
        public string HinhThucGoiDon { get; set; }
        public string LoaiDon { get; set; }
        public string LoaiQuanHe { get; set; }
        public string QuanHePhapLuat { get; set; }
        public string YeuToNuocNgoai { get; set; }
        public string NguoiKyXacNhanDaNhanDon { get; set; }
        public string CanBoNhanDon { get; set; }
        public string ThuLyTheoThuTuc { get; set; }
        public string ThamPhan { get; set; }
        public string ThamPhanKhac { get; set; }
        public string HoiThamNhanDan { get; set; }
        public string ThuKy { get; set; }
        public string KiemSatVien { get; set; }
        public int TrangThai { get; set; }
        public decimal? GiaTriDuNop { get; set; }
        public DateTime? HanNopAnPhi { get; set; }
        public DateTime? NgayChuyenDon { get; set; }
        public DateTime? NgayTraDon { get; set; }
        public DateTime? NgayKhieuNai { get; set; }
        public string NguoiTao { get; set; }
        public DateTime NgayTao { get; set; }
        public string GhiChu { get; set; }
    }
}
