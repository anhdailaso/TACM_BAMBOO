using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Biz.Lib.TACM.MauIn.Model;

namespace Biz.TACM.Models.ViewModel.MauIn
{
    public class MauInQuyetDinhDinhChiViewModel
    {
        public string MaToaAn { get; set; }
        public string TenToaAn { get; set; }
        public string MaHoSo { get; set; }
        public string NhomAn { get; set; }
        public string ThamPhan { get; set; }
        public string BienPhapXuLyHanhChinh { get; set; }
        public string ThuLyAD { get; set; }
        public DateTime NgayNopDonTaiToaAn { get; set; }
        public string SoQuyetDinh { get; set; }
        public DateTime NgayRaQuyetDinh { get; set; }
        public string LyDo { get; set; }
        public string Loai { get; set; }
        public List<DuongSuModel> DanhSachDuongSu { get; set; }
    }
}