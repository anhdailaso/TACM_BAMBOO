using System;

namespace Biz.Lib.TACM.NhanDon.Model
{
    public class ThamPhanModel
    {
        public int ThamPhanID { get; set; }
        public int HoSoVuAnID { get; set; }
        public string ThamPhan { get; set; }
        public DateTime NgayPhanCong { get; set; }
        public string TenNguoiPhanCong { get; set; }
        public string HoiThamNhanDan2 { get; set; }
        public string NhomNghiepVu { get; set; }
        public int GiaiDoan { get; set; }
        public int CongDoanHoSo { get; set; }
        public int TrangThai { get; set; }
        public string NguoiTao { get; set; }
        public DateTime NgayTao { get; set; }
        public string GhiChu { get; set; }
    }
}
