using System;
using System.Collections.Generic;

namespace Biz.Lib.TACM.SauXetXu.Model.Shared.DataRequest
{
    public class PhatHanhBanAnDataRequest : DataRequestBase
    {
        public DateTime NgayPhatHanh { get; set; }
        public DateTime? HieuLuc { get; set; }
        public IEnumerable<int> DuongSuNhanPhatHanhBanAnIds { get; set; }
        public IEnumerable<string> DuongSuNgayPhatHanh { get; set; }
    }
}
