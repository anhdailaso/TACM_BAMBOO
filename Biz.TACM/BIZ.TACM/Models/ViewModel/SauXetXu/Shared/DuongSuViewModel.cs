using System;

namespace Biz.TACM.Models.ViewModel.SauXetXu.Shared
{
    public class DuongSuViewModel
    {
        public int DuongSuId { get; set; }
        public string HoVaTen { get; set; }
        public string TuCachThamGiaToTung { get; set; }
        public string NgayNhanPhatHanhBanAn { get; set; }
        public bool Checked { get; set; }
    }
}