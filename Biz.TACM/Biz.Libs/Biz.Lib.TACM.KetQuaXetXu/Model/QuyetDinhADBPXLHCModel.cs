using System;
using System.ComponentModel.DataAnnotations;
using Biz.Lib.SettingData.Model;
using Biz.Lib.TACM.Resources.Resources;

namespace Biz.Lib.TACM.KetQuaXetXu.Model
{
    public class QuyetDinhADBPXLHCModel
    {
        public int QuyetDinhID { get; set; }

        [Required]
        public int HoSoVuAnID { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_SOQUYETDINH")]
        //[Range(0, int.MaxValue, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_SONGUYENDUONG")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string SoQuyetDinh { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_NGAYRA_QUYETDINH")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public DateTime NgayRaQuyetDinh { get; set; }

        //[Display(ResourceType = typeof(ViewText), Name = "LABEL_LYDO_CANCU")]
        //[Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string LyDo { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_KETQUA_GIAIQUYET")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string KetQuaGiaiQuyet { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_THAMPHAN")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string ThamPhan { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_THUKY")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string ThuKy { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_KIEMSATVIEN")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string KiemSatVien { get; set; }

        public int GiaiDoan { get; set; }

        public int CongDoanHoSo { get; set; }

        public int TrangThai { get; set; }

        public string NguoiTao { get; set; }

        public DateTime? NgayTao { get; set; }

        public string GhiChu { get; set; }

        public NhanVienModel NhanVienThamPhan { get; set; }
        public NhanVienModel NhanVienThuKy { get; set; }
        public NhanVienModel NhanVienKiemSatVien { get; set; }
    }
}
