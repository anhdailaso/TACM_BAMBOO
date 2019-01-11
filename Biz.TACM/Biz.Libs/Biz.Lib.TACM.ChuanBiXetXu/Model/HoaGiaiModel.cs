using Biz.Lib.TACM.Resources.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Lib.TACM.ChuanBiXetXu.Model
{
    public class HoaGiaiModel
    {
        public int HoaGiaiID { get; set; }

        [Required]
        public int HoSoVuAnID { get; set; }
        [Display(ResourceType = typeof(ViewText), Name = "LABEL_NGAYMO_PHIENHOP")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages),ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public DateTime NgayMoPhienHop { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_NOIDUNG_PHIENHOP")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string NoiDungPhienHop { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_THAMPHAN")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string ThamPhan { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_THUKY")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string ThuKy { get; set; }

        public int GiaiDoan { get; set; }

        public int CongDoanHoSo { get; set; }

        public int TrangThai { get; set; }

        public string NguoiTao { get; set; }

        public DateTime? NgayTao { get; set; }

        public string GhiChu { get; set; }

        public string TenThamPhan { get; set; }

        public string TenThuKy { get; set; }
    }
}
