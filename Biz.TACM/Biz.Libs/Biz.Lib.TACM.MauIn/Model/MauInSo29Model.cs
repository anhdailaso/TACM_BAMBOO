using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Lib.TACM.MauIn.Model
{
    public class MauInSo29Model
    {
        public string MaToaAn { get; set; }
        public string MaHoSo { get; set; }
        public string SoHoSo { get; set; }
        public string TenToaAn { get; set; }
        public DateTime NgayRaThongBao { get; set; }
        public string CoQuanThiHanhAnThu { get; set; }
        public string DiaChiCoQuanThiHanhAnThu { get; set; }
        public decimal? GiaTriDuNop { get; set; }
        public int NguoiDuNop { get; set; }
        public string ThamPhan { get; set; }
        public string LoaiQuanHe { get; set; }
    }
}