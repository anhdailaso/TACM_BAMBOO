using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Biz.TACM.Models.ViewModel.HoSo
{
    public class TruyVanHoSoViewModel
    {
        public string DieuKienLoc { get; set; }
        public DateTime TuNgay { get; set; }
        public DateTime ToiNgay { get; set; }
        public string TrangThai { get; set; }
        public string NguoiNhanDon { get; set; }
        public string DuongSu { get; set; }
        public string LoaiDon { get; set; }
        public string LoaiQuanHe { get; set; }
        public bool CoYeuToNuocNgoai { get; set; }
    }
}