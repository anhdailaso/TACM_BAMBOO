using System;

namespace Biz.Lib.TACM.SauXetXu.Model.LuuKho.DataRequest
{
    public class LuuKhoDataRequest : DataRequestBase
    {
        public DateTime NgayLuu { get; set; }
        public string GhiChuTinhTrangHoSo { get; set; }
        public string NguoiGiao { get; set; }
        public string NguoiNhanLuu { get; set; }
    }
}
