using System.Web.Mvc;

namespace Biz.TACM.Models.ViewModel.SauXetXu.SuaChuaBanAn
{
    public class SuaChuaBanAnViewModel
    {
        public SelectList DanhSachNgaySuaChuaBanAn { get; set; }

        public ThongTinSuaChuaBanAnViewModel ThongTinSuaChuaBanAn { get; set; }
    }
}