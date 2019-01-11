using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Biz.Lib.SettingData.Model;
using Biz.Lib.TACM.Resources.Resources;
using Biz.Lib.TACM.ThuLy.Model;

namespace Biz.TACM.Models.ViewModel.ThuLy
{
    public class PhanCongThamPhanViewModel
    {
        public int ThamPhanID { get; set; }
        public int HoSoVuAnID { get; set; }

        //[Display(ResourceType = typeof(ViewText), Name = "LABEL_THAMPHAN")]
        //[Required(ErrorMessageResourceType = typeof(ValidationMessages),ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string ThamPhan { get; set; }

        //[Display(ResourceType = typeof(ViewText), Name = "LABEL_THAMPHAN1")]
        //[Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string ThamPhan1 { get; set; }

        //[Display(ResourceType = typeof(ViewText), Name = "LABEL_THAMPHAN2")]
        //[Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string ThamPhan2 { get; set; }

        public string ThamPhanKhac { get; set; }

        //[Display(ResourceType = typeof(ViewText), Name = "LABEL_NGAYPHANCONG")]
        //[Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string NgayPhanCong { get; set; }

        //[Display(ResourceType = typeof(ViewText),Name = "LABEL_NGUOIPHANCONG")]
        //[Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string TenNguoiPhanCong { get; set; }

        //[Display(ResourceType = typeof(ViewText), Name = "LABEL_HOITHAMNHANDAN_1")]
        //[Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string HoiThamNhanDan { get; set; }

        //[Display(ResourceType = typeof(ViewText), Name = "LABEL_HOITHAMNHANDAN_2")]
        //[Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string HoiThamNhanDan2 { get; set; }

        //[Display(ResourceType = typeof(ViewText), Name = "LABEL_HOITHAMNHANDAN_3")]
        //[Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string HoiThamNhanDan3 { get; set; }

        //[Display(ResourceType = typeof(ViewText), Name = "LABEL_THUKY")]
        //[Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "VALIDATION_KIEMTRARONG")]
        public string ThuKy { get; set; }

        public int HoiDong { get; set; }

        public string NhomNghiepVu { get; set; }
        public int GiaiDoan { get; set; }
        public int CongDoanHoSo { get; set; }
        public int TrangThai { get; set; }
        public string NguoiTao { get; set; }
        public string NgayTao { get; set; }
        public string GhiChu { get; set; }
        public List<ThamPhanDuKhuyetModel> ThamPhanDuKhuyetViewModel { get; set; }
        public List<HoiThamNhanDanDuKhuyetModel> HoiThamNhanDanDuKhuyetViewModel { get; set; }
        public List<ThuKyDuKhuyetModel> ThuKyDuKhuyetViewModel { get; set; }
        public List<String> ThamPhanDuKhuyet { get; set; }
        public List<String> HoiThamNhanDanDuKhuyet { get; set; }
        public List<String> ThuKyDuKhuyet { get; set; }

        public NhanVienModel NhanVienThamPhan { get; set; }
        public NhanVienModel NhanVienThamPhan1 { get; set; }
        public NhanVienModel NhanVienThamPhan2 { get; set; }
        public NhanVienModel NhanVienThamPhanKhac { get; set; }
        public NhanVienModel NhanVienNguoiPhanCong { get; set; }
        public NhanVienModel NhanVienHoiThamNhanDan { get; set; }
        public NhanVienModel NhanVienHoiThamNhanDan2 { get; set; }
        public NhanVienModel NhanVienHoiThamNhanDan3 { get; set; }
        public NhanVienModel NhanVienThuKy { get; set; }
    }

    //public class ThamPhanDuKhuyetViewModel
    //{
    //    public int HoSoVuAnID { get; set; }
    //    public int ThamPhanDuKhuyetID { get; set; }
    //    public string ThamPhanDuKhuyet { get; set; }
    //}

    //public class HoiThamNhanDanDuKhuyetViewModel
    //{
    //    public int HoSoVuAnID { get; set; }
    //    public int HoiThamNhanDanDuKhuyetID { get; set; }
    //    public string HoiThamNhanDanDuKhuyet { get; set; }
    //}

    //public class ThuKyDuKhuyetViewModel
    //{
    //    public int HoSoVuAnID { get; set; }
    //    public int ThuKyDuKhuyetID { get; set; }
    //    public string ThuKyDuKhuyet { get; set; }
    //}
}