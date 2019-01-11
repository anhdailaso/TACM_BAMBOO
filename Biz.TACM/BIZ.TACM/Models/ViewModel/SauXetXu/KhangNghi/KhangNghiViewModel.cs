using System.Web.Mvc;

namespace Biz.TACM.Models.ViewModel.SauXetXu.KhangNghi
{
    public class KhangNghiViewModel
    {
        public SelectList DanhSachNgayKhangNghi { get; set; }     

        public ThongTinKhangNghiViewModel ThongTinKhangNghi { get; set; }
    }
}