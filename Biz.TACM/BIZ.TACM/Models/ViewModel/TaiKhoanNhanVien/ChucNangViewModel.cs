using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Biz.Lib.TACM.Resources.Resources;
using System.ComponentModel.DataAnnotations;

namespace Biz.TACM.Models.ViewModel.TaiKhoanNhanVien
{

    public class ChucNangViewModel
    {
        public List<ChucNang> ListChucNang { get; set; }
        
        public string ChucNangChinh { get; set; }
        
    }
    public class ChucNang
    {
        public int NhanVienID { get; set; }
        public string MaChucNang { get; set; }
        public string TenChucNang { get; set; }
        public bool IsHaveChucNang { get; set; }
        public string GhiChu { get; set; }
    }
}