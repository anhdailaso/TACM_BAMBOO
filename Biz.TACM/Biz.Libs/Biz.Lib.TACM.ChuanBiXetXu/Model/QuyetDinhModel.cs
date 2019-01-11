using Biz.Lib.TACM.Resources.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Lib.TACM.ChuanBiXetXu.Model
{
    public class QuyetDinhModel
    {
        public int QuyetDinhID { get; set; }

        [Required]
        public int HoSoVuAnID { get; set; }

        public int QuyetDinhLoai { get; set; }

        //[Display(ResourceType = typeof(ViewText), Name = "LABEL_LOAIQUYETDINH")]
        //[Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        //public string LoaiQuyetDinh { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_TENQUYETDINH")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string TenQuyetDinh { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_NGAYRA_QUYETDINH")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public DateTime NgayRaQuyetDinh { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_THOIHAN_GIAHAN")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string ThoiHanGiaHan { get; set; }

        public int GiaiDoan { get; set; }

        public int CongDoanHoSo { get; set; }

        public int TrangThai { get; set; }

        public string NguoiTao { get; set; }

        public DateTime? NgayTao { get; set; }

        public string GhiChu { get; set; }
    }
}
