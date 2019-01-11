using Biz.Lib.Authentication;
using Biz.Lib.Helpers;
using Biz.Lib.TACM.GiamDocThamTaiTham.DataAccess;
using Biz.Lib.TACM.GiamDocThamTaiTham.IDataAccess;
using Biz.Lib.TACM.GiamDocThamTaiTham.Model;
using Biz.Lib.SettingData.Model;
using Biz.TACM.IServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Excel;
using Biz.TACM.Models.ViewModel.GiamDocThamTaiTham;
using System.Threading.Tasks;
using Biz.Lib.TACM.Resources.Resources;

namespace Biz.TACM.Services
{
    public class GiamDocThamTaiThamService : IGiamDocThamTaiThamService
    {
        private IGiamDocThamTaiThamDataAccess _GiamDocThamTaiThamDA;

        private IGiamDocThamTaiThamDataAccess GiamDocThamTaiThamDA
        {
            get { return _GiamDocThamTaiThamDA ?? (_GiamDocThamTaiThamDA = new GiamDocThamTaiThamDataAccess()); }
        }
        private ISettingDataService _settingDataService;
        private ISettingDataService SettingDataService => _settingDataService ?? (_settingDataService = new SettingDataService());
       

        public IEnumerable<GiamDocThamTaiThamViewListModel> DanhSachGDTTT()
        {
            try
            {
                var list = new List<GiamDocThamTaiThamViewListModel>();
                list = GiamDocThamTaiThamDA.DanhSachGDTTT().ToList();
                return list;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("DanhSachGDTTT with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }
        
        public GiamDocThamTaiThamViewModel ChiTietGDTTT(int gDTTTid)
        {
            try
            {
                var dbModel = GiamDocThamTaiThamDA.ChiTietGDTTT(gDTTTid);
                var hs = TimHoSo(dbModel.MaHoSo);
                if (dbModel != null)
                {
                    return new GiamDocThamTaiThamViewModel()
                    {
                        GiamDocThamTaiThamID=gDTTTid,
                        MaHoSo=dbModel.MaHoSo,
                        So=dbModel.So,
                        SoQuyetDinh=dbModel.SoQuyetDinh,
                        NgayRaQuyetDinh= dbModel.NgayRaQuyetDinh != null ? dbModel.NgayRaQuyetDinh.Value.ToString("dd/MM/yyyy") : null,
                        Nhom=dbModel.Nhom,
                        NhomAn=dbModel.NhomAn,
                        NoiDungQuyetDinh=dbModel.NoiDungQuyetDinh,
                        NoiDungHuySuaAn=dbModel.NoiDungHuySuaAn,
                        GhiChu=dbModel.GhiChu,
                        HuySuaSoTham=dbModel.HuySuaSoTham,
                        HuySuaPhucTham=dbModel.HuySuaPhucTham,
                        BanAnSoTham=dbModel.BanAnSoTham,
                        BanAnPhucTham=dbModel.BanAnPhucTham,
                        QuyetDinhSoTham=dbModel.QuyetDinhSoTham,
                        QuyetDinhPhucTham=dbModel.QuyetDinhPhucTham,
                        Hoso = hs
                    };
                }
                else
                    return new GiamDocThamTaiThamViewModel()
                    {
                        Hoso=hs
                    };
                
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("ChiTietGDTTT with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }
        public ResponseResult SuaGDTTT(GiamDocThamTaiThamViewModel model)
        {
            try
            {
                var result = new ResponseResult();
                var dbModel = new GiamDocThamTaiThamModel()
                {
                    GiamDocThamTaiThamID = model.GiamDocThamTaiThamID,
                    MaHoSo = model.MaHoSo,
                    HoSoVuAnID=model.HoSoVuAnID,
                    So = model.So,
                    SoQuyetDinh = model.SoQuyetDinh,
                    NgayRaQuyetDinh = model.NgayRaQuyetDinh != null ? Convert.ToDateTime(model.NgayRaQuyetDinh) : (DateTime?)null,
                    Nhom = model.Nhom,
                    NhomAn = model.NhomAn,
                    NoiDungQuyetDinh = model.NoiDungQuyetDinh,
                    NoiDungHuySuaAn = model.NoiDungHuySuaAn,
                    GhiChu = model.GhiChu,
                    HuySuaSoTham = model.HuySuaSoTham,
                    HuySuaPhucTham = model.HuySuaPhucTham,
                    BanAnSoTham = model.BanAnSoTham,
                    BanAnPhucTham = model.BanAnPhucTham,
                    QuyetDinhSoTham = model.QuyetDinhSoTham,
                    QuyetDinhPhucTham = model.QuyetDinhPhucTham,
                    NguoiTao=model.NguoiTao
                };
                result = GiamDocThamTaiThamDA.SuaGDTTT(dbModel);
                return result;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("SuaGDTTT with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }
        public ResponseResult XoaGDTTT(int gDTTTid, string NguoiTao)
        {
            try
            {
                var result = GiamDocThamTaiThamDA.XoaGDTTT(gDTTTid, NguoiTao);
                return result;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("XoaGDTTT with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }
        public HoSoVuAnViewModel TimHoSo(string maHoSo)
        {
            try
            {
                var dbModel = GiamDocThamTaiThamDA.TimHoSo(maHoSo);
                var st = new List<NhanVienModel>();
                var pt = new List<NhanVienModel>();
                if (dbModel != null)
                {   
                    if (dbModel.ThamPhanST != null)
                    {
                        var x = SettingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.ThamPhanST);
                        if(x!=null)
                            st.Add(x);
                    }  
                    if (dbModel.ThamPhanKhac != null && dbModel.HoiDong == 2)
                    {
                        var x = SettingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.ThamPhanKhac);
                        if (x != null)
                            st.Add(x);
                    }
                    if (dbModel.HoiThamNhanDan != null)
                    {
                        var x = SettingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.HoiThamNhanDan);
                        if (x != null)
                            st.Add(x);
                    }
                    if (dbModel.HoiThamNhanDan2 != null)
                    {
                        var x = SettingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.HoiThamNhanDan2);
                        if (x != null)
                            st.Add(x);
                    }
                    if (dbModel.HoiThamNhanDan3 != null && dbModel.HoiDong == 2)
                    {
                        var x = SettingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.HoiThamNhanDan3);
                        if (x != null)
                            st.Add(x);
                    }
                    if (dbModel.KiemSatVienST != null)
                    {
                        var x = SettingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.KiemSatVienST);
                        if (x != null)
                            st.Add(x);
                    }
                    if (dbModel.ThuKyST != null)
                    {
                        var x = SettingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.ThuKyST);
                        if (x != null)
                            st.Add(x);
                    }

                    if (dbModel.GiaiDoan == 2)
                    {
                        if (dbModel.ThamPhanPT != null)
                        {
                            var x = SettingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.ThamPhanPT);
                            if (x != null)
                                pt.Add(x);
                        }
                        if (dbModel.ThamPhan1 != null)
                        {
                            var x = SettingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.ThamPhan1);
                            if (x != null)
                                pt.Add(x);
                        }
                        if (dbModel.ThamPhan2 != null)
                        {
                            var x = SettingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.ThamPhan2);
                            if (x != null)
                                pt.Add(x);
                        }
                        if (dbModel.KiemSatVienPT != null)
                        {
                            var x = SettingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.KiemSatVienPT);
                            if (x != null)
                                pt.Add(x);
                        }
                        if (dbModel.ThuKyPT != null)
                        {
                            var x = SettingDataService.ChiTietNhanVienTheoMaNhanVien(dbModel.ThuKyPT);
                            if (x != null)
                                pt.Add(x);
                        }
                    }

                    return new HoSoVuAnViewModel()
                    {
                        MaHoSo = dbModel.MaHoSo,
                        HoiDong = dbModel.HoiDong,
                        GiaiDoan=dbModel.GiaiDoan,
                        NoiDungBanAnST = dbModel.NoiDungBanAnST,
                        NoiDungBanAnPT = dbModel.NoiDungBanAnPT,
                        NoiDungQuyetDinhST = dbModel.NoiDungQuyetDinhST,
                        NoiDungQuyetDinhPT = dbModel.NoiDungQuyetDinhPT,
                        HoiDongSoTham = st,
                        HoiDongPhucTham = pt
                    };
                }
                else
                    return new HoSoVuAnViewModel()
                    {
                        HoiDongSoTham=st,
                        HoiDongPhucTham=pt
                    };
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("TimHoSo with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public List<MaHoSoModel> DanhSachHoSo(string nhomAn)
        {
            try
            {
                var list = new List<MaHoSoModel>();
                list = GiamDocThamTaiThamDA.DanhSachHoSo(nhomAn).ToList();
                return list;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("DanhSachHoSo with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }
    }
}