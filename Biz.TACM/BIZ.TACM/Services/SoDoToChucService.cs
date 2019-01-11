using Biz.TACM.IServices;
using Biz.TACM.Services;
using Biz.TACM.Models.ViewModel.SoDoToChuc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Biz.Lib.TACM.SoDoToChuc.DataAccess;
using Biz.Lib.TACM.SoDoToChuc.IDataAccess;
using Biz.Lib.TACM.SoDoToChuc.Models;
using Biz.Lib.Helpers;
using System.Web.Mvc;
using Biz.Lib.Authentication;

namespace Biz.TACM.Services
{
    public class SoDoToChucService : ISoDoToChucService
    {
        private ISoDoToChucDataAccess _sodotochucDataAccess;
        private ISoDoToChucDataAccess SoDoToChucDataAccess => _sodotochucDataAccess ?? (_sodotochucDataAccess = new SoDoToChucDataAccess());

        private ISettingDataService _settingDataService;
        private ISettingDataService SettingDataService => _settingDataService ?? (_settingDataService = new SettingDataService());
        //private ISoDoToChucDataAccess _chucdanhDA;

        //private ISoDoToChucDataAccess ChucDanhDA
        //{
        //    get { return _chucdanhDA ?? (_chucdanhDA = new SoDoToChucDataAccess()); }
        //}
        //public object ResponResult { get; private set; }

        public IEnumerable<ChucDanhViewModel> DanhSachChucDanhTheoToaAn(int? toaAnId)
        {
            var dbModel = SoDoToChucDataAccess.DanhSachChucDanhTheoToaAn(toaAnId);
            dbModel = dbModel.Where(x => x.Loai != 2).ToList();
            List<ChucDanhViewModel> danhSach = dbModel.Select(s => new ChucDanhViewModel()
            {
                SoDoToChucID = s.SoDoToChucID,
                ChucDanh = s.ChucDanh,
                ChucDanhChaID = s.ChucDanhChaID,
                ToaAnID = s.ToaAnID
            }).ToList();
            return PreOrderTreeSoDoToChuc(danhSach);
        }
        public IEnumerable<ChucDanhViewModel> DanhSachChucVuTheoToaAn(int? toaAnId)
        {
            var dbModel = SoDoToChucDataAccess.DanhSachChucDanhTheoToaAn(toaAnId);
            dbModel = dbModel.Where(x => x.Loai == 2).ToList();
            List<ChucDanhViewModel> danhSach = dbModel.Select(s => new ChucDanhViewModel()
            {
                SoDoToChucID = s.SoDoToChucID,
                ChucDanh = s.ChucDanh,
                ChucDanhChaID = s.ChucDanhChaID,
                ToaAnID = s.ToaAnID
            }).ToList();
            return PreOrderTreeSoDoToChuc(danhSach);
        }
        public ChucDanhViewModel ChiTietChucDanhTheoId(int? soDoToChucId) //chucDanhId
        {
            try
            {
                var dbModel = SoDoToChucDataAccess.ChiTietChucDanhTheoId(soDoToChucId);
                return new ChucDanhViewModel
                {
                    SoDoToChucID = dbModel.SoDoToChucID,
                    ChucDanh = dbModel.ChucDanh,
                    ChucDanhChaID = dbModel.ChucDanhChaID,
                    ChucDanhCha = dbModel.ChucDanhCha,
                    ToaAnID = dbModel.ToaAnID,
                    DienGiaiNghiepVu = dbModel.DienGiaiNghiepVu,
                    NguoiTao = dbModel.NguoiTao,
                    TrangThai = dbModel.TrangThai,
                    Loai=dbModel.Loai,
                    NgayTao = dbModel.NgayTao.ToString("HH:mm:ss, dd'/'MM'/'yyyy"),
                    GhiChu = dbModel.GhiChu
                };
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("ChiTietChucDanhTheoId with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return new ChucDanhViewModel();
            }
        }

        public List<ChucDanhViewModel> PreOrderTreeSoDoToChuc(List<ChucDanhViewModel> danhSach)
        {
            List<ChucDanhViewModel> temp = new List<ChucDanhViewModel>();
            List<ChucDanhViewModel> leftOver = danhSach.ToList();

            for (int i = 0; i < danhSach.Count; i++)
            {
                if (danhSach.ElementAt(i).ChucDanhChaID == 0 || danhSach.ElementAt(i).ChucDanhChaID == null)
                {
                    temp.Add(danhSach.ElementAt(i));
                    leftOver.Remove(danhSach.ElementAt(i));
                    FindChildrent(temp, leftOver, danhSach.ElementAt(i).SoDoToChucID);
                }
            }
            return temp;
        }
        public void FindChildrent(List<ChucDanhViewModel> temp, List<ChucDanhViewModel> danhSach, int id)
        {
            List<ChucDanhViewModel> leftOver = danhSach.ToList();
            for (int j = 0; j < danhSach.Count; j++)
            {
                if (danhSach.ElementAt(j).ChucDanhChaID == id)
                {
                    temp.Add(danhSach.ElementAt(j));
                    leftOver.Remove(danhSach.ElementAt(j));
                    FindChildrent(temp, leftOver, danhSach.ElementAt(j).SoDoToChucID);
                }
            }
        }

        public SelectList DanhSachChucDanhCha(string selectedValue, int? toaAnId)
        {
            try
            {
                var dbModel = SoDoToChucDataAccess.DanhSachChucDanhTheoToaAn(toaAnId).Where(x=>x.Loai!=2);
                //
                List<ChucDanhViewModel> danhSach = dbModel.Select(s => new ChucDanhViewModel()
                {
                    SoDoToChucID = s.SoDoToChucID,
                    ChucDanh = s.ChucDanh,
                    ChucDanhChaID = s.ChucDanhChaID,
                    ToaAnID = s.ToaAnID
                }).ToList();
                List<ChucDanhViewModel> ds = PreOrderTreeSoDoToChuc(danhSach);
                //
                var listItem = ds.Select(
                    x => new SelectListItem
                    {
                        Text = x.ChucDanh,
                        Value = x.SoDoToChucID.ToString()
                    });
                return new SelectList(listItem, "Value", "Text", selectedValue);
            }
            catch (Exception ex)
            {
                WriteLog.Error($"DanhSachToaAn with error [{ex.ToString()}]", AppName.BizSecurity);
                return null;
            }
        }
        public SelectList DanhSachChucVuCha(string selectedValue, int? toaAnId)
        {
            try
            {
                var dbModel = SoDoToChucDataAccess.DanhSachChucDanhTheoToaAn(toaAnId).Where(x => x.Loai == 2);
                //
                List<ChucDanhViewModel> danhSach = dbModel.Select(s => new ChucDanhViewModel()
                {
                    SoDoToChucID = s.SoDoToChucID,
                    ChucDanh = s.ChucDanh,
                    ChucDanhChaID = s.ChucDanhChaID,
                    ToaAnID = s.ToaAnID
                }).ToList();
                List<ChucDanhViewModel> ds = PreOrderTreeSoDoToChuc(danhSach);
                //
                var listItem = ds.Select(
                    x => new SelectListItem
                    {
                        Text = x.ChucDanh,
                        Value = x.SoDoToChucID.ToString()
                    });
                return new SelectList(listItem, "Value", "Text", selectedValue);
            }
            catch (Exception ex)
            {
                WriteLog.Error($"DanhSachToaAn with error [{ex.ToString()}]", AppName.BizSecurity);
                return null;
            }
        }
        public ResponseResult XoaChucDanh(int soDoToChucId)
        {
            ResponseResult result = null;
            try
            {
                result = SoDoToChucDataAccess.XoaChucDanh(soDoToChucId);
            }
            catch(Exception ex)
            {
                WriteLog.Error(string.Format("XoaChucDanh with error [{0}]", ex.ToString()), AppName.BizSecurity);
                result = null;
            }
            return result;
        }

        public ResponseResult ThemChucDanh(ChucDanhViewModel viewModel)
        {
            ResponseResult result = null;
            try
            {
                viewModel.NguoiTao = SettingDataService.GetNhanVienSessionInfo().MaNV;
                var dbModel = new ChucDanhModel
                {
                    ToaAnID = viewModel.ToaAnID,
                    ChucDanh = viewModel.ChucDanh,
                    DienGiaiNghiepVu = viewModel.DienGiaiNghiepVu,
                    ChucDanhChaID = viewModel.ChucDanhChaID,
                    Loai=viewModel.Loai,
                    NguoiTao = viewModel.NguoiTao,
                    GhiChu = viewModel.GhiChu
                };
                result = SoDoToChucDataAccess.ThemChucDanh(dbModel);
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("Themchucdanh with error [{0}]", ex.ToString()), AppName.BizSecurity);
                throw;
            }
            return result;
        }

        public ResponseResult SuaChucDanh(ChucDanhViewModel viewModel)
        {
            ResponseResult result = null;
            try
            {
                viewModel.NguoiTao = SettingDataService.GetNhanVienSessionInfo().MaNV;
                var dbModel = new ChucDanhModel
                {
                    SoDoToChucID = viewModel.SoDoToChucID,
                    ToaAnID = viewModel.ToaAnID,
                    ChucDanh = viewModel.ChucDanh,
                    DienGiaiNghiepVu = viewModel.DienGiaiNghiepVu,
                    ChucDanhChaID = viewModel.ChucDanhChaID,
                    Loai=viewModel.Loai,
                    TrangThai = viewModel.TrangThai,
                    NguoiTao = viewModel.NguoiTao,
                    GhiChu = viewModel.GhiChu
                };
                result = SoDoToChucDataAccess.SuaChucDanh(dbModel);
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("Suachucdanh with error [{0}]", ex.ToString()), AppName.BizSecurity);
                throw;
            }
            return result;
        }
    }
}