using System;

namespace Biz.Lib.TACM.SauXetXu.Model.KhangNghi
{
    public class ThongTinKhangNghiModel
    {
        public int HoSoVuAnID { get; set; }
        public int KhangNghiID { get; set; }
        public string VienKiemSatKhangNghi { get; set; }
        public string NguoiBiKhangNghi { get; set; }
        public string VanBanKhangNghi { get; set; }
        public string NoiDungKhangNghi { get; set; }
        public string HinhThuc { get; set; }
        public DateTime NgayToaAnNhanVanBan { get; set; }
        public string NguoiTao { get; set; }
        public DateTime NgayTao { get; set; }
    }
}
