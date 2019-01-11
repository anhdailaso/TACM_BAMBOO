using System;

namespace Biz.TACM.Models.Model.SauXetXu.KhangCao
{
    public class KhangCaoModel
    {
        public int KhangCaoId { get; set; }
        public string NguoiKhangCao { get; set; }
        public string TuCachThamGiaToTung { get; set; }
        public string NgayLamDon { get; set; }
        public string NgayNopDon { get; set; }
        public string HinhThucNopDon { get; set; }
        public string TinhTrangKhangCao { get; set; }
        public string TaiLieuChungTuKemTheo { get; set; }
        public string NguoiKhieuNai { get; set; }
        public string NguoiNhanKhieuNai { get; set; }
        public string LyDo { get; set; }
    }
}