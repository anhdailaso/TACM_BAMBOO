using System;
using System.Collections.Generic;

namespace Biz.Lib.TACM.ThuLy.Model
{
    public class PhanCongThamPhanModel
    {
        public int ThamPhanID { get; set; }
        public int HoSoVuAnID { get; set; }
        public string ThamPhan { get; set; }
        public string ThamPhan1 { get; set; }
        public string ThamPhan2 { get; set; }
        public string ThamPhanKhac { get; set; }
        public DateTime NgayPhanCong { get; set; }
        public string TenNguoiPhanCong { get; set; }
        public string HoiThamNhanDan { get; set; }
        public string HoiThamNhanDan2 { get; set; }
        public string HoiThamNhanDan3 { get; set; }
        public string ThuKy { get; set; }
        public int HoiDong { get; set; }
        public string NhomNghiepVu { get; set; }
        public int GiaiDoan { get; set; }
        public int CongDoanHoSo { get; set; }
        public int TrangThai { get; set; }
        public string NguoiTao { get; set; }
        public DateTime NgayTao { get; set; }
        public string GhiChu { get; set; }
        public List<String> ThamPhanDuKhuyet { get; set; }
        public List<String> HoiThamNhanDanDuKhuyet { get; set; }
        public List<String> ThuKyDuKhuyet { get; set; }
    }

    public class ThamPhanDuKhuyetModel
    {
        public int HoSoVuAnID { get; set; }
        public int ThamPhanDuKhuyetID { get; set; }
        public string ThamPhanDuKhuyet { get; set; }
    }

    public class HoiThamNhanDanDuKhuyetModel
    {
        public int HoSoVuAnID { get; set; }
        public int HoiThamNhanDanDuKhuyetID { get; set; }
        public string HoiThamNhanDanDuKhuyet { get; set; }
    }

    public class ThuKyDuKhuyetModel
    {
        public int HoSoVuAnID { get; set; }
        public int ThuKyDuKhuyetID { get; set; }
        public string ThuKyDuKhuyet { get; set; }
    }
}
