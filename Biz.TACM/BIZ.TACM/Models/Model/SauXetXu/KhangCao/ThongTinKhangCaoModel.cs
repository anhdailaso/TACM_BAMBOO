using System;
using System.Collections.Generic;

namespace Biz.TACM.Models.Model.SauXetXu.KhangCao
{
    public class ThongTinKhangCaoModel
    {
        public int KhangCaoId { get; set; }
        public int DuongSuId { get; set; }
        public int HoSoVuAnId { get; set; }
        public string NguoiKhangCao { get; set; }        
        public string TuCachThamGiaToTung { get; set; }
        public string NgayLamDon { get; set; }
        public string NgayNopDon { get; set; }
        public string HinhThucNopDon { get; set; }
        public string TinhTrangKhangCao { get; set; }
        public string NoiDungKhangCao { get; set; }
        public string TaiLieuChungTuKemTheo { get; set; }
        public string NguoiKhieuNai { get; set; }
        public string NguoiNhanKhieuNai { get; set; }
        public string LyDo { get; set; }
        public string NgayTao { get; set; }
        public string NguoiTao { get; set; }
        public string GhiChu { get; set; }
        public string NguoiBiKhangCao { get; set; }
        public List<string> DanhSachNguoiBiKhangCao { get; set; }
    }
}