using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Biz.TACM.Models.ViewModel.SauXetXu.KhangCao.Request
{
    public class KhangCaoRequestBase
    {
        public int NguoiKhangCao { get; set; }
        public DateTime NgayLamDon { get; set; }
        public DateTime NgayNopDon { get; set; }
        public string HinhThucNop { get; set; }
        public string TinhTrangKhangCao { get; set; }
        [AllowHtml]
        public string NoiDungKhangCao { get; set; }
        [AllowHtml]
        public string TaiLieuChungTuKemTheo { get; set; }
        public string NguoiKhieuNai { get; set; }
        public string NguoiNhanKhieuNai { get; set; }
        [AllowHtml]
        public string LyDo { get; set; }
        [AllowHtml]
        public string GhiChu { get; set; }
    }
}