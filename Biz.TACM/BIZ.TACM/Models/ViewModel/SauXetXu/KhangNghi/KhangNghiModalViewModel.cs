using System.Web.Mvc;

namespace Biz.TACM.Models.ViewModel.SauXetXu.KhangNghi
{
    public class KhangNghiModalViewModel
    {
        public SelectList DanhSachVienKiemSatKhangNghi { get; set; }
        public SelectList DanhSachHinhThuc { get; set; }
    }
}