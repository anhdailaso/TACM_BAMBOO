using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Lib.TACM.HuyHoSo.Model
{
    public class HoSoVuAnModel
    {
        public int HoSoVuAnID { get; set; }
        public string MaHoSo { get; set; }
        public int SoHoSo { get; set; }
        public string TenVuAn { get; set; }
        public string NhomAn { get; set; }
        public string ToaAn { get; set; }
        public string GiaiDoan { get; set; }
        public int CongDoan { get; set; }
        public string LyDoHuy { get; set; }
        public string NguoiHuy { get; set; }
        public DateTime NgayHuy { get; set; }
        public string NgayHuyHoSo { get; set; }
    }
}
