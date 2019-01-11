using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Biz.TACM.Models.Model
{
    public class NhanVienSessionModel
    {
        public int NhanvienID { get; set; }
        public string MaNV { get; set; }
        public string MaNVMoi { get; set; }
        public string HoNV { get; set; }
        public string TenNV { get; set; }
        public string TenLotNV { get; set; }
        public string DuongDanHinhDaiDien { get; set; }
        public string HoVaTen
        {
            get { return HoNV + (string.IsNullOrEmpty(TenLotNV) ? "" : " " + TenLotNV) + " " + TenNV; }
            set {; }
        }
    }
}