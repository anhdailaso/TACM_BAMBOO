using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Biz.TACM.Models.ViewModel.NhanDon
{
    public class NoiDungDonViewModel
    {
        public int NoiDungDonID { get; set; }
        public int HoSoVuAnID { get; set; }
        public string NoiDungDon { get; set; }
        public string NhomNghiepVu { get; set; }
        public int GiaiDoan { get; set; }
        public int CongDoanHoSo { get; set; }
        public int TrangThai { get; set; }
        public string NguoiTao { get; set; }
        public string NgayTao { get; set; }
        public string GhiChu { get; set; }
    }
}