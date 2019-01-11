using System;

namespace Biz.Lib.TACM.SauXetXu.Model.SuaChuaBanAn
{
    public class ThongTinSuaChuaBanAnModel
    {
        public int SuaChuaBanAnID { get; set; }
        public string LyDo { get; set; }
        public DateTime NgaySuaChua { get; set; }
        public string NguoiKy { get; set; }
        public string NoiDungSuaChua { get; set; }
        public string DinhKemFile { get; set; }
        public DateTime NgayTao { get; set; }
        public string NguoiTao { get; set; }
    }
}
