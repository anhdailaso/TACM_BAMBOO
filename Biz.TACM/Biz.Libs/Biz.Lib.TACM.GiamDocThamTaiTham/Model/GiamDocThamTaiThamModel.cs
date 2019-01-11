using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Lib.TACM.GiamDocThamTaiTham.Model
{
    public class GiamDocThamTaiThamModel
    {
        public int So { get; set; }
        public string MaHoSo { get; set; }
        public int HoSoVuAnID { get; set; }
        public string Nhom { get; set; }
        public string NhomAn { get; set; }
        public string SoQuyetDinh { get; set; }
        public DateTime? NgayRaQuyetDinh { get; set; }
        public int GiamDocThamTaiThamID { get; set; }
        public string NoiDungQuyetDinh { get; set; }
        public string NoiDungHuySuaAn { get; set; }
        public string GhiChu { get; set; }
        public bool HuySuaSoTham { get; set; }
        public bool HuySuaPhucTham { get; set; }
        public bool BanAnSoTham { get; set; }
        public bool BanAnPhucTham { get; set; }
        public bool QuyetDinhSoTham { get; set; }
        public bool QuyetDinhPhucTham { get; set; }
        public string NguoiTao { get; set; }
    }
}
