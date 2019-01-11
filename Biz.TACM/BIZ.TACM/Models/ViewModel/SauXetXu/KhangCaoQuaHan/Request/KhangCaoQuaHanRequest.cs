using System.Web.Mvc;

namespace Biz.TACM.Models.ViewModel.SauXetXu.KhangCaoQuaHan.Request
{
    public class KhangCaoQuaHanRequest
    {
        public int HoSoVuAnId { get; set; }
        public string KhangCaoHayKhangNghi { get; set; }
        [AllowHtml]
        public string LyDo { get; set; }
        
        [AllowHtml]
        public string KetQua { get; set; }
    }
}