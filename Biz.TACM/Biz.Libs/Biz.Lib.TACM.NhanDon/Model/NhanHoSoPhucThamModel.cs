using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Lib.TACM.NhanDon.Model
{
    public class NhanHoSoPhucThamModel
    {
        public int STT { get; set; }
        public int ChuyenDonID { get; set; }
        public int HoSoVuAnID { get; set; }
        public string MaHoSo { get; set; }
        public string ToaAnChuyenDi { get; set; }
        public DateTime NgayChuyenDon { get; set; }
        public string NguoiTao { get; set; }
        public DateTime NgayNhanHoSo { get; set; }
        public string NguoiNhanHoSo { get; set; }
        public string GhiChu { get; set; }
        public int CongDoanHoSo { get; set; }
        public int GiaiDoan { get; set; }
        public string TenVuAn { get; set; }
        public string NhomAn { get; set; }
    }
}