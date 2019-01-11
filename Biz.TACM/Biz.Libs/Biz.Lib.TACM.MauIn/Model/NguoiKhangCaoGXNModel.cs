using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Lib.TACM.MauIn.Model
{
    public class NguoiKhangCaoGXNModel : DuongSuModel
    {

        public DateTime NgayDe { get; set; }
        public DateTime NgayNop { get; set; }
        public string NoiDungKhangCao { get; set; }
        public string NguoiLapBienBan { get; set; }
        public string ChucVu { get; set; }
        public string ChucDanh { get; set; }
        public string GioiTinhNguoiLapBB { get; set; }
        public DateTime NgayTao { get; set; }
        public int KhangCaoID { get; set; }
    }
}