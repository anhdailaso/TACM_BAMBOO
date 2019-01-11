using Biz.Lib.TACM.Resources.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Lib.TACM.NhanDon.Model
{
    public class TraLaiDonModel
    {
        public int TraLaiDonID { get; set; }

        public int HoSoVuAnID { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_NGAYTRADON")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages),ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public DateTime NgayTraDon { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_LYDO_TRADON")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string LyDoTraDon { get; set; }

        public string NhomNghiepVu { get; set; }

        public int GiaiDoan { get; set; }

        public int CongDoanHoSo { get; set; }

        public int TrangThai { get; set; }

        public string NguoiTao { get; set; }

        public DateTime? NgayTao { get; set; }

        public string GhiChu { get; set; }
    }
}
