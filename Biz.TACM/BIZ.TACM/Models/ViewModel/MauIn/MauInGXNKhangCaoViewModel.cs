using System;
using System.Collections.Generic;
using Biz.Lib.TACM.MauIn.Model;
using System.Linq;
using System.Web;

namespace Biz.TACM.Models.ViewModel.MauIn
{
    public class MauInGXNKhangCaoViewModel
    {
        public string TenToaAn { get; set; }
        public string MaHoSo { get; set; }
        public string SoHoSo { get; set; }
        public string SoBanAn { get; set; }
        public string SoQuyetDinh { get; set; }
        public DateTime NgayRaBanAn { get; set; }
        public DateTime NgayRaQuyetDinh { get; set; }
        public string ThamPhan { get; set; }
        public int GiaiDoan { get; set; }
        public string NhomAn { get; set; }
        public string LoaiQuanHe { get; set; }
        public string HinhThucGoiDon { get; set; }
        public string ToaAnSoTham { get; set; }
        public IEnumerable<NguoiKhangCaoGXNModel> NguoiKhangCao { get; set; }
        public List<DuongSuModel> DanhSachDuongSu { get; set; }
        
    }
}