using System.Web.Mvc;

namespace Biz.TACM.Models.ViewModel.SauXetXu.LuuKho
{
    public class LuuKhoViewModel
    {
        public SelectList DanhSachNgayLuuKho { get; set; }

        public ThongTinLuuKhoViewModel ThongTinLuuKho { get; set; }
    }
}