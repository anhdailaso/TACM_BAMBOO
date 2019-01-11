using System;

namespace Biz.Lib.TACM.SauXetXu.Model.KhangCao
{
    public class ThongTinKhangCaoModel
    {
        public int KhangCaoID { get; set; }
        public int DuongSuID { get; set; }
        public int HoSoVuAnID { get; set; }
        public string NguoiKhangCao { get; set; }
        public string NguoiBiKhangCao { get; set; }
        public string TuCachThamGiaToTung { get; set; }
        public DateTime NgayLamDon { get; set; }
        public DateTime NgayNopDon { get; set; }
        public string HinhThucNopDon { get; set; }
        public string TinhTrangKhangCao { get; set; }
        public string NoiDungKhangCao { get; set; }
        public string TaiLieuChungTuKemTheo { get; set; }
        public string NguoiKhieuNai { get; set; }
        public string NguoiNhanKhieuNai { get; set; }
        public string LyDo { get; set; }
        public DateTime NgayTao { get; set; }
        public string NguoiTao { get; set; }
        public string GhiChu { get; set; }
    }
}
