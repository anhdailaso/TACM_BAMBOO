using System;
using System.Collections.Generic;
using Biz.Lib.SettingData.Model;
using Biz.Lib.TACM.MauIn.Model;

namespace Biz.TACM.Models.ViewModel.MauIn
{
    public class MauInQuyetDinhTamHoanViewModel
    {
        public string MaHoSo { get; set; }
        public string SoHoSo { get; set; }
        public string NhomAn { get; set; }
        public string ThamPhan { get; set; }
        public string GioiTinhThamPhan { get; set; }
        public string ThamPhan1 { get; set; }
        public string GioiTinhThamPhan1 { get; set; }
        public string ThamPhan2 { get; set; }
        public string GioiTinhThamPhan2 { get; set; }
        public string ThamPhanKhac { get; set; }
        public string GioiTinhThamPhanKhac { get; set; }
        public string KiemSatVien { get; set; }
        public string GioiTinhKiemSatVien { get; set; }
        public string HoiThamNhanDan { get; set; }
        public string GioiTinhHoiThamNhanDan { get; set; }
        public string HoiThamNhanDan2 { get; set; }
        public string GioiTinhHoiThamNhanDan2 { get; set; }
        public string HoiThamNhanDan3 { get; set; }
        public string GioiTinhHoiThamNhanDan3 { get; set; }
        public string ThuKy { get; set; }
        public string GioiTinhThuKy { get; set; }
        public string ChanhAn { get; set; }
        public string LoaiChanhAn { get; set; }
        public string SoThuLy { get; set; }
        public DateTime NgayThuLy { get; set; }
        public int GiaiDoan { get; set; }
        public string MaToaAn { get; set; }
        public string TenToaAn { get; set; }
        public string SoQuyetDinh { get; set; }
        public DateTime NgayRaQuyetDinh { get; set; }
        public string LyDo { get; set; }
        public string ToaAnSoTham { get; set; }
        public int HoiDong { get; set; }
        public string ThoiHanGiaHan { get; set; }
        public int LanThu { get; set; }
        public DateTime ThoiGianMoPhienToa { get; set; }
        public string DiaDiemMoPhienToa { get; set; }
        public string DiaChiToaAn { get; set; }
        public List<DuongSuModel> DanhSachBiCao { get; set; }
        public string ToiDanh { get; set; }
        public string DieuLuat { get; set; }
    }
}
