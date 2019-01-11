using Biz.Lib.TACM.MauIn.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Biz.TACM.Models.ViewModel.MauIn
{
    public class MauInGiayTrieuTapViewModel
    {
        public string TenToaAn { get; set; }
        public string MaHoSo { get; set; }
        public string SoHoSo { get; set; }
        public string LoaiQuanHe { get; set; }
        public string QuanHePhapLuat { get; set; }
        public DateTime ThoiGianMoPhienToa { get; set; }
        public string DiaDiemMoPhienToa { get; set; }
        public string DiaChiToaAn { get; set; }
        public string NguoiKy { get; set; }
        public string ThamPhan { get; set; }
        public DateTime NgayRaThongBao { get; set; }
        public DateTime NgayHienTai { get; set; }
        public List<DuongSuModel> DanhSachDuongSu { get; set; }
        public string TenNguyenDon { get; set; }
        public string TenBiDon { get; set; }
        public int GiaiDoan { get; set; }
        public string SoThuLy { get; set; }
        public DateTime NgayThuLy { get; set; }
        public string TenVuAn { get; set; }
        public string ToiDanh { get; set; }
        public string BienPhapXuLyHanhChinh { get; set; }
        public string ToaAnSoTham { get; set; }
    }
}