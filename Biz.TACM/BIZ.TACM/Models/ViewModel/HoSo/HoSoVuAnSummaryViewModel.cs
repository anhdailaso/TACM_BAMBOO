using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Biz.TACM.Models.ViewModel.HoSo
{
    public class HoSoVuAnSummaryViewModel
    {
        public int SoTT { get; set; }
        public string MaHoSo { get; set; }
        public string DuongSu { get; set; }
        public string ThamPhan { get; set; }
        public DateTime NgayNhan { get; set; }
        public int TrangThai { get; set; }
    }
}