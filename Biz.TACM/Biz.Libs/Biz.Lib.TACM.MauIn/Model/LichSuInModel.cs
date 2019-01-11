using System;

namespace Biz.Lib.TACM.MauIn.Model
{
    public class LichSuInModel
    {
        public int HoSoVuAnID { get; set; }
        public int? SoMauIn { get; set; }
        public string DuongDanFileIn { get; set; }
        public string NhomNghiepVu { get; set; }
        public int GiaiDoan { get; set; }
        public int CongDoanHoSo { get; set; }
        public int TrangThai { get; set; }
        public string NguoiTao { get; set; }
        public DateTime NgayTao { get; set; }
        public string GhiChu { get; set; }
        public string NhomAn { get; set; }
    }
}
