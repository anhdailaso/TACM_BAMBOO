using System;
using System.Collections.Generic;
using Biz.Lib.Helpers;
using System.Linq;
using System.Web;
using Biz.TACM.Models.ViewModel.HuyHoSo;
using Biz.TACM.IServices;
using Biz.Lib.TACM.HuyHoSo.Model;
using Biz.Lib.TACM.HuyHoSo.DataAccess;
using Biz.Lib.TACM.HuyHoSo.IDataAccess;

namespace Biz.TACM.Services
{
    public class HuyHoSoService : IHuyHoSoService
    {

        private IHuyHoSoDataAccess _HuyHoSoDA;

        private IHuyHoSoDataAccess HuyHoSoDA
        {
            get { return _HuyHoSoDA ?? (_HuyHoSoDA = new HuyHoSoDataAccess()); }
        }
        private ISettingDataService _settingDataService;
        private ISettingDataService SettingDataService => _settingDataService ?? (_settingDataService = new SettingDataService());
        private INhanDonService _nhanDonService;
        private INhanDonService NhanDonService => _nhanDonService ?? (_nhanDonService = new NhanDonService());
        public IEnumerable<HoSoVuAnModel> DanhSachHoSo()
        {
            try
            {
                var model = HuyHoSoDA.DanhSachHoSo();
                var giaiDoan = XMLUtils.BindData("GiaiDoan");
                var tentoa = SettingDataService.ListSettingDataItem("DINHNGHIACHUNG_NHOMANTOANAN");
                int i = model.Count();
                foreach(var item in model)
                {
                    item.SoHoSo = i;
                    item.GiaiDoan = giaiDoan.Where(x => x.value == item.GiaiDoan.ToString()).FirstOrDefault().text;
                    item.TenVuAn = (item.NhomAn == "HS" ? item.TenVuAn : NhanDonService.GetTenCuaCacDuongSu(item.HoSoVuAnID, item.NhomAn));
                    i--;
                }
                return model;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("DanhSachHoSo with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }
        public HoSoVuAnViewModel TimHoSo(string maHoSo)
        {
            try
            {
                var dbModel = HuyHoSoDA.TimHoSo(maHoSo);
                if (dbModel != null)
                {
                    string str = "CongDoanHoSo";
                    if (dbModel.GiaiDoan == "2")
                        str = "CongDoanHoSoPhucTham";
                    return new HoSoVuAnViewModel()
                    {
                        HoSoVuAnID = dbModel.HoSoVuAnID,
                        SoHoSo = dbModel.SoHoSo,
                        MaHoSo = dbModel.MaHoSo,
                        TenVuAn = (dbModel.NhomAn=="HS" ? dbModel.TenVuAn : NhanDonService.GetTenCuaCacDuongSu(dbModel.HoSoVuAnID,dbModel.NhomAn)),
                        NhomAn = SettingDataService.ListSettingDataItem("DINHNGHIACHUNG_NHOMANTOANAN").Where(x => x.ItemRemark == dbModel.NhomAn).FirstOrDefault().ItemText,
                        ToaAn = dbModel.ToaAn,
                        GiaiDoan = XMLUtils.BindData("GiaiDoan").Where(x => x.value == dbModel.GiaiDoan.ToString()).FirstOrDefault().text,
                        CongDoan = XMLUtils.BindData(str).Where(x => x.value == dbModel.CongDoan.ToString()).FirstOrDefault().text,
                    };
                }
                else return new HoSoVuAnViewModel();
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("TimHoSo with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }
        public HoSoVuAnViewModel ChiTietHuy(int hoSoVuAnID)
        {
            try
            {
                var dbModel = HuyHoSoDA.ChiTietHuy(hoSoVuAnID);
                if (dbModel != null)
                {
                    string str = "CongDoanHoSo";
                    if (dbModel.GiaiDoan == "2")
                        str = "CongDoanHoSoPhucTham";
                    return new HoSoVuAnViewModel()
                    {
                        HoSoVuAnID = dbModel.HoSoVuAnID,
                        SoHoSo = dbModel.SoHoSo,
                        MaHoSo = dbModel.MaHoSo,
                        TenVuAn = dbModel.TenVuAn,
                        NhomAn = SettingDataService.ListSettingDataItem("DINHNGHIACHUNG_NHOMANTOANAN").Where(x => x.ItemRemark == dbModel.NhomAn).FirstOrDefault().ItemText,
                        ToaAn = dbModel.ToaAn,
                        GiaiDoan = XMLUtils.BindData("GiaiDoan").Where(x => x.value == dbModel.GiaiDoan.ToString()).FirstOrDefault().text,
                        CongDoan = XMLUtils.BindData(str).Where(x => x.value == dbModel.CongDoan.ToString()).FirstOrDefault().text,
                        LyDoHuy = dbModel.LyDoHuy,
                        NguoiHuy = dbModel.NguoiHuy,
                        NgayHuy = dbModel.NgayHuy.ToString()
                    };
                }
                else return new HoSoVuAnViewModel();
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("ChiTietHuy with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }
        public ResponseResult HuyHoSo(int hoSoVuAnID, string lyDoHuy, string nguoiHuy)
        {
            try
            {
                var result = HuyHoSoDA.HuyHoSo(hoSoVuAnID, lyDoHuy, nguoiHuy);
                return result;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("HuyHoSo with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }
        public ResponseResult SuaHuyHoSo(int hoSoVuAnID, string lyDoHuy)
        {
            try
            {
                var result = HuyHoSoDA.SuaHuyHoSo(hoSoVuAnID, lyDoHuy);
                return result;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("SuaHuyHoSo with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }
        
    }
}