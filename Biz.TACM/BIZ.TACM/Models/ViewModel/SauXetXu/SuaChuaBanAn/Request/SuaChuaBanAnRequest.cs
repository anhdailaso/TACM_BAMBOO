using System;
using System.Web.Mvc;

namespace Biz.TACM.Models.ViewModel.SauXetXu.SuaChuaBanAn.Request
{
    public class SuaChuaBanAnRequest
    {
        public int HoSoVuAnId { get; set; }
        public string LyDo { get; set; }
        public DateTime NgaySuaChua { get; set; }
        public string NguoiKy { get; set; }
        [AllowHtml]
        public string NoiDungSuaChua { get; set; }
        public string DinhKemFile { get; set; }
    }
}