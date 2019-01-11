using System;

namespace Biz.Lib.SettingData.Model
{
    public class NhanVienModel
    {
        public int NhanVienID { get; set; }
        public string MaNV { get; set; }
        public string MaNVMoi { get; set; }
        public string HoNV { get; set; }
        public string TenNV { get; set; }
        public string TenLotNV { get; set; }
        public string GioiTinh { get; set; }
        public DateTime NgaySinh { get; set; }
        public string SoDienThoai { get; set; }
        public string Email { get; set; }
        public string DuongDanHinhDaiDien { get; set; }
        public int SoDoToChucID { get; set; }
        public string ChucDanh { get; set; }
        public int HoSoVuAnID { get; set; }
        public bool Checked { get; set; }
        public int TrangThai { get; set; }
        public string HoVaTenNV
        {
            get
            {
                return HoNV + (string.IsNullOrEmpty(TenLotNV) ? "" : " " + TenLotNV) + " " + TenNV;
            }
        }

        public string HoTenVaMaNV
        {
            get
            {
                return HoNV + (string.IsNullOrEmpty(TenLotNV) ? "" : " " + TenLotNV) + " " + TenNV + (string.IsNullOrWhiteSpace(MaNVMoi) ? " (" + MaNV + ")" : " (" + MaNVMoi + ")");
            }
        }
    }
}
