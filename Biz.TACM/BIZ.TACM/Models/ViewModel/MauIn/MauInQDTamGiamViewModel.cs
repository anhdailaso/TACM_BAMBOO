using System;
using System.Collections.Generic;
using Biz.Lib.TACM.MauIn.Model;
using System.Linq;
using System.Web;

namespace Biz.TACM.Models.ViewModel.MauIn
{
    public class MauInQDTamGiamViewModel
    {
        public string MaHoSo { get; set; }
        public string SoHoSo { get; set; }
        public string NhomAn { get; set; }
        public string LoaiChanhAn { get; set; }
        public string ChanhAn { get; set; }
        public string SoThuLy { get; set; }
        public string ThoiHanGiaiQuyet { get; set; }
        public string ThoiHanTamGiamBangChu { get; set; }
        public string SoQuyetDinh { get; set; }
        public int GiaiDoan { get; set; }
        public int MauSo { get; set; }
        public int CongDoan { get; set; }
        public DateTime NgayTamGiam { get; set; }
        public DateTime NgayThuLy { get; set; }
        public DateTime NgayQuyetDinh { get; set; }
        public string MaToaAn { get; set; }
        public string TenToaAn { get; set; }
        public string ThoiHanGiaHan { get; set; }
        public string ToaAnSoTham { get; set; }
        public string VienKiemSatTruyTo { get; set; }
        public List<DuongSuModel> DanhSachBiCao { get; set; }
    }
}