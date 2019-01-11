using Biz.Lib.TACM.Resources.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Biz.TACM.Models.ViewModel.SauXetXu.ChuyenHoSo
{
    public class ChuyenHoSoViewModel
    {
        public int ChuyenHoSoID { get; set; }

        public int HoSoVuAnID { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_NGAYRA_PHIEUCHUYEN")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages),ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string NgayRaThongBao { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_NGAYCHUYENHOSO")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string NgayChuyenHoSo { get; set; }

        public string ToaAnChuyenDi { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_TOAANCHUYENDEN")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string ToaAnChuyenDen { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_LYDOCHUYEN_HOSO")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string LyDoChuyenHoSo { get; set; }

        //[Display(ResourceType = typeof(ViewText), Name = "LABEL_TRUONGHOPCHUYEN")]
        //[Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
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