using Biz.Lib.TACM.Resources.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Lib.TACM.NhanDon.Model
{
    public class KhieuNaiTraDonModel
    {
        public int KhieuNaiViecTraDonID { get; set; }

        public int HoSoVuAnID { get; set; }

        [Display(Name = "Nhóm")]
        public string Nhom { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_LANTHU")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages),ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public int LanThu { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_NGUOI_KHIEUNAI")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string NguoiKhieuNai { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_NGAY_KHIEUNAI")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public DateTime NgayKhieuNai { get; set; }
        
        [Display(Name = "Nội dung khiếu nại")]
        public string NoiDungKhieuNai { get; set; }
        
        [Display(Name = "Kết quả giải quyết")]
        public string KetQuaGiaiQuyet { get; set; }        

        public string NhomNghiepVu { get; set; }

        public int GiaiDoan { get; set; }

        public int CongDoanHoSo { get; set; }

        public int TrangThai { get; set; }

        public string NguoiTao { get; set; }

        public DateTime? NgayTao { get; set; }

        public string GhiChu { get; set; }
    }

    public class KienNghiTraDonModel
    {
        public int KienNghiViecTraDonID { get; set; }

        public int HoSoVuAnID { get; set; }

        [Display(Name = "Nhóm")]
        public string Nhom { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_LANTHU")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public int LanThu { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_VIENKIEMSAT_KIENNGHI")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string VKSKienNghi { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_NGAYKIENNGHI")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public DateTime? NgayKienNghi { get; set; }

        [Display(Name = "Nội dung khiếu nại")]
        public string NoiDungKienNghi { get; set; }

        [Display(Name = "Kết quả giải quyết")]
        public string KetQuaGiaiQuyet { get; set; }

        public string NhomNghiepVu { get; set; }

        public int GiaiDoan { get; set; }

        public int CongDoanHoSo { get; set; }

        public int TrangThai { get; set; }

        public string NguoiTao { get; set; }

        public DateTime? NgayTao { get; set; }

        public string GhiChu { get; set; }
    }
}
