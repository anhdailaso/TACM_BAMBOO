using System;
using System.Collections.Generic;
using Biz.Lib.TACM.MauIn.Model;

namespace Biz.TACM.Models.ViewModel.MauIn
{
    public class MauInSo30ViewModel
    {
        public string MaToaAn { get; set; }
        public string TenToaAn { get; set; }
        public string MaHoSo { get; set; }
        public string NhomAn { get; set; }
        public string ThamPhan { get; set; }
        public string SoThuLy { get; set; }
        public string QuanHePhapLuat { get; set; }
        public string NoiDungYeuCau { get; set; }
        public string TaiLieuChungTuKemTheo { get; set; }
        public DateTime NgayThuLy { get; set; }
        public List<DuongSuModel> DanhSachDuongSuViewModel { get; set; }
        public string LoaiQuanHe { get; set; }
        public string ThuLyTheoThuTuc { get; set; }
        public string TenCoQuanDeNghi { get; set; }
        public string BienPhapXuLyHanhChinh { get; set; }
        public string ThuLyAD { get; set; }
        public DateTime NgayNopDonTaiToaAn { get; set; }
    }
}