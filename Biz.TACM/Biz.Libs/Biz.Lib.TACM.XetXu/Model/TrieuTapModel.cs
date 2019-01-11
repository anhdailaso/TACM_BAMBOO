using Biz.Lib.TACM.Resources.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biz.Lib.SettingData.Model;

namespace Biz.Lib.TACM.XetXu.Model
{
    public class TrieuTapModel
    {
        public int TrieuTapID { get; set; }

        [Required]
        public int HoSoVuAnID { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_THOIGIAN")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public DateTime ThoiGianMoPhienToa { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_DIADIEM")]
        [StringLength(250, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_SOLUONGKYTUTOIDA")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages),ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string DiaDiemMoPhienToa { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_LANTHU")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public int LanThu { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_NGUOIKY")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string NguoiKy { get; set; }

        public int GiaiDoan { get; set; }

        public int CongDoanHoSo { get; set; }

        public int TrangThai { get; set; }

        public string NguoiTao { get; set; }

        public DateTime? NgayTao { get; set; }

        public string GhiChu { get; set; }

        public List<String> DuongSuID { get; set; }

        public NhanVienModel NhanVienNguoiKy { get; set; }
    }
}
