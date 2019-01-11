using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Biz.TACM.Models.Model
{
    public class AnSessionModel
    {
        public int ToaAnId { get; set; }
        public string MaToaAn { get; set; }
        public string TenToaAn { get; set; } //Toa an tinh Ca Mau

        public int NhomAnId { get; set; }
        public string MaNhomAn { get; set; }
        public string TenNhomAn { get; set; } //Dan Su

        public int GiaiDoanId { get; set; }
        public string MaGiaiDoan { get; set; }
        public string TenGiaiDoan { get; set; } //So Tham

        public int CongDoanId { get; set; }
        public string MaCongDoan { get; set; }
        public string TenCongDoan { get; set; } //Nhan Don

        public int VuAnId { get; set; }
        public string MaVuAn { get; set; }
        public string SoHoSo { get; set; }
    }
}