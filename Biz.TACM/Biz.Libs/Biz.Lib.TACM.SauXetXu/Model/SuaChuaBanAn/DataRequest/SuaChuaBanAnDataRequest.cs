using System;

namespace Biz.Lib.TACM.SauXetXu.Model.SuaChuaBanAn.DataRequest
{
    public class SuaChuaBanAnDataRequest : DataRequestBase
    {
        public DateTime NgaySuaChua { get; set; }
        public string LyDo { get; set; }
        public string NguoiKy { get; set; }
        public string NoiDungSuaChua { get; set; }
        public string DinhKemFile { get; set; }
    }
}
