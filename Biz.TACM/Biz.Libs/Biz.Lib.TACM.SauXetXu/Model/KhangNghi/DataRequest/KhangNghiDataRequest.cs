using System;

namespace Biz.Lib.TACM.SauXetXu.Model.KhangNghi.DataRequest
{
    public class KhangNghiDataRequest : DataRequestBase
    {
        public DateTime NgayToaAnNhanVanBan { get; set; }
        public string VienKiemSatKhangNghi { get; set; }
        public string VanBanKhangNghi { get; set; }
        public string NguoiBiKhangNghi { get; set; }
        public string NoiDungKhangNghi { get; set; }
        public string HinhThuc { get; set; }
    }
}
