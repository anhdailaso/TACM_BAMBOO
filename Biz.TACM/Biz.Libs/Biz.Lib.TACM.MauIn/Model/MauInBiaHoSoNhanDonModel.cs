using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Lib.TACM.MauIn.Model
{
    public class MauInBiaHoSoNhanDonModel
    {
        public string TenToaAn { get; set; }
        public string MaHoSo { get; set; }
        public string SoHoSo { get; set; }
        public string NhomAn { get; set; }
        public string LoaiQuanHe { get; set; }
        public string QuanHePhapLuat { get; set; }
        public string ThamPhanChuToa { get; set; }
        public string GioiTinhThamPhanChuToa { get; set; }
        public DateTime NgayNhanDon { get; set; }
        public string NoiDungDon { get; set; }
    }
}
