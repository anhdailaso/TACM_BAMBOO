using System;

namespace Biz.Lib.TACM.NhanDon.Model
{
    public class ChuyenDonModel
    {
        public int ChuyenDonID { get; set; }
        public int HoSoVuAnID { get; set; }
        public DateTime NgayRaThongBao { get; set; }
        public DateTime NgayChuyenDon { get; set; }
        public string ToaAnChuyenDi { get; set; }
        public string ToaAnChuyenDen { get; set; }
        public string LyDoChuyenDon { get; set; }
        public string TruongHopChuyen { get; set; }
        public string NhomNghiepVu { get; set; }
        public int GiaiDoan { get; set; }
        public int CongDoanHoSo { get; set; }
        public int TrangThai { get; set; }
        public string NguoiTao { get; set; }
        public DateTime NgayTao { get; set; }
        public string GhiChu { get; set; }
        public string TrangThaiCongDoan { get; set; }
    }
}
