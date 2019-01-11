using System;
namespace Biz.Lib.TACM.MauIn.Model
{
    public class MauInModel
    {
        public int MauInID { get; set; }
        public string NhomAn { get; set; }
        public int GiaiDoan { get; set; }
        public int CongDoanHoSo { get; set; }
        public int SoMauIn { get; set; }
        public string TenMauIn { get; set; }
        public string DuongDanFileMau { get; set; }
        public string DinhDangHTML { get; set; }
        public int TrangThai { get; set; }
        public string NguoiTao { get; set; }
        public DateTime NgayTao { get; set; }
        public string GhiChu { get; set; }
    }
}
