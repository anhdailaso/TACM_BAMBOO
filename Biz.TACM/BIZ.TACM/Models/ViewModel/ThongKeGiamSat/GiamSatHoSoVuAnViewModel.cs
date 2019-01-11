using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Biz.TACM.Models.ViewModel.ThongKeGiamSat
{
    public class GiamSatHoSoVuAnViewModel
    {
        public int HoSoVuAnID { get; set; }
        public int SoHoSo { get; set; }
        public string MaHoSo { get; set; }
        public string ToaAn { get; set; }
        public string NhomAn { get; set; }
        public string GiaiDoan { get; set; }
        public string CongDoanHoSo { get; set; }
        public string DuongSu { get; set; }
    }
}