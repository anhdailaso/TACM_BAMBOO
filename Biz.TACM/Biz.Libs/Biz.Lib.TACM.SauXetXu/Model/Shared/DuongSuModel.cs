using System;

namespace Biz.Lib.TACM.SauXetXu.Model.Shared
{
    public class DuongSuModel
    {
        public int DuongSuID { get; set; }
        public string HoVaTen { get; set; }
        public string TuCachThamGiaToTung { get; set; }
        public DateTime? NgayNhanPhatHanhBanAn { get; set; }
        public bool Checked { get; set; }
    }
}
