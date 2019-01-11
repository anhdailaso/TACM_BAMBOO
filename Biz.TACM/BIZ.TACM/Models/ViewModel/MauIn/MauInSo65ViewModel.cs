using System;
using System.Collections.Generic;
using Biz.Lib.TACM.MauIn.Model;
using System.Linq;
using System.Web;

namespace Biz.TACM.Models.ViewModel.MauIn
{
    public class MauInSo65ViewModel
    {
        public string MaHoSo { get; set; }
        public string SoHoSo { get; set; }
        public string SoThuLy { get; set; }
        public string TenToaAn { get; set; }
        public DateTime NgayThuLy { get; set; }
        public string VienKiemSatKhangNghi { get; set; }
        public string SoBanAn { get; set; }
        public string SoQuyetDinh { get; set; }
        public DateTime NgayTuyenAn { get; set; }
        public DateTime NgayRaQuyetDinh { get; set; }
        public string ToaAnSoTham { get; set; }
        public string ThamPhan { get; set; }
        public string LoaiQuanHe { get; set; }
        public string QuanHePhapLuat { get; set; }
        public string NoiDungKhangNghi { get; set; }
        public IEnumerable<DuongSuModel> DanhSachDuongSu { get; set; }
        public IEnumerable<NguoiKhangCaoMauInSo65Model> NguoiKhangCao { get; set; }
        
    }
}