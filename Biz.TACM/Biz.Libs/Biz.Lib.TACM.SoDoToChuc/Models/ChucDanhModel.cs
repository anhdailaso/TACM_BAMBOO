using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Lib.TACM.SoDoToChuc.Models
{
    public class ChucDanhModel
    {
        public int SoDoToChucID { get; set; }
        public string ChucDanh { get; set; }
        public int? ChucDanhChaID { get; set; }
        public string ChucDanhCha { get; set; }
        public int ToaAnID { get; set; }
        public int Loai { get; set; }
        public string DienGiaiNghiepVu { get; set; }
        public int TrangThai { get; set; }
        public string NguoiTao { get; set; }
        public DateTime NgayTao { get; set; }
        public string GhiChu { get; set; }
    }
}
