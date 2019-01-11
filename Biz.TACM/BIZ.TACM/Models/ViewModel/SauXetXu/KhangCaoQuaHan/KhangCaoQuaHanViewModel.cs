using System.Web.Mvc;

namespace Biz.TACM.Models.ViewModel.SauXetXu.KhangCaoQuaHan
{
    public class KhangCaoQuaHanViewModel
    {
        public SelectList DanhSachNgayKhangCaoQuaHan { get; set; }
        public ThongTinKhangCaoQuaHanViewModel ThongTinKhangCaoQuaHan { get; set; }
    }
}