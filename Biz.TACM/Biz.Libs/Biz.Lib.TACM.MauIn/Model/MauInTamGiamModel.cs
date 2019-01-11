using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Lib.TACM.MauIn.Model
{
    public class MauInTamGiamModel
    {
        public string MaHoSo { get; set; }
        public string SoHoSo { get; set; }
        public string NhomAn { get; set; }
        public string LoaiChanhAn { get; set; }
        public string ChanhAn { get; set; }
        public string SoThuLy { get; set; }
        public string ThoiHanGiaiQuyet { get; set; }
        public string ThoiHanGiaiQuyetBangChu { get; set; }
        public string ThoiHanGiaHan { get; set; }
        public string SoQuyetDinh { get; set; }
        public int MauSo { get; set; }
        public int CongDoan { get; set; }
        public DateTime NgayThuLy { get; set; }
        public DateTime NgayQuyetDinh { get; set; }
        public string MaToaAn { get; set; }
        public string TenToaAn { get; set; }
        public string ToaAnSoTham { get; set; }
        public string VienKiemSatTruyTo { get; set; }
    }
}
