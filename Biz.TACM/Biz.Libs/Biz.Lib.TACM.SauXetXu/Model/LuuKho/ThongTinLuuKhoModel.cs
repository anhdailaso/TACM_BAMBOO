using System;

namespace Biz.Lib.TACM.SauXetXu.Model.LuuKho
{
    public class ThongTinLuuKhoModel
    {
        public int LuuKhoID { get; set; }
        public DateTime NgayLuu { get; set; }
        public string NguoiGiao { get; set; }
        public string NguoiNhanLuu { get; set; }
        public string GhiChuTinhTrangHoSo { get; set; }
        public string GhiChu { get; set; }
        public string NguoiTao { get; set; }
        public DateTime NgayTao { get; set; }
    }
}
