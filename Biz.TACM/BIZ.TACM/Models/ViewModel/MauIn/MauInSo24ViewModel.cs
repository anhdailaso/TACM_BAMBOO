using System;
using System.Collections.Generic;
using Biz.Lib.TACM.MauIn.Model;

namespace Biz.TACM.Models.ViewModel.MauIn
{
    public class MauInSo24ViewModel
    {
        public string MaToaAn { get; set; }
        public string TenToaAn { get; set; }
        public string MaHoSo { get; set; }
        public string SoHoSo { get; set; }
        public DateTime NgayLamDon { get; set; }
        public DateTime NgayNopDonTaiToaAn { get; set; }
        public string NoiDungDon { get; set; }
        public string HoTenNguoiKyXacNhanDaNhanDon { get; set; }
        public string ChucDanhNguoiKyXacNhan { get; set; }
        public string ChucVuNguoiKyXacNhan { get; set; }
        public string LoaiQuanHe { get; set; }
        public string HinhThucGoiDon { get; set; }
        public List<DuongSuModel> DanhSachDuongSuViewModel { get; set; }
        public string TieuDeGiayXacNhan { get; set; }
        public string LoaiDon { get; set; }
        public string NguoiNopDon { get; set; }
    }  
}