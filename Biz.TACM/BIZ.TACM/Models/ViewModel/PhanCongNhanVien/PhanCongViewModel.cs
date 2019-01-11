using System.Collections.Generic;
namespace Biz.TACM.Models.ViewModel.PhanCongNhanVien
{
    public class PhanCongViewModel
    {
        public int NhomAnID { get; set; }
        public List<NhanVienNhomAnViewModel> listnv { get; set; }
    }
}
