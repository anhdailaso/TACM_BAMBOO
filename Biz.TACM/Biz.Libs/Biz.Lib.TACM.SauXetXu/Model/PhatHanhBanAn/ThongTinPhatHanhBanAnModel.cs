using System;

namespace Biz.Lib.TACM.SauXetXu.Model.Shared
{
    public class ThongTinPhatHanhBanAnModel
    {
        public int PhatHanhBanAnID { get; set; }
        public DateTime NgayPhatHanh { get; set; }
        public DateTime? HieuLuc { get; set; }
        public string NguoiTao { get; set; }
        public DateTime NgayTao { get; set; }
    }
}
