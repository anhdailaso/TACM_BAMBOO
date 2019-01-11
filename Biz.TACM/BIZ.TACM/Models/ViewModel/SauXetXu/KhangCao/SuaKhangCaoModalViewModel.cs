using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Biz.TACM.Models.ViewModel.SauXetXu.KhangCao
{
    public class SuaKhangCaoModalViewModel
    {
        public int KhangCaoId { get; set; }
        public int DuongSuId { get; set; }
        public string NgayLamDon { get; set; }
        public string NgayNopDon { get; set; }
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
        
        public SelectList DanhSachNguoiKhieuNai { get; set; }
        public SelectList DanhSachHinhThucNop { get; set; }
        public SelectList DanhSachTinhTrangKhangCao { get; set; }
        public SelectList DanhSachDuongSu { get; set; }

        public string NguoiBiKhangCao { get; set; }
        public List<string> DanhSachNguoiBiKhangCao { get; set; }
    }
}