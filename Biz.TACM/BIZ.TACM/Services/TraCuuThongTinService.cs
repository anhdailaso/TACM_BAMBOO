using Biz.Lib.TACM.CongThongTinDienTu.IDataAccess;
using Biz.Lib.TACM.CongThongTinDienTu.DataAccess;
using Biz.TACM.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Biz.TACM.Models.ViewModel.CongThongTinDienTu;
using Biz.Lib.Helpers;
using Biz.Lib.TACM.NhanDon.DataAccess;
using Biz.Lib.TACM.NhanDon.IDataAccess;

namespace Biz.TACM.Services
{
    public class TraCuuThongTinService : ITraCuuThongTinService
    {
        private ITraCuuThongTinDataAccess _traCuuThongTinDataAccess;
        private ITraCuuThongTinDataAccess TraCuuThongTinDataAccess => _traCuuThongTinDataAccess ?? (_traCuuThongTinDataAccess = new TraCuuThongTinDataAccess());
        private INhanDonDataAccess _nhanDonDataAccess;
        private INhanDonDataAccess NhanDonDataAccess => _nhanDonDataAccess ?? (_nhanDonDataAccess = new NhanDonDataAccess());

        public TraCuuHoSoVuAnViewModel GetGetHoSoVuAnDuocTraCuu(string keyword)
        {
            try
            {
                int hoSoVuAnID = 0;
                var dbModel = TraCuuThongTinDataAccess.GetHoSoVuAnDuocTraCuu(keyword, ref hoSoVuAnID);
                var listDuongSu = NhanDonDataAccess.NguyenDonVaBiDon(hoSoVuAnID);
                var tenVuAn = new List<String>();
                foreach (var duongSu in listDuongSu)
                {
                    tenVuAn.Add(duongSu.HoVaTen);
                }

                TraCuuHoSoVuAnViewModel viewModel = new TraCuuHoSoVuAnViewModel
                {
                    HoSoVuAnID = dbModel.HoSoVuAnID,//
                    MaHoSo = dbModel.MaHoSo,//
                    //TenVuAn = dbModel.TenVuAn,
                    SoThuLySoTham = dbModel.SoThuLySoTham,//
                    QuanHePhapLuat = dbModel.QuanHePhapLuat,//
                    TenThamPhanSoTham = dbModel.TenThamPhanSoTham,//
                    KetQuaGiaiQuyetSoTham = dbModel.KetQuaGiaiQuyetSoTham,//
                    KhangCaoKhangNghi = dbModel.KhangCaoKhangNghi,//
                    SoThuLyPhucTham = dbModel.SoThuLyPhucTham,//
                    TenThamPhanPhucTham = dbModel.TenThamPhanPhucTham,//
                    KetQuaGiaiQuyetPhucTham = dbModel.KetQuaGiaiQuyetPhucTham,//
                    HoaGiaiNgayGioMoPhienHop = string.Format("{0:HH:mm:ss dd/MM/yyyy}", dbModel.HoaGiaiNgayGioMoPhienHop), //
                    DanhSachQuyetDinhThongBaoSoTham = dbModel.DanhSachQuyetDinhThongBaoSoTham,
                    DanhSachQuyetDinhThongBaoPhucTham = dbModel.DanhSachQuyetDinhThongBaoPhucTham,
                    MoPhienHopNgayRaQuyetDinhSoTham = string.Format("{0:dd/MM/yyyy}", dbModel.MoPhienHopNgayRaQuyetDinhSoTham),//
                    MoPhienHopNgayRaQuyetDinhPhucTham = string.Format("{0:dd/MM/yyyy}", dbModel.MoPhienHopNgayRaQuyetDinhPhucTham),//
                    MoPhienHopNgayGioMoPhienHopSoTham = string.Format("{0:HH:mm:ss dd/MM/yyyy}", dbModel.MoPhienHopNgayGioMoPhienHopSoTham),//
                    MoPhienHopNgayGioMoPhienHopPhucTham = string.Format("{0:HH:mm:ss dd/MM/yyyy}", dbModel.MoPhienHopNgayGioMoPhienHopPhucTham),//
                    DuaVuAnRaXetXuNgayRaQuyetDinhSoTham = string.Format("{0:dd/MM/yyyy}", dbModel.DuaVuAnRaXetXuNgayRaQuyetDinhSoTham),//
                    DuaVuAnRaXetXuNgayRaQuyetDinhPhucTham = string.Format("{0:dd/MM/yyyy}", dbModel.DuaVuAnRaXetXuNgayRaQuyetDinhPhucTham),//
                    DuaVuAnRaXetXuNgayGioMoPhienToaSoTham = string.Format("{0:HH:mm:ss dd/MM/yyyy}", dbModel.DuaVuAnRaXetXuNgayGioMoPhienToaSoTham),//
                    DuaVuAnRaXetXuNgayGioMoPhienToaPhucTham = string.Format("{0:HH:mm:ss dd/MM/yyyy}", dbModel.DuaVuAnRaXetXuNgayGioMoPhienToaPhucTham),//
                    TrieuTapLanThuSoTham = dbModel.TrieuTapLanThuSoTham,//
                    TrieuTapLanThuPhucTham = dbModel.TrieuTapLanThuPhucTham,//
                    TrieuTapThoiGianMoPhienToaSoTham = string.Format("{0:HH:mm:ss dd/MM/yyyy}", dbModel.TrieuTapThoiGianMoPhienToaSoTham),//
                    TrieuTapThoiGianMoPhienToaPhucTham = string.Format("{0:HH:mm:ss dd/MM/yyyy}", dbModel.TrieuTapThoiGianMoPhienToaPhucTham),//
                    TrieuTapDiaDiemMoPhienToaSoTham = dbModel.TrieuTapDiaDiemMoPhienToaSoTham,//
                    TrieuTapDiaDiemMoPhienToaPhucTham = dbModel.TrieuTapDiaDiemMoPhienToaPhucTham,//

                    LoaiQuanHe = dbModel.LoaiQuanHe,//
                    GiaiDoan = dbModel.GiaiDoan,//
                    CongDoanHoSo = dbModel.CongDoanHoSo,//
                    TrangThaiCongDoan = dbModel.TrangThaiCongDoan,//
                    NhomAn = dbModel.NhomAn
                };
                viewModel.TenVuAn = String.Join(" - ", tenVuAn);

                return viewModel;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("GetGetHoSoVuAnDuocTraCuu with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }
    }
}