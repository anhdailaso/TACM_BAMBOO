﻿using System;
using System.Collections.Generic;
using Biz.Lib.TACM.MauIn.Model;

namespace Biz.TACM.Models.ViewModel.MauIn
{
    public class MauInLenhTrichXuatViewModel
    {
        public string MaToaAn { get; set; }
        public string TenToaAn { get; set; }
        public string DiaChiToaAn { get; set; }
        public string MaHoSo { get; set; }
        public string SoHoSo { get; set; }
        public string NhomAn { get; set; }
        public string ThamPhan { get; set; }
        public string SoThuLy { get; set; }
        public DateTime NgayThuLy { get; set; }
        public string VienKiemSatTruyTo { get; set; }
        public DateTime NgayRaQuyetDinhXetXu { get; set; }
        public DateTime ThoiGianMoPhienToa { get; set; }
        public int GiaiDoan { get; set; }
        public IEnumerable<DuongSuModel> DanhSachBiCao { get; set; }
       
    }
}