using System;

namespace Biz.Lib.TACM.ThuLy.Model
{
    public class ThongTinThuLyModel
    {
        public int ThongTinThuLyID { get; set; }
        public int HoSoVuAnID { get; set; }
        public string SoThuLy { get; set; }
        public string TruongHopThuLy { get; set; }
        public string ThuLyTheoThuTuc { get; set; }
        public DateTime? NgayThuLy { get; set; }
        public string LoaiQuanHe { get; set; }
        public string QuanHePhapLuat { get; set; }
        public string ThoiHanGiaiQuyet { get; set; }
        public DateTime? ThoiHanGiaiQuyetTuNgay { get; set; }
        public DateTime? ThoiHanGiaiQuyetDenNgay { get; set; }
        public string NoiDungYeuCau { get; set; }
        public string NoiDungKhangCao { get; set; }
        public string TaiLieuChungTuKemTheo { get; set; }
        public string NhomNghiepVu { get; set; }
        public string QuyetDinh { get; set; }
        public string GiaiDoan { get; set; }
        public int CongDoanHoSo { get; set; }
        public int TrangThai { get; set; }
        public string NguoiTao { get; set; }
        public DateTime NgayTao { get; set; }
        public string GhiChu { get; set; }
    }
}
