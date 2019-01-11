using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Lib.TACM.QuanLyNhanVien.Model
{
    public class ChucNangModel
    {
        public int NhanVienID { get; set; }
        public string MaChucNang { get; set; }
        public string TenChucNang { get; set; }
        public int? ChucNangChinh { get; set; }
        public string NguoiTao { get; set; }
        public string GhiChu { get; set; }
        public int TrangThai { get; set; }
    }
}
