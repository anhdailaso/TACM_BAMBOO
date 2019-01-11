using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Biz.TACM.Models.ViewModel.SauXetXu.KhangNghi.Request
{
    public class KhangNghiRequest
    {
        public int HoSoVuAnId { get; set; }
        public string VienKiemSatKhangNghi { get; set; }
        public string VanBanKhangNghi { get; set; }
        public DateTime NgayToaAnNhanVanBan { get; set; }
        public string HinhThuc { get; set; }
        [AllowHtml]
        public string NoiDungKhangNghi { get; set; }
        public List<string> DanhSachNguoiBiKhangNghi { get; set; }
    }
}