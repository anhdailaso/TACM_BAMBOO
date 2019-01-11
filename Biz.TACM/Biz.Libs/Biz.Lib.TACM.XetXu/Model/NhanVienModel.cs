//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Biz.Lib.TACM.XetXu.Model
//{
//    public class NhanVienModel
//    {
//        public int NhanVienID { get; set; }
        
//        public string MaNV { get; set; }
        
//        public string HoNV { get; set; }
        
//        public string TenNV { get; set; }
        
//        public string TenLotNV { get; set; }

//        public bool Checked { get; set; }

//        public string HoVaTenNV
//        {
//            get
//            {
//                return HoNV + (string.IsNullOrEmpty(TenLotNV) ? "" : " " + TenLotNV) + " " + TenNV;
//            }            
//        }

//        public string HoTenVaMaNV
//        {
//            get
//            {
//                return HoVaTenNV + (!string.IsNullOrEmpty(MaNV) ? " (" + MaNV + ")" : "");
//            }
//        }
//    }
//}
