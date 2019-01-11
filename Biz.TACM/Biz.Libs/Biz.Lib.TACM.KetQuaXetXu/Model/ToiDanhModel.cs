using Biz.Lib.SettingData.Model;
using Biz.Lib.TACM.Resources.Resources;
using System;
using System.ComponentModel.DataAnnotations;

namespace Biz.Lib.TACM.KetQuaXetXu.Model
{
    public class ToiDanhModel
    {
        public int STT { get; set; }
        public int ToiDanhID { get; set; }
        public int HoSoVuAnID { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_BICAO")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string BiCao { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_KETQUA_XETXU")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string KetQuaXetXu { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_DIEULUATAPDUNG")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string DieuLuatApDung { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_TOIDANH")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string ToiDanh { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_ANPHI")]
        [MaxLength(23, ErrorMessage = "Giá trị tranh chấp không được vượt quá 18 kí tự.")]
        //[MaxLength(23, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_SOLUONGKYTUTOIDA")]
        [RegularExpression("([0-9.]+)", ErrorMessage = "Giá trị nhập không hợp lệ.")]
        //[RegularExpression("([0-9.]+)", ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_GIATRINHAPKHONGHOPLE")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string AnPhi { get; set; }

        public string HinhPhat { get; set; }
        public string BienPhapTuPhap { get; set; }
        public string BienPhapKhienTrach { get; set; }
        public string DinhKemFile { get; set; }
        public string TrachNhiemDanSu { get; set; }
        public string XuLy { get; set; }
        public int GiaiDoan { get; set; }
        public int CongDoanHoSo { get; set; }
        public int TrangThai { get; set; }
        public string NguoiTao { get; set; }
        public DateTime? NgayTao { get; set; }
        public string GhiChu { get; set; }
        public string TenBiCao { get; set; }
    }
}
