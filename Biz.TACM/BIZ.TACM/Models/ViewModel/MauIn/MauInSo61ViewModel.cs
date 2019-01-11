using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Biz.Lib.TACM.MauIn.Model;

namespace Biz.TACM.Models.ViewModel.MauIn
{
    public class MauInSo61ViewModel
    {
        public string MaToaAn { get; set; }
        public string MaHoSo { get; set; }
        public string TenToaAn { get; set; }
        public string SoHoSo { get; set; }
        public DateTime NgayRaThongBao { get; set; }
        public string CoQuanThiHanhAnThu { get; set; }
        public string DiaChiCoQuanThiHanhAnThu { get; set; }
        public string ThamPhan { get; set; }
        public string LoaiQuanHe { get; set; }
        public string GiaTriDuNopFormatted { get; set; }
        public string GiaTriDuNopBangChu { get; set; }
        public List<NguoiKhangCaoMauInSo61Model> DanhSachNguoiKhangCao { get; set; }

    }
}