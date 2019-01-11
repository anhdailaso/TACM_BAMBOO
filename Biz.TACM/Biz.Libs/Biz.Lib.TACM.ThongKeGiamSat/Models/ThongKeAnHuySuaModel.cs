
using System.Collections.Generic;

namespace Biz.Lib.TACM.ThongKeGiamSat.Models
{
    public class ThongKeAnHuySuaModel
    {
        public List<DuLieuBieuDoHuySuaModel> ListData { get; set; }
        public string ListHoSoHuyID { get; set; }
        public string ListHoSoSuaID { get; set; }
    }

    public class DuLieuBieuDoHuySuaModel
    {
        public string NhomAn { get; set; }
        public string SoAnHuy { get; set; }
        public string SoAnSua { get; set; }
        public string TongHuySua { get; set; }
        public string ListHoSoID { get; set; }
    }
}
