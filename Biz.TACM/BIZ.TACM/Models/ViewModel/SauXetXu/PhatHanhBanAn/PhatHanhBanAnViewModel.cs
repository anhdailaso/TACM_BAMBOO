using System;
using System.Web.Mvc;

namespace Biz.TACM.Models.ViewModel.SauXetXu.PhatHanhBanAn
{
    public class PhatHanhBanAnViewModel
    {
        public SelectList DanhSachNgayPhatHanhBanAn { get; set; }
        public ThongTinPhatHanhBanAnViewModel ThongTinPhatHanhBanAn { get; set; }
    }
}