using System.Collections.Generic;
using System.Web.Mvc;

namespace Biz.TACM.Models.ViewModel.SauXetXu.KhangNghi
{
    public class ThongTinKhangNghiViewModel
    {
        public int HoSoVuAnId { get; set; }
        public int KhangNghiId { get; set; }
        public string VienKiemSatKhangNghi { get; set; }
        public string VanBanKhangNghi { get; set; }
        public string NoiDungKhangNghi { get; set; }
        public string HinhThuc { get; set; }
        public string NgayToaAnNhanVanBan { get; set; }
        public string NguoiTao { get; set; }
        public string NgayTao { get; set; }
        public string NguoiBiKhangCao { get; set; }
        public List<string> DanhSachNguoiBiKhangNghi { get; set; }
    }
}