
using Biz.Lib.SettingData.Model;
using Biz.Lib.TACM.Resources.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Lib.TACM.KetQuaXetXu.Model
{
    public class BanAnModel
    {
        public int KQXXBanAnID { get; set; }

        [Required]
        public int HoSoVuAnID { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_SO")]
        //[Range(0, int.MaxValue, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_SONGUYENDUONG")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages),ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string SoBanAn { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_NGAYRABANAN")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public DateTime? NgayThangNamBanAn { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_NGAYMO_PHIENTOA_PHIENHOP")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public DateTime NgayMoPhienToa { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_NGAY_TUYENAN")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public DateTime NgayTuyenAn { get; set; }

        public DateTime? HieuLuc { get; set; }

        //[Required(ErrorMessage = "Loại quan hệ không được để trống.")]
        //[Display(Name = "Loại quan hệ")]
        //public string LoaiQuanHe { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_QUANHE_PHAPLUAT")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string QuanHePhapLuat { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_KETQUA_XETXU")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string KetQuaXetXu { get; set; }

        //[Required(ErrorMessage = "Bạn chưa chọn Xét xử")]
        //[Display(Name = "Xét xử")]
        //public string XetXu { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_THAMPHAN_CHUTOA")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string ThamPhan { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_THAMPHAN1")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string ThamPhan1 { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_THAMPHAN2")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string ThamPhan2 { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_THAMPHAN_KHAC")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string ThamPhanKhac { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_HOITHAMNHANDAN_1")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string HoiThamNhanDan { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_HOITHAMNHANDAN_2")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string HoiThamNhanDan2 { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_HOITHAMNHANDAN_3")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string HoiThamNhanDan3 { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_THUKY")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string ThuKy { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_KIEMSATVIEN")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string KiemSatVien { get; set; }

        public int HoiDong { get; set; }

        public string DinhKemFile { get; set; }

        public string NoiDungTuyenXu { get; set; }

        public int GiaiDoan { get; set; }

        public int CongDoanHoSo { get; set; }

        public int TrangThai { get; set; }

        public string NguoiTao { get; set; }

        public DateTime? NgayTao { get; set; }

        public string GhiChu { get; set; }

        public NhanVienModel NhanVienThamPhan { get; set; }
        public NhanVienModel NhanVienThamPhan1 { get; set; }
        public NhanVienModel NhanVienThamPhan2 { get; set; }
        public NhanVienModel NhanVienThamPhanKhac { get; set; }
        public NhanVienModel NhanVienHoiThamNhanDan { get; set; }
        public NhanVienModel NhanVienHoiThamNhanDan2 { get; set; }
        public NhanVienModel NhanVienHoiThamNhanDan3 { get; set; }
        public NhanVienModel NhanVienThuKy { get; set; }
        public NhanVienModel NhanVienKiemSatVien { get; set; }
    }
}
