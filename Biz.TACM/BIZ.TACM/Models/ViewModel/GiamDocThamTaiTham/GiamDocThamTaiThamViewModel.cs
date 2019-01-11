using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Biz.Lib.TACM.Resources.Resources;
using System.ComponentModel.DataAnnotations;
namespace Biz.TACM.Models.ViewModel.GiamDocThamTaiTham
{
    public class GiamDocThamTaiThamViewModel
    {
        public int So { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_MAHOSO")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages),
        ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string MaHoSo { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_NHOMAN")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages),
        ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string NhomAn { get; set; }

        public string Nhom { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_SOQUYETDINH")]
        [StringLength(50, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_SOLUONGKYTUTOIDA")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages),
        ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string SoQuyetDinh { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_NGAYRA_QUYETDINH")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages),
        ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string NgayRaQuyetDinh { get; set; }
        public int GiamDocThamTaiThamID { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_NOIDUNG_QUYETDINH")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string NoiDungQuyetDinh { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_NOIDUNGHUYSUAAN")]
        [StringLength(100, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_SOLUONGKYTUTOIDA")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages),
        ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string NoiDungHuySuaAn { get; set; }
        public string GhiChu { get; set; }
        public int HoSoVuAnID { get; set; }
        public bool HuySuaSoTham { get; set; }
        public bool HuySuaPhucTham { get; set; }
        public bool BanAnSoTham { get; set; }
        public bool BanAnPhucTham { get; set; }
        public bool QuyetDinhSoTham { get; set; }
        public bool QuyetDinhPhucTham { get; set; }
        public string NguoiTao { get; set; }
        public HoSoVuAnViewModel Hoso { get; set; }
    }
}