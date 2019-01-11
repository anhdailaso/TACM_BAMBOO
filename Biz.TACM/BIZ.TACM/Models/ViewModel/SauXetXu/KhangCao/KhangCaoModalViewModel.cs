using System.Web.Mvc;

namespace Biz.TACM.Models.ViewModel.SauXetXu.KhangCao
{
    public class KhangCaoModalViewModel
    {
        public SelectList DanhSachHinhThucNop { get; set; }
        public SelectList DanhSachTinhTrangKhangCao { get; set; }
        public SelectList DanhSachDuongSu { get; set; }
        public SelectList DanhSachNguoiKhieuNai { get; set; }
        public SelectList DanhSachDuongSuBiKhangCao { get; set; }
    }
}