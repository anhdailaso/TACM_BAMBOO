using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Biz.Lib.Helpers;
using Biz.Lib.SettingData.DataAccess;
using Biz.Lib.Authentication;
using Biz.Lib.SettingData.Model;
using Biz.TACM.IServices;
using Microsoft.Ajax.Utilities;
using System.Data.Entity.Infrastructure;
using System.Text.RegularExpressions;
using Biz.TACM.Models.Model;

namespace Biz.TACM.Services
{
    public class SettingDataService : ISettingDataService
    {
        private ISettingDataAccess _settingDataAccess;
        public ISettingDataAccess SettingDataAccess => _settingDataAccess ?? (_settingDataAccess = new SettingDataAccess());

        public List<SettingItemModel> ListSettingDataItem(string groupName)
        {
            try
            {
                var dbModel = SettingDataAccess.GetDataItem(groupName);
                return dbModel;
            }
            catch (Exception ex)
            {
                WriteLog.Error($"SettingDataItem with error [{ex.ToString()}]", AppName.BizSecurity);
                return null;
            }
        }

        public SelectList SettingDataItem(string groupName, string selected)
        {
            try
            {
                var dbModel = SettingDataAccess.GetDataItem(groupName);
                var listItem = dbModel.Select(
                    x => new SelectListItem
                    {
                        Text = x.ItemText,
                        Value = x.ItemValue
                    });
                return new SelectList(listItem, "Text", "Text", selected);
            }
            catch (Exception ex)
            {
                WriteLog.Error($"SettingDataItem with error [{ex.ToString()}]", AppName.BizSecurity);
                return null;
            }
        }

        public SelectList SettingDataItemXML(string groupName, string selectedValue)
        {
            var list = XMLUtils.BindData(groupName);
            return new SelectList(list, "Value", "Text", selectedValue);
        }
        
        public SelectList DanhSachNhanVienTheoChucDanh(string chucDanh, int? toaAnId, string selected)
        {
            try
            {
                var listNhanVien = SettingDataAccess.DanhSachNhanVienTheoChucDanh(chucDanh, toaAnId);
                var list_ttv = new List<NhanVienModel>();
                if (chucDanh.ToLower() == "thư ký")
                    list_ttv = SettingDataAccess.DanhSachNhanVienTheoChucDanh("Thẩm tra viên", toaAnId);
                listNhanVien.AddRange(list_ttv);
                if (!selected.IsNullOrWhiteSpace())
                {
                    foreach (var item in listNhanVien)
                    {
                        if (item.HoTenVaMaNV == selected)
                        {
                            selected = item.MaNV;
                            break;
                        }
                    }
                }
                var listItem = listNhanVien.Select(
                    x => new SelectListItem
                    {
                        Text = x.HoTenVaMaNV,
                        Value = x.MaNV,
                    }).ToList();
                return new SelectList(listItem, "Value", "Text", selected);
            }
            catch (Exception ex)
            {
                WriteLog.Error($"DanhSachNhanVienTheoChucDanh with error [{ex.ToString()}]", AppName.BizSecurity);
                return null;
            }
        }

        public List<NhanVienModel> DanhSachNhanVienTheoChucDanhSelected(string chucDanh, int toaAnId, List<string> selected)
        {
            try
            {
                var listNhanVien = SettingDataAccess.DanhSachNhanVienTheoChucDanh(chucDanh, toaAnId);
                var list_ttv=new List<NhanVienModel>();
                var list = new List<NhanVienModel>();
                if(chucDanh.ToLower()=="thư ký")
                    list_ttv= SettingDataAccess.DanhSachNhanVienTheoChucDanh("Thẩm tra viên", toaAnId);
                foreach (var item in listNhanVien)
                {
                    bool check = selected != null && selected.Contains(item.HoTenVaMaNV);
                    item.Checked = check;
                    list.Add(item);
                }
                foreach (var item in list_ttv)
                {
                    bool check = selected != null && selected.Contains(item.HoTenVaMaNV);
                    item.Checked = check;
                    list.Add(item);
                }
                return list;
            }
            catch (Exception ex)
            {
                WriteLog.Error($"DanhSachNhanVienTheoChucDanhSelected with error [{ex.ToString()}]", AppName.BizSecurity);
                return null;
            }
        }

        public SelectList DanhSachNhanVienTheoTenChucNangNghiepVu(string tenChucNang, int toaAnId, string selected)
        {
            try
            {
                var dbModel = SettingDataAccess.DanhSachNhanVienTheoTenChucNangNghiepVu(tenChucNang, toaAnId);
                var listItem = dbModel.Select(
                    x => new SelectListItem
                    {
                        Text = x.HoTenVaMaNV,
                        Value = x.MaNV
                    }
                );
                return new SelectList(listItem, "Value", "Text", selected);
            }
            catch (Exception ex)
            {
                WriteLog.Error($"DanhSachNhanVienTheoTenChucNangNghiepVu with error [{ex.ToString()}]", AppName.BizSecurity);
                return null;
            }
        }

        public NhanVienModel ChiTietNhanVienTheoMaNhanVien(string maNV)
        {
            try
            {
                var dbModel = new NhanVienModel();
                if (!string.IsNullOrEmpty(maNV))
                {
                    dbModel = SettingDataAccess.ChiTietNhanVienTheoMaNhanVien(maNV);
                }                
                return dbModel;
            }
            catch (Exception ex)
            {
                WriteLog.Error($"ChiTietNhanVienTheoMaNhanVien with error [{ex.ToString()}]", AppName.BizSecurity);
                return null;
            }
        }
        
        public NhanVienModel GetSessionInfoNhanVienTheoMaNhanVien(string maNV)
        {
            try
            {
                var dbModel = SettingDataAccess.GetSessionInfoNhanVienTheoMaNhanVien(maNV);
                return dbModel;
            }
            catch (Exception ex)
            {
                WriteLog.Error($"GetSessionInfoNhanVienTheoMaNhanVien with error [{ex.ToString()}]", AppName.BizSecurity);
                return null;
            }
        }

        public List<HoSoVuAnModel> DanhSachHoSoVuAnIsInRole(string maNV)
        {
            try
            {
                var list = SettingDataAccess.DanhSachHoSoVuAnIsInRole(maNV);
                return list;
            }
            catch (Exception ex)
            {
                WriteLog.Error($"DanhSachHoSoVuAnIsInRole with error [{ex.ToString()}]", AppName.BizSecurity);
                return null;
            }
        }

        public ToaAnModel GetToaAnTheoMaNhanVien(string maNV)
        {
            try
            {
                var model = SettingDataAccess.GetToaAnTheoMaNhanVien(maNV);
                return model;
            }
            catch (Exception ex)
            {
                WriteLog.Error($"GetToaAnTheoMaNhanVien with error [{ex.ToString()}]", AppName.BizSecurity);
                return null;
            }
        }

        public ToaAnModel GetToaAnTheoToaAnID(int toaAnID)
        {
            try
            {
                var model = SettingDataAccess.GetToaAnTheoToaAnID(toaAnID);
                return model;
            }
            catch (Exception ex)
            {
                WriteLog.Error($"GetToaAnTheoToAnID with error [{ex.ToString()}]", AppName.BizSecurity);
                return null;
            }
        }

        public ToaAnModel GetToaAnTheoTenToaAn(string tenToaAn)
        {
            try
            {
                var model = SettingDataAccess.GetToaAnTheoTenToaAn(tenToaAn);
                return model;
            }
            catch (Exception ex)
            {
                WriteLog.Error($"GetToaAnTheoTenToaAn with error [{ex.ToString()}]", AppName.BizSecurity);
                return null;
            }
        }

        public List<ToaAnModel> DanhSachToaAn()
        {
            try
            {
                var listToaAn = SettingDataAccess.DanhSachToaAn();
                return listToaAn;
            }
            catch (Exception ex)
            {
                WriteLog.Error($"DanhSachToaAn with error [{ex.ToString()}]", AppName.BizSecurity);
                return null;
            }
        }

        public SelectList SelectListDanhSachToaAn(string selectedValue)
        {
            try
            {
                var dbModel = SettingDataAccess.DanhSachToaAn();
                var listItem = dbModel.Select(
                    x => new SelectListItem
                    {
                        Text = x.TenToaAn,
                        Value = x.MaToaAn
                    });
                return new SelectList(listItem, "Value", "Text", selectedValue);
            }
            catch (Exception ex)
            {
                WriteLog.Error($"DanhSachToaAn with error [{ex.ToString()}]", AppName.BizSecurity);
                return null;
            }
        }

        public SelectList SelectListDanhSachToaAnValueIsToaAnID(int? selectedValue)
        {
            try
            {
                var dbModel = SettingDataAccess.DanhSachToaAn();
                if(dbModel.Count>1)
                    dbModel = SortToaAn(dbModel);
                var listItem = dbModel.Select(
                    x => new SelectListItem
                    {
                        Text = x.TenToaAn,
                        Value = x.ToaAnID.ToString()
                    });
                return new SelectList(listItem, "Value", "Text", selectedValue);
            }
            catch (Exception ex)
            {
                WriteLog.Error($"SelectListDanhSachToaAnValueIsToaAnID with error [{ex.ToString()}]", AppName.BizSecurity);
                return null;
            }
        }

        public SelectList SelectListDanhSachToaAnValueIsTenToaAn(string selectedValue)
        {
            try
            {
                var dbModel = SettingDataAccess.DanhSachToaAn();
                if (dbModel.Count > 1)
                    dbModel = SortToaAn(dbModel);
                var listItem = dbModel.Select(
                    x => new SelectListItem
                    {
                        Text = x.TenToaAn,
                        Value = x.TenToaAn
                    });
                return new SelectList(listItem, "Value", "Text", selectedValue);
            }
            catch (Exception ex)
            {
                WriteLog.Error($"SelectListDanhSachToaAnValueIsTenToaAn with error [{ex.ToString()}]", AppName.BizSecurity);
                return null;
            }
        }

        public SelectList DanhSachDiaChiCoQuanThiHanhAn(string groupName, string selected)
        {
            try
            {
                var dbModel = SettingDataAccess.GetDataItem(groupName);
                var listItem = dbModel.Select(
                    x => new SelectListItem
                    {
                        Text = x.ItemRemark,
                        Value = x.ItemText
                    });
                return new SelectList(listItem, "Value", "Text", selected);
            }
            catch (Exception ex)
            {
                WriteLog.Error($"DanhSachDiaChiCoQuanThiHanhAn with error [{ex.ToString()}]", AppName.BizSecurity);
                return null;
            }
        }

        public List<NhomAnModel> DanhSachNhomAnTheoToaAn(int toaAnId)
        {
            try
            {
                var listNhomAn = SettingDataAccess.DanhSachNhomAnTheoToaAn(toaAnId);
                return listNhomAn;
            }
            catch (Exception ex)
            {
                WriteLog.Error($"DanhSachNhomAnTheoToaAn with error [{ex.ToString()}]", AppName.BizSecurity);
                return null;
            }
        }

        public SelectList DanhSachNhomAnChoDDL(string groupName, string selected)
        {
            try
            {
                var dbModel = SettingDataAccess.GetDataItem(groupName);
                var listItem = dbModel.Select(
                    x => new SelectListItem
                    {
                        Text = x.ItemText,
                        Value = x.ItemRemark
                    });
                return new SelectList(listItem, "Value", "Text", selected);
            }
            catch (Exception ex)
            {
                WriteLog.Error($"DanhSachNhomAnChoDDL with error [{ex.ToString()}]", AppName.BizSecurity);
                return null;
            }
        }

        public SelectList SettingDataItemHaveValueText(string groupName, string selected)
        {
            try
            {
                var dbModel = SettingDataAccess.GetDataItem(groupName);
                var listItem = dbModel.Select(
                    x => new SelectListItem
                    {
                        Text = x.ItemText,
                        Value = x.ItemValue
                    });
                return new SelectList(listItem, "Value", "Text", selected);
            }
            catch (Exception ex)
            {
                WriteLog.Error($"SettingDataItem with error [{ex.ToString()}]", AppName.BizSecurity);
                return null;
            }
        }

        public string GetLoaiQuanHe(int hoSoVuAnId)
        {
            try
            {
                var dbModel = SettingDataAccess.GetLoaiQuanHe(hoSoVuAnId);
                return dbModel;
            }
            catch (Exception ex)
            {
                WriteLog.Error($"GetLoaiQuanHe with error [{ex.ToString()}]", AppName.BizSecurity);
                return null;
            }
        }

        public List<ToaAnModel> DanhSachToaAnTheoNV(string maNV)
        {
            try
            {
                var dbModel = SettingDataAccess.DanhSachToaAnTheoNV(maNV);
                if (dbModel.Count > 1)
                    dbModel = SortToaAn(dbModel);
                return dbModel;
            }
            catch (Exception ex)
            {
                WriteLog.Error($"DanhSachToaAnTheoNV with error [{ex.ToString()}]", AppName.BizSecurity);
                return null;
            }
        }
        private List<ToaAnModel> SortToaAn(List<ToaAnModel> dbModel)
        {
            var tinh = dbModel.Where(x => x.ToaAnID == 10).FirstOrDefault();
            var tp = dbModel.Where(x => x.ToaAnID == 1).FirstOrDefault();
            var index = dbModel.FindIndex(x => x.ToaAnID == 10);
            if (tinh != null && tp != null) 
            {
                var temp = dbModel[0];
                dbModel[0] = dbModel[index];
                dbModel[index] = temp;
                temp = dbModel[1];
                dbModel[1] = dbModel[index];
                dbModel[index] = temp;
                dbModel = dbModel.OrderBy(x => x.ToaAnID != 1 && x.ToaAnID != 10).ThenBy(i => i.TenToaAn).ToList();
            }
            else if (tinh == null && tp != null)
            {
                dbModel = dbModel.OrderBy(x => x.ToaAnID != 1).ThenBy(i => i.TenToaAn).ToList();
            }
            else if (tinh != null && tp == null)
            {
                var temp = dbModel[0];
                dbModel[0] = dbModel[index];
                dbModel[index] = temp;
                dbModel = dbModel.OrderBy(x => x.ToaAnID != 10).ThenBy(i => i.TenToaAn).ToList();
            }
            else
                dbModel = dbModel.OrderBy(i => i.TenToaAn).ToList();
            return dbModel;
        }

        public SelectList SelectListDanhSachToaAnValueIsToaAnIDTheoNV(int? selectedValue, string maNV)
        {
            try
            {
                var dbModel = SettingDataAccess.DanhSachToaAnTheoNV(maNV);
                if (dbModel.Count > 1)
                    dbModel = SortToaAn(dbModel);
                
                var listItem = dbModel.Select(
                    x => new SelectListItem
                    {
                        Text = x.TenToaAn,
                        Value = x.ToaAnID.ToString()
                    });
                return new SelectList(listItem, "Value", "Text", selectedValue);
            }
            catch (Exception ex)
            {
                WriteLog.Error($"SelectListDanhSachToaAnValueIsToaAnID with error [{ex.ToString()}]", AppName.BizSecurity);
                return null;
            }
        }

        public List<NhanVienModel> DanhSachThamPhanDuKhuyetTheoHoSoVuAnId(int hoSoVuAnId)
        {
            try
            {
                var dbModel = SettingDataAccess.DanhSachThamPhanDuKhuyetTheoHoSoVuAnId(hoSoVuAnId);
                return dbModel;
            }
            catch (Exception ex)
            {
                WriteLog.Error($"DanhSachThamPhanDuKhuyetTheoHoSoVuAnId with error [{ex.ToString()}]", AppName.BizSecurity);
                return null;
            }
        }

        public List<NhanVienModel> DanhSachHoiThamNhanDanDuKhuyetTheoHoSoVuAnId(int hoSoVuAnId)
        {
            try
            {
                var dbModel = SettingDataAccess.DanhSachHoiThamNhanDanDuKhuyetTheoHoSoVuAnId(hoSoVuAnId);
                return dbModel;
            }
            catch (Exception ex)
            {
                WriteLog.Error($"DanhSachHoiThamNhanDanDuKhuyetTheoHoSoVuAnId with error [{ex.ToString()}]", AppName.BizSecurity);
                return null;
            }
        }

        public List<NhanVienModel> DanhSachThuKyDuKhuyetTheoHoSoVuAnId(int hoSoVuAnId)
        {
            try
            {
                var dbModel = SettingDataAccess.DanhSachThuKyDuKhuyetTheoHoSoVuAnId(hoSoVuAnId);
                return dbModel;
            }
            catch (Exception ex)
            {
                WriteLog.Error($"DanhSachThuKyDuKhuyetTheoHoSoVuAnId with error [{ex.ToString()}]", AppName.BizSecurity);
                return null;
            }
        }

        public List<NhanVienModel> DanhSachKiemSatVienDuKhuyetTheoHoSoVuAnId(int hoSoVuAnId)
        {
            try
            {
                var dbModel = SettingDataAccess.DanhSachKiemSatVienDuKhuyetTheoHoSoVuAnId(hoSoVuAnId);
                return dbModel;
            }
            catch (Exception ex)
            {
                WriteLog.Error($"DanhSachKiemSatVienDuKhuyetTheoHoSoVuAnId with error [{ex.ToString()}]", AppName.BizSecurity);
                return null;
            }
        }

        //For So Ban An, So Quyet Dinh, So Thu Ly
        public int ParseCacLoaiSoToInt(string strNumber)
        {
            return int.Parse(Regex.Replace(strNumber, "[^0-9]+", string.Empty));
        }

        //public List<string> DanhSachSettingItemSelected(string groupName, List<string> selected)
        //{
        //    try
        //    {
        //        var listItem = SettingDataAccess.GetDataItem(groupName);
        //        var list = new List<string>();

        //        foreach (var item in listItem)
        //        {
        //            bool check = selected != null && selected.Contains(item.ItemText);
        //            item.Checked = check;
        //            list.Add(item);
        //        }
        //        return list;
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteLog.Error($"DanhSachNhanVienTheoChucDanhSelected with error [{ex.ToString()}]", AppName.BizSecurity);
        //        return null;
        //    }
        //}
        public SelectList SelectListLanThu(int lanThu)
        {
            var list = new List<SelectListItem>();

            for (int i = 1; i <= (lanThu + 1); i++)
            {
                list.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }

            return new SelectList(list, "Value", "Text", list);
        }

        //session nhanvien
        public void UpdateNhanVienSessionInfo(int? nhanVienID, string maNV, string maNVMoi, string hoNV, string tenLotNV, string tenNV, string hoVaTen, string pathImage)
        {
            var nhanVien = Sessions.GetObject<NhanVienSessionModel>("NhanVienSessionObject");
            if (nhanVien == null)
                nhanVien = new NhanVienSessionModel();
            nhanVien.NhanvienID = nhanVienID ?? 0;
            nhanVien.MaNV = maNV ?? string.Empty;
            nhanVien.MaNVMoi = maNVMoi ?? string.Empty;
            nhanVien.HoNV = hoNV ?? string.Empty;
            nhanVien.TenLotNV = tenLotNV ?? string.Empty;
            nhanVien.TenNV = tenNV ?? string.Empty;
            nhanVien.HoVaTen = hoVaTen ?? string.Empty;
            nhanVien.DuongDanHinhDaiDien = pathImage ?? string.Empty;
            Sessions.AddObject<NhanVienSessionModel>("NhanVienSessionObject", nhanVien);
        }

        public NhanVienSessionModel GetNhanVienSessionInfo()
        {
            var nhanVien = Sessions.GetObject<NhanVienSessionModel>("NhanVienSessionObject");
            if (nhanVien == null)
                nhanVien = new NhanVienSessionModel();

            return nhanVien;
        }
    }
}