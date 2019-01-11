using System;
using System.Collections.Generic;
using Biz.Lib.TACM.Resources.Resources;
using System.ComponentModel.DataAnnotations;

namespace Biz.Lib.TACM.NhanDon.Model
{
    public class ToiDanhTruyToModel
    {        
        public int STT { get; set; }
        public int ToiDanhTruyToID { get; set; }
        public int HoSoVuAnID { get; set; }

        [Display(ResourceType = typeof(ViewText), Name = "LABEL_TOIDANH")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string ToiDanh { get; set; }
        [Display(ResourceType = typeof(ViewText), Name = "LABEL_DIEULUAT")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string Dieu { get; set; }
        public string BoLuatHinhSu { get; set; }
        public string DieuLuatApDung { get; set; }

        //[Display(ResourceType = typeof(ViewText), Name = "LABEL_NOIDUNG")]
        //[Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string NoiDungDieuLuat { get; set; }

        public int GiaiDoan { get; set; }
        public int CongDoanHoSo { get; set; }
        public int TrangThai { get; set; }
        public string NguoiTao { get; set; }
        public DateTime NgayTao { get; set; }
        public string GhiChu { get; set; }
        public List<ToiDanhTruyTo_KhoanDiemModel> KhoanDiem { get; set; }
    }
    public class ToiDanhTruyTo_KhoanDiemModel
    {
        public int KhoanDiemID { get; set; }
        public int ToiDanhTruyToID { get; set; }
        public string Khoan { get; set; }
        public string Diem { get; set; }
        public string NguoiTao { get; set; }
        public int check { get; set; }
    }
}

