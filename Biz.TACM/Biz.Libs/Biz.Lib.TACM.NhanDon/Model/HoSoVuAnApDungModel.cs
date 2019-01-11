using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Biz.Lib.SettingData.Model;
using Biz.Lib.TACM.Resources.Resources;

namespace Biz.Lib.TACM.NhanDon.Model
{
    public class HoSoVuAnApDungModel
    {
        public int HoSoVuAnID { get; set; }

        public string MaHoSo { get; set; }

        public int SoHoSo { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_SOTHULY")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string SoThuLy { get; set; }

        public int ToaAnID { get; set; }

        public string NhomAn { get; set; }

        public int GiaiDoan { get; set; }

        public int CongDoanHoSo { get; set; }

        public string TrangThaiCongDoan { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_NGAYNHAN_HOSO")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public DateTime? NgayNopDonTaiToaAn { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_HINHTHUC_NHANHOSO")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string HinhThucGoiDon { get; set; }

        public string HinhThucGoiDonKhac { get; set; }

        //[Display(ResourceType = typeof(ViewText), Name = "LABEL_NGUOINHAN_HOSO")]
        //[Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string CanBoNhanDon { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_TENCOQUAN_DENGHI")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string TenCoQuanDeNghi { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_HOSO_DENGHI")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string HoSoDeNghi { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_BIENPHAP_XLHC_DUOC_DENGHI")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string BienPhapXuLyHanhChinh { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_THOIHAN_APDUNG_DENGHI")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string ThoiHanApDung { get; set; }

        public string LoaiQuanHe { get; set; }

        public int TrangThai { get; set; }

        public string NguoiCapNhat { get; set; }

        public DateTime? NgayCapNhat { get; set; }

        public string NguoiTao { get; set; }

        public DateTime? NgayTao { get; set; }

        public string GhiChu { get; set; }

        public IEnumerable<DuongSuModel> DuongSu { get; set; }

        public string ThamPhan { get; set; }

        public string ThamPhan1 { get; set; }

        public string ThamPhan2 { get; set; }

        public string ThamPhanKhac { get; set; }

        public string HoiThamNhanDan { get; set; }

        public string HoiThamNhanDan2 { get; set; }

        public string HoiThamNhanDan3 { get; set; }

        public string ThuKy { get; set; }

        public string KiemSatVien { get; set; }

        public int HoiDong { get; set; }

        public NhanVienModel NhanVienCanBoNhanDon { get; set; }
    }
}
