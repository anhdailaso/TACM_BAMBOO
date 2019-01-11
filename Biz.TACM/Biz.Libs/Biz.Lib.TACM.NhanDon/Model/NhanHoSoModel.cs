using System;

namespace Biz.Lib.TACM.NhanDon.Model
{
    public class NhanHoSoModel
    {
        public int NhanHoSoID { get; set; }
        public int HoSoVuAnID { get; set; }
        public DateTime NgayNhanHoSo { get; set; }
        public string NguoiNhanHoSo { get; set; }
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
