using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Lib.TACM.ChuanBiXetXu.Model
{
    public class NhanVienModel
    {
        public int NhanVienID { get; set; }

        public string MaNV { get; set; }

        [Required]
        public string HoNV { get; set; }

        [Required]
        public string TenNV { get; set; }

        [Required]
        public string TenLotNV { get; set; }

        public bool Checked { get; set; }

        public string HoVaTenNV
        {
            get
            {
                return HoNV + (string.IsNullOrEmpty(TenLotNV) ? "" : " " + TenLotNV) + " " + TenNV;
            }
        }
    }
}
