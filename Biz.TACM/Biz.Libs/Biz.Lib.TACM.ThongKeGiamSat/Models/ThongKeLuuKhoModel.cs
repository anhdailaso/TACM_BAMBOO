
using System.Collections.Generic;

namespace Biz.Lib.TACM.ThongKeGiamSat.Models
{
    public class ThongKeLuuKhoModel
    {
        public List<DuLieuBieuDoModel> ListData { get; set; }

        public string ListHoSoVuAnID { get; set; }
    }

    public class DuLieuBieuDoModel
    {
        public string TenNhom { get; set; }
        public int SoLuongHoSo { get; set; }
    }  
}
