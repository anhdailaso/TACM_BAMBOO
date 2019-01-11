using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Biz.Lib.TACM.Resources.Resources;

namespace Biz.Lib.TACM.ChuanBiXetXu.Model
{
    public class QuyetDinhHinhSuModel
    {
        public int QuyetDinhID { get; set; }

        [Required]
        public int HoSoVuAnID { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_SOQUYETDINH")]
        //[Range(0, int.MaxValue, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_SONGUYENDUONG")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string SoQuyetDinh { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_LANTHU")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public int LanThu { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_NGAYTHANGNAM")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public DateTime NgayRaQuyetDinh { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_NGAYRUTHOSO")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public DateTime? NgayRutHoSo { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_LYDO")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string LyDo { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_LYDO")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public List<String> DanhSachLyDo { get; set; }

        public List<LyDo> DanhSachLyDoSelected { get; set; }

        public string DinhKemFile { get; set; }

        public int QuyetDinhGroup { get; set; }

        public int LoaiDinhChi { get; set; }

        public int GiaiDoan { get; set; }

        public int CongDoanHoSo { get; set; }

        public int TrangThai { get; set; }

        public string NguoiTao { get; set; }

        public DateTime? NgayTao { get; set; }

        public string GhiChu { get; set; }

        public string KetQuaGiaiQuyet { get; set; }
    }

    public class LyDo
    {
        public string LyDoItem { get; set; }
        public bool Checked { get; set; }
    }
}
