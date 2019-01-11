using System;
using System.Collections.Generic;
using Biz.Lib.SettingData.Model;
using Biz.Lib.TACM.MauIn.Model;

namespace Biz.TACM.Models.ViewModel.MauIn
{
    public class MauInQuyetDinhGiaHanCBXXViewModel
    {
        public string MaHoSo { get; set; }
        public string SoHoSo { get; set; }
        public string NhomAn { get; set; }
        public string ChanhAn { get; set; }
        public string LoaiChanhAn { get; set; }
        public string SoThuLy { get; set; }
        public DateTime NgayThuLy { get; set; }
        public int Giaidoan { get; set; }
        public string MaToaAn { get; set; }
        public string TenToaAn { get; set; }
        public string SoQuyetDinh { get; set; }
        public DateTime NgayRaQuyetDinh { get; set; }
        public string ThoiHanGiaHan { get; set; }
        public string VienKiemSatTruyTo { get; set; }
        public List<DuongSuModel> DanhSachBiCao { get; set; }
        public string ToiDanh { get; set; }
        public string DieuLuat { get; set; }
        public string ToaAnSoTham { get; set; }
    }
}
