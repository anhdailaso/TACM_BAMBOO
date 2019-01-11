using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Biz.Lib.SettingData.Model;

namespace Biz.TACM.Models.ViewModel.GiamDocThamTaiTham
{
    public class HoSoVuAnViewModel
    {

        public string MaHoSo { get; set; }
        public string NoiDungBanAnST { get; set; }
        public string NoiDungBanAnPT { get; set; }
        public string NoiDungQuyetDinhST { get; set; }
        public string NoiDungQuyetDinhPT { get; set; }
        public int HoiDong { get; set; }
        public List<NhanVienModel> HoiDongSoTham { get; set; }
        public List<NhanVienModel> HoiDongPhucTham { get; set; }
        public int GiaiDoan { get; set; }
    }
}