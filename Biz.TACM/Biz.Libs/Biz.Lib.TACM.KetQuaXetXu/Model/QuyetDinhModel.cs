using Biz.Lib.TACM.Resources.Resources;
using System;
using Biz.Lib.SettingData.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Lib.TACM.KetQuaXetXu.Model
{
    public class QuyetDinhModel
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

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_LOAIQUYETDINH")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string LoaiQuyetDinh { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_LOAI_QUANHE")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string LoaiQuanHe { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_QUANHE_PHAPLUAT")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string QuanHePhapLuat { get; set; }

        //[Display(ResourceType = typeof(ViewText), Name = "LABEL_BIENPHAP_XLHC_DUOC_APDUNG")]
        //[Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string BienPhapXuLyHanhChinh { get; set; }

        //[Display(ResourceType = typeof(ViewText), Name = "LABEL_THOIHAN_APDUNG")]
        //[Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string ThoiHanApDung { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_XEMXETQUYETDINH")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string LyDo { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_KETQUA_GIAIQUYET")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string KetQuaGiaiQuyet { get; set; }

        //[Display(ResourceType = typeof(ViewText), Name = "LABEL_HIEULUC")]
        //[Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        //public DateTime HieuLuc { get; set; }

        public string DinhKemFile { get; set; }

        public string NoiDungQuyetDinh { get; set; }

        public int GiaiDoan { get; set; }

        public int CongDoanHoSo { get; set; }

        public int TrangThai { get; set; }

        public string NguoiTao { get; set; }

        public DateTime? NgayTao { get; set; }

        public string GhiChu { get; set; }

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
