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
    public class XetXuModel
    {
        public int XetXuID { get; set; }

        [Required]
        public int HoSoVuAnID { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_THUTUC")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string ThuTuc { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_NGAYRA_QUYETDINH")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public DateTime NgayRaQuyetDinh { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_THOIGIAN")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public DateTime ThoiGianMoPhienToa { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_DIADIEM")]
        [StringLength(250, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_SOLUONGKYTUTOIDA")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages),ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string DiaDiemMoPhienToa { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_VUAN_DUOCXETXU")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string VuAnDuocXetXu { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_THAMPHAN_CHUTOA")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string ThamPhan { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_THAMPHAN_KHAC")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string ThamPhanKhac { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_THAMPHAN1")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string ThamPhan1 { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_THAMPHAN2")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string ThamPhan2 { get; set; }

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

        public int XetXuGroup { get; set; }

        public int LoaiKetQuaXetXu { get; set; }

        public int GiaiDoan { get; set; }

        public int CongDoanHoSo { get; set; }

        public int TrangThai { get; set; }

        public string NguoiTao { get; set; }

        public DateTime? NgayTao { get; set; }

        public string GhiChu { get; set; }

        public List<NhanVienModel> ThamPhanDuKhuyet { get; set; }

        public List<NhanVienModel> HoiThamNhanDanDuKhuyet { get; set; }

        public List<NhanVienModel> ThuKyDuKhuyet { get; set; }

        public List<NhanVienModel> KiemSatVienDuKhuyet { get; set; }

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
