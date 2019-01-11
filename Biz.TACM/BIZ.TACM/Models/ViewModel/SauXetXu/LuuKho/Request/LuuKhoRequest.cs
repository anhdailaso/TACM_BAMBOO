using System;
using System.Web.Mvc;

namespace Biz.TACM.Models.ViewModel.SauXetXu.LuuKho.Request
{
    public class LuuKhoRequest
    {
        public int HoSoVuAnId { get; set; }
        public DateTime NgayLuu { get; set; }
        public string NguoiGiao { get; set; }
        public string NguoiNhanLuu { get; set; }
        [AllowHtml]
        public string GhiChuTinhTrangHoSo { get; set; }
        [AllowHtml]
        public string GhiChu { get; set; }
    }
}