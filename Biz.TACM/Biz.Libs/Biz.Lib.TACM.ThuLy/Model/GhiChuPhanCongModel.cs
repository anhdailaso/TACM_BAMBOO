using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Lib.TACM.ThuLy.Model
{
    public class GhiChuPhanCongModel
    {
        public int GhiChuPhanCongID { get; set; }
        public int HoSoVuAnID { get; set; }
        public string NoiDungGhiChu { get; set; }
        public int GiaiDoan { get; set; }
        public DateTime NgayTao { get; set; }
        public string NguoiTao { get; set; }
    }
}
