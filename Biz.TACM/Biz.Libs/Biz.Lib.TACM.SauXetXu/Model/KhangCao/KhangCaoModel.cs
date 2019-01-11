using System;

namespace Biz.Lib.TACM.SauXetXu.Model.KhangCao
{
    public class KhangCaoModel
    {
        public int KhangCaoID { get; set; }
        public string NguoiKhangCao { get; set; }
        public string TuCachThamGiaToTung { get; set; }
        public DateTime NgayLamDon { get; set; }
        public DateTime NgayNopDon { get; set; }
        public string HinhThucNopDon { get; set; }
        public string TinhTrangKhangCao { get; set; }
        public string TaiLieuChungTuKemTheo { get; set; }
        public string NguoiKhieuNai { get; set; }
        public string NguoiNhanKhieuNai { get; set; }
        public string LyDo { get; set; }
        public string GhiChu { get; set; }
    }
}
