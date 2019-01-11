using System.Collections.Generic;
namespace Biz.TACM.Models.ViewModel.QuanLyNhanVien
{
    public class NhanVienToaAnViewModel
    {
        public int NhanvienID { get; set; }
        public int ToaAnID { get; set; }
        public List<ToaAnNhanVien> ListToaAn { get; set; } 
    }
    public class ToaAnNhanVien
    {
        public int ToaAnID { get; set; }
        public string TenToaAn { get; set; }
        public bool isckeck { get; set; }
    }
}