using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Biz.Lib.SettingData.Model;
using Biz.Lib.TACM.Resources.Resources;

namespace Biz.TACM.Models.ViewModel.NhanDon
{
    public class ThamPhanViewModel
    {
        public int ThamPhanID { get; set; }

        public int HoSoVuAnID { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_THAMPHAN")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages),ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]

        public string ThamPhan { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_NGAYPHANCONG")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string NgayPhanCong { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_TENNGUOI_PHANCONG")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string TenNguoiPhanCong { get; set; }

        public string HoiThamNhanDan2 { get; set; }

        public string NhomNghiepVu { get; set; }

        public int GiaiDoan { get; set; }

        public int CongDoanHoSo { get; set; }

        public int TrangThai { get; set; }

        public string NguoiTao { get; set; }

        public string NgayTao { get; set; }

        public string GhiChu { get; set; }

        public NhanVienModel NhanVienThamPhan { get; set; }
        public NhanVienModel NhanVienPhanCong { get; set; }
    }
}