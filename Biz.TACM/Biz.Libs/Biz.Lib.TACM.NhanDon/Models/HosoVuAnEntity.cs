using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Lib.TACM.NhanDon.Models
{
    public class HosoVuAnEntity : EntityBase<int>
    {
        public int HoSoVuAnID { get; set; }

        [MaxLength(50)]
        public string MaHoSo { get; set; }
        public int? SoHoSo { get; set; }

        [MaxLength(50)]
        public string NhomAn { get; set; }
        public int? GiaiDoan { get; set; }
        public int? CongDoanHoSo { get; set; }

        [MaxLength(100)]
        public string TrangThaiCongDoan { get; set; }
        public DateTime? NgayLamDon { get; set; }
        public DateTime? NgayNopDonTaiToaAn { get; set; }

        [MaxLength(200)]
        public string HinhThucGoiDon { get; set; }

        [MaxLength(200)]
        public string LoaiDon { get; set; }

        [MaxLength(200)]
        public string LoaiQuanHe { get; set; }

        [MaxLength(200)]
        public string QuanHePhapLuat { get; set; }

        [MaxLength(50)]
        public string YeuToNuocNgoai { get; set; }

        [MaxLength(100)]
        public string NguoiKyXacNhanDaNhanDon { get; set; }
        public int? TrangThai { get; set; }

        [MaxLength(100)]
        public string NguoiTao { get; set; }
        public DateTime? NgayTao { get; set; }
        public string GhiChu { get; set; }

        public override int Id => HoSoVuAnID;
    }
}
