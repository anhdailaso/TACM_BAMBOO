using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Lib.TACM.KetQuaXetXu.Model
{
    public class LoaiQuyetDinhModel
    {
        public int DuLieuLoaiQuyetDinhID { get; set; }

        public string TenLoaiQuyetDinh { get; set; }

        public int TrangThai { get; set; }

        public string NguoiTao { get; set; }

        public DateTime? NgayTao { get; set; }

        public string GhiChu { get; set; }
    }
}
