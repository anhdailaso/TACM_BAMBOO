using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Lib.TACM.NhanDon.Models
{

    public class NoiDungDonEntity : EntityBase<int>
    {
        public int NoiDungDonID { get; set; }
        public int HoSoVuAnID { get; set; }
        public string NoiDungDon { get; set; }

        [MaxLength(100)]
        public string NhomNghiepVu { get; set; }
        public int? GiaiDoan { get; set; }
        public int? CongDoanHoSo { get; set; }
        public int? TrangThai { get; set; }

        [MaxLength(100)]
        public string NguoiTao { get; set; }
        public DateTime? NgayTao { get; set; }
        public string GhiChu { get; set; }

        public override int Id => NoiDungDonID;
    }
}
