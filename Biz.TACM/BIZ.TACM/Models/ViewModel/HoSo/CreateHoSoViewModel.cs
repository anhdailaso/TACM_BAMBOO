using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Biz.TACM.Models.ViewModel.HoSo
{
    public class CreateHoSoViewModel
    {
        public DateTime NgayLamDon { get; set; }
        public DateTime NgayNopDonTaiToaAn { get; set; }

        [MaxLength(200)]
        public string HinhThucGuiDon { get; set; }

        [MaxLength(200)]
        public string LoaiDon { get; set; }

        [MaxLength(200)]
        public string LoaiQuanHe { get; set; }

        [MaxLength(200)]
        public string QuanHePhapLuat { get; set; }

        [MaxLength(50)]
        public string YeuToNuocNgoai { get; set; }

        [MaxLength(100)]
        public string NguoiKyXacNhan { get; set; }
        public string NoiDungDon { get; set; }
    }
}