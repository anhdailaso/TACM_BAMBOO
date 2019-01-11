using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Lib.TACM.MauIn.Model
{
    public class MauInSo47PTModel:MauInSo47Model
    {
        public string VienKiemSatKhangNghi { get; set; }
        public string ThamPhan1 { get; set; }
        public string GioiTinhThamPhan1 { get; set; }
        public string ThamPhan2 { get; set; }
        public string GioiTinhThamPhan2 { get; set; }
        public string SoQuyetDinh { get; set; }
        public DateTime NgayRaQuyetDinh { get; set; }
        public string ToaAnSoTham { get; set; }
    }
}
