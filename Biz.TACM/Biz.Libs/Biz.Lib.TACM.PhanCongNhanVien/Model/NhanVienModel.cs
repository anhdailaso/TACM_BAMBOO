using System;
using System.Collections.Generic;

namespace Biz.Lib.TACM.PhanCongNhanVien.Model
{
    public class NhanVienModel
    {
        public int NhanvienID { get; set; }


        public string HoNV { get; set; }

        public string TenNV { get; set; }


        public string TenLotNV { get; set; }

        public string ChucDanh { get; set; }

        public string HoVaTen
        {
            get { return HoNV + (string.IsNullOrEmpty(TenLotNV) ? "" : " " + TenLotNV) + " " + TenNV; }
            set {; }
        }
    }
}