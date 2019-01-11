using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Biz.Lib.TACM.QuanLyNhanVien.Model;
using Biz.Lib.TACM.Resources.Resources;

using Newtonsoft.Json;

namespace Biz.TACM.Models.ViewModel.SoDoToChuc
{
    public class ChucDanhViewModel
    {
        public int SoDoToChucID { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_TENCHUCDANH")]
        [StringLength(150, ErrorMessage ="{0} không được quá {1} ký tự.")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages),
                ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string ChucDanh { get; set; }

        public int? ChucDanhChaID { get; set; }
        public int ToaAnID { get; set; }
        public string DienGiaiNghiepVu { get; set; }
        public int TrangThai { get; set; }
        public string NguoiTao { get; set; }
        public string NgayTao { get; set; }
        public string GhiChu { get; set; }
        public string ChucDanhCha { get; set; }
        public int Loai { get; set; }
        public int SoLuongNhanVienTheoChucDanh { get; set; }
    }
}