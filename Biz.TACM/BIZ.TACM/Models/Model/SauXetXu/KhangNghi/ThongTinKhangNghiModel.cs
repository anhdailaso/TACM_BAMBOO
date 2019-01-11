using System.Collections.Generic;

namespace Biz.TACM.Models.Model.SauXetXu.KhangNghi
{
    public class ThongTinKhangNghiModel
    {
        public int HoSoVuAnID { get; set; }
        public int KhangNghiId { get; set; }
        public string VienKiemSatKhangNghi { get; set; }
        public string VanBanKhangNghi { get; set; }       
        public string NoiDungKhangNghi { get; set; }
        public string HinhThuc { get; set; }
        public string NgayToaAnNhanVanBan { get; set; }
        public string NguoiTao { get; set; }
        public string NgayTao { get; set; }
        public string NguoiBiKhangNghi { get; set; }
        public List<string> DanhSachNguoiBiKhangNghi { get; set; }
    }
}
