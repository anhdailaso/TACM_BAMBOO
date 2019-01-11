using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Biz.TACM.Models.ViewModel.NhanDon
{
    public class NhanHoSoPhucThamViewModel
    {
        public int STT { get; set; }
        public int ChuyenDonID { get; set; }
        public int HoSoVuAnID { get; set; }
        public string MaHoSo { get; set; }
        public string ChuyenDenTuToaAn { get; set; }
        public string NgayChuyenHoSo { get; set; }
        public string NguoiChuyenHoSo { get; set; }
        public DateTime NgayNhanHoSo { get; set; }
        public string NguoiNhanHoSo { get; set; }
        public string GhiChu { get; set; }
        public int CongDoanHoSo { get; set; }
        public int GiaiDoan { get; set; }
        public string TruongHopChuyen { get; set; }
        public string TenCacDuongSu { get; set; }
    }
}