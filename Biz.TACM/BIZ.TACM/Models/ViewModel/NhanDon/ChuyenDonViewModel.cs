using Biz.Lib.TACM.Resources.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Biz.TACM.Models.ViewModel.NhanDon
{
    public class ChuyenDonViewModel
    {
        public int ChuyenDonID { get; set; }

        public int HoSoVuAnID { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_NGAYRATHONGBAO")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages),ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string NgayRaThongBao { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_NGAYCHUYENDON")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string NgayChuyenDon { get; set; }

        public string ToaAnChuyenDi { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_TOAANCHUYENDEN")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string ToaAnChuyenDen { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_LYDO_CHUYENDON")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string LyDoChuyenDon { get; set; }

        public string TruongHopChuyen { get; set; }

        public string NhomNghiepVu { get; set; }

        public int GiaiDoan { get; set; }

        public int CongDoanHoSo { get; set; }

        public int TrangThai { get; set; }

        public string NguoiTao { get; set; }

        public string NgayTao { get; set; }

        public string GhiChu { get; set; }

        public string TrangThaiCongDoan { get; set; }
    }
}