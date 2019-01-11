using System;
using System.Collections.Generic;

namespace Biz.Lib.TACM.SauXetXu.Model.KhangCao.DataRequest
{
    public class KhangCaoDataRequest : DataRequestBase
    {
        public int NguoiKhangCao { get; set; }
        public DateTime NgayLamDon { get; set; }
        public DateTime NgayNopDon { get; set; }
        public string HinhThucNopDon { get; set; }
        public string TinhTrangKhangCao { get; set; }
        public string NoiDungKhangCao { get; set; }
        public string TaiLieuChungTuKemTheo { get; set; }
        public string NguoiBiKhangCao { get; set; }
        public string NguoiKhieuNai { get; set; }
        public string NguoiNhanKhieuNai { get; set; }
        public string LyDo { get; set; }
    }
}
