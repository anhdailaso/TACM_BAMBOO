using System.Web.Optimization;

namespace Biz.TACM
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.cookie.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/jquery.stickytabs.js",
                        "~/Scripts/tether.min.js",
                        "~/Scripts/bootstrap.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/datatables").Include(
                "~/Scripts/jquery.dataTables.min.js",
                "~/Scripts/dataTables.bootstrap4.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/loadingOverlay").Include(
                "~/Scripts/loadingoverlay.min.js",
                "~/Scripts/loadingoverlay_progress.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/boxSlider").Include(
                "~/Scripts/box-slider-all.jquery.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/moment").Include(
                "~/Scripts/moment.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/datetimepicker").Include(
                "~/Scripts/bootstrap-datetimepicker.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/notify").Include(
                "~/Scripts/bootstrap-notify.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/tinymce").Include(
                "~/Scripts/tinymce/tinymce.min.js",
                "~/Scripts/tinymce/themes/modern/theme.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-grid.css",
                      "~/Content/bootstrap-reboot.css",
                      "~/Content/font-awesome.min.css",
                      "~/Content/dataTables.bootstrap4.min.css",
                      "~/Content/bootstrap-datetimepicker.min.css",
                      "~/Content/animate.css",
                      "~/Content/site.css",
                      "~/Content/custom.css",
                      "~/Content/jquery.treegrid.css",
                      "~/Content/jquery-ui.css",
                      "~/Content/dropzone-basic.css",
                      "~/Content/dropzone.css"));

            bundles.Add(new ScriptBundle("~/bundles/biz").Include(
                "~/Scripts/BIZ.Application.js",
                "~/Scripts/BIZ.UploadDocuments.js",
                "~/Scripts/dropzone.js"));

            bundles.Add(new ScriptBundle("~/bundles/treeview").Include(
                "~/Scripts/jquery.treegrid.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqplot")
                .Include("~/Scripts/jquery.jqplot/jquery.jqplot.{version}.js")
                .Include("~/Scripts/jquery.jqplot/excanvas.js")
                .Include("~/Scripts/jquery.jqplot/plugins/*.js"));


            bundles.Add(new ScriptBundle("~/bundles/underscore").Include(
               "~/Scripts/underscore-min.js"));

            /* custom js */
            bundles.Add(new ScriptBundle("~/bundles/NhanDon")
                //.Include("~/Scripts/Custom/NhanDon/Biz.NhandonChiTiet.js")
                .Include("~/Scripts/Custom/NhanDon/Biz.Nhandon.js"));

            bundles.Add(new ScriptBundle("~/bundles/NhanDonChiTiet")
                .Include("~/Scripts/Custom/NhanDon/Biz.NhandonChiTiet.js")
                //.Include("~/Scripts/Custom/NhanDon/Biz.Nhandon.js")
                .Include("~/Scripts/Custom/NhanDon/Biz.SuaDoiBoSungDon.js")
                .Include("~/Scripts/Custom/NhanDon/Biz.TraLaiDon.js")
                .Include("~/Scripts/Custom/NhanDon/Biz.KhieuNaiTraLaiDon.js")
                .Include("~/Scripts/Custom/NhanDon/Biz.ChuyenDon.js")
                .Include("~/Scripts/Custom/NhanDon/Biz.ThamPhan.js")
                .Include("~/Scripts/Custom/NhanDon/Biz.DuongSu.js")
                .Include("~/Scripts/Custom/NhanDon/Biz.NoiDungDon.js")
                .Include("~/Scripts/Custom/NhanDon/Biz.ToiDanhTruyTo.js"));

            bundles.Add(new ScriptBundle("~/bundles/ThuLy")
                .Include("~/Scripts/Custom/ThuLy/Biz.ThuLy.js")
                .Include("~/Scripts/Custom/NhanDon/Biz.DuongSu.js")
                .Include("~/Scripts/Custom/ThuLy/Biz.AnPhi.js")
                .Include("~/Scripts/Custom/ThuLy/Biz.ThongTinThuLy.js")
                .Include("~/Scripts/Custom/ThuLy/Biz.GhiChuPhanCong.js")
                .Include("~/Scripts/Custom/ThuLy/Biz.PhanCongThamPhan.js"));

            bundles.Add(new ScriptBundle("~/bundles/SauXetXu")
                .Include("~/Scripts/Custom/SauXetXu/BIZ.SauXetXu.js")
                .Include("~/Scripts/Custom/SauXetXu/BIZ.PhatHanhBanAn.js")
                .Include("~/Scripts/Custom/SauXetXu/BIZ.SuaChuaBanAn.js")
                .Include("~/Scripts/Custom/SauXetXu/BIZ.KhangCao.js")
                .Include("~/Scripts/Custom/SauXetXu/BIZ.KhangNghi.js")
                .Include("~/Scripts/Custom/SauXetXu/BIZ.KhangCaoQuaHan.js")
                .Include("~/Scripts/Custom/SauXetXu/BIZ.LuuKho.js")
                .Include("~/Scripts/Custom/SauXetXu/BIZ.TamUngAnPhi.js")
                .Include("~/Scripts/Custom/SauXetXu/BIZ.TraLaiDonKhangCao.js")
                .Include("~/Scripts/Custom/SauXetXu/BIZ.ChuyenHoSo.js"));

            bundles.Add(new ScriptBundle("~/bundles/XetXu")
                .Include("~/Scripts/Custom/XetXu/BIZ.XetXu.js")
                .Include("~/Scripts/Custom/XetXu/BIZ.XetXuNoiDung.js")
                .Include("~/Scripts/Custom/XetXu/BIZ.XetXuTrieuTap.js"));

            bundles.Add(new ScriptBundle("~/bundles/KetQuaXetXu")
                .Include("~/Scripts/Custom/KetQuaXetXu/BIZ.KetQuaXetXu.js")
                .Include("~/Scripts/Custom/KetQuaXetXu/BIZ.KetQuaXetXuQuyetDinh.js")
                .Include("~/Scripts/Custom/KetQuaXetXu/BIZ.KetQuaXetXuQuyetDinhADBPXLHC.js")
                .Include("~/Scripts/Custom/KetQuaXetXu/BIZ.KetQuaXetXuBanAn.js")
                .Include("~/Scripts/Custom/ChuanBiXetXu/BIZ.TraHoSoDieuTraBoSung.js")
                .Include("~/Scripts/Custom/ChuanBiXetXu/BIZ.DinhChiVuAn.js")
                .Include("~/Scripts/Custom/ChuanBiXetXu/BIZ.DinhChiXetXuPhucTham.js"));

            bundles.Add(new ScriptBundle("~/bundles/ChuanBiXetXu")
                .Include("~/Scripts/Custom/ChuanBiXetXu/BIZ.ChuanBiXetXu.js")
                .Include("~/Scripts/Custom/ChuanBiXetXu/BIZ.ChuanBiXetXuHoaGiai.js")
                .Include("~/Scripts/Custom/ChuanBiXetXu/BIZ.ChuanBiXetXuQuyetDinh.js")
                .Include("~/Scripts/Custom/ChuanBiXetXu/BIZ.ChuyenHoSo.js")
                .Include("~/Scripts/Custom/ChuanBiXetXu/BIZ.TraHoSoDieuTraBoSung.js")
                .Include("~/Scripts/Custom/ChuanBiXetXu/BIZ.TamDinhChiVuAn.js")
                .Include("~/Scripts/Custom/ChuanBiXetXu/BIZ.DinhChiVuAn.js")
                .Include("~/Scripts/Custom/ChuanBiXetXu/BIZ.PhucHoiVuAn.js")
                .Include("~/Scripts/Custom/ChuanBiXetXu/BIZ.DinhChiXetXuPhucTham.js")
                .Include("~/Scripts/Custom/NhanDon/Biz.DuongSu.js")
                .Include("~/Scripts/Custom/ThuLy/Biz.PhanCongThamPhan.js"));

            bundles.Add(new ScriptBundle("~/bundles/MauIn")
                .Include("~/Scripts/Custom/MauIn/Biz.MauInChiTiet.js")
                .Include("~/Scripts/Custom/MauIn/Biz.TimKiemMauIn.js"));

            bundles.Add(new ScriptBundle("~/bundles/QuanLyNhanVien")
                .Include("~/Scripts/Custom/QuanLyNhanVien/BIZ.QLNhanVien.js")
                .Include("~/Scripts/Custom/QuanLyNhanVien/Biz.TimKiemNhanVien.js"));

            bundles.Add(new ScriptBundle("~/bundles/SoDoToChuc")
                .Include("~/Scripts/Cstom/SoDoToChuc/Biz.SoDoToChucTreeView.js"));

            bundles.Add(new ScriptBundle("~/bundles/Account")
                .Include("~/Scripts/Custom/Account/Biz.Account.js"));

            bundles.Add(new ScriptBundle("~/bundles/Home")
                .Include("~/Scripts/Custom/Home/Biz.Home.js"));

            bundles.Add(new ScriptBundle("~/bundles/TimKiem")
                .Include("~/Scripts/Custom/NhanDon/Biz.TimKiemOnHeader.js"));

            bundles.Add(new ScriptBundle("~/bundles/NhomAnToaAn")
                .Include("~/Scripts/Custom/NhomAnToaAn/Biz.NhomAnToaAn.js"));

            bundles.Add(new ScriptBundle("~/bundles/PhanCongNhanVien")
                .Include("~/Scripts/Custom/PhanCongNhanVien/Biz.PhanCong.js"));

            bundles.Add(new ScriptBundle("~/bundles/MaHoaHoSoVuAn")
                .Include("~/Scripts/Custom/MaHoaHoSoVuAn/Biz.MaHoaHoSoVuAn.js"));

            bundles.Add(new ScriptBundle("~/bundles/NhanHoSoPhucTham")
                .Include("~/Scripts/Custom/NhanDon/Biz.NhanHoSoPhucTham.js"));

            bundles.Add(new ScriptBundle("~/bundles/NhanHoSo")
                .Include("~/Scripts/Custom/NhanHoSo/Biz.NhanHoSo.js"));

            bundles.Add(new ScriptBundle("~/bundles/ThongKeGiamSat")
                .Include("~/Scripts/Custom/ThongKeGiamSat/Biz.BaoCaoThongKe.js")
                .Include("~/Scripts/Custom/ThongKeGiamSat/Biz.GiamSatThucHien.js"));

            bundles.Add(new ScriptBundle("~/bundles/ThongKePhanCongThamPhan")
                .Include("~/Scripts/Custom/ThongKeGiamSat/Biz.ThongKePhanCongThamPhan.js"));

            bundles.Add(new ScriptBundle("~/bundles/ThongKeLuuKho")
                .Include("~/Scripts/Custom/ThongKeGiamSat/Biz.ThongKeLuuKho.js"));

            bundles.Add(new ScriptBundle("~/bundles/GiamDocThamTaiTham")
                .Include("~/Scripts/Custom/GiamDocThamTaiTham/Biz.GDTTT.js"));

            bundles.Add(new ScriptBundle("~/bundles/ThongKeAnHuySua")
                .Include("~/Scripts/Custom/ThongKeGiamSat/Biz.ThongKeAnHuySua.js"));

            bundles.Add(new ScriptBundle("~/bundles/HuyHoSo")
                .Include("~/Scripts/Custom/HuyHoSo/Biz.HuyHoSo.js"));
        }
    }
}
