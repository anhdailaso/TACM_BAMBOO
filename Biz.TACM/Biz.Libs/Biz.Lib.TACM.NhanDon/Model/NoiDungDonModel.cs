using System;

namespace Biz.Lib.TACM.NhanDon.Model
{
    public class NoiDungDonModel
    {
        public int NoiDungDonID { get; set; }
        public int HoSoVuAnID { get; set; }
        public string NoiDungDon { get; set; }
        public string NhomNghiepVu { get; set; }
        public int GiaiDoan { get; set; }
        public int CongDoanHoSo { get; set; }
        public int TrangThai { get; set; }
        public string NguoiTao { get; set; }
        public DateTime NgayTao { get; set; }
        public string GhiChu { get; set; }
    }
}
