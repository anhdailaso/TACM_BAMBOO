using System;

namespace Biz.Lib.TACM.SauXetXu.Model
{
    public class DataRequestBase
    {
        public int HoSoVuAnId { get; set; }
        public int? GiaiDoan { get; set; }
        public int? CongDoanHoSo { get; set; }
        public DateTime NgayTao { get; set; }
        public string NguoiTao { get; set; }
        public string GhiChu { get; set; }

    }
}
