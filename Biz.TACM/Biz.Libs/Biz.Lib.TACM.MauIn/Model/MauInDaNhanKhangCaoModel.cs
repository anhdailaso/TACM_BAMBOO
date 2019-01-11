using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Lib.TACM.MauIn.Model
{
    public class MauInDaNhanKhangCaoModel
    {
        public string TenToaAn { get; set; }
        public string MaHoSo { get; set; }
        public string SoHoSo { get; set; }
        public string NhomAn { get; set; }
        public string SoBanAn { get; set; }
        public string SoQuyetDinh { get; set; }
        public DateTime NgayRaBanAn { get; set; }
        public DateTime NgayRaQuyetDinh { get; set; }
        public string ThamPhan { get; set; }
        public string HinhThucGoiDon { get; set; }
        public string ToaAnSoTham { get; set; }
        public string LoaiQuanHe { get; set; }
    }
}
