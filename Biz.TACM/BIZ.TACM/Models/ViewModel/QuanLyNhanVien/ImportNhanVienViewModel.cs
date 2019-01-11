using System;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Biz.Lib.TACM.Resources.Resources;

namespace Biz.TACM.Models.ViewModel.QuanLyNhanVien
{
    public class ImportNhanVienViewModel
    {
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_CHUACHONFILE")]
        public HttpPostedFileBase File { get; set; }
    }
}
