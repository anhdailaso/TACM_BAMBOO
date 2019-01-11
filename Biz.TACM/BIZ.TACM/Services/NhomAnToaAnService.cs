using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Biz.Lib.SettingData;
using Biz.Lib.TACM.NhomAnToaAn.IDataAccess;
using Biz.Lib.TACM.NhomAnToaAn.DataAccess;
using Biz.Lib.TACM.NhomAnToaAn.Model;
using Biz.TACM.IServices;
using Biz.Lib.Helpers;
using Biz.Lib.Authentication;

namespace Biz.TACM.Services
{
    public class NhomAnToaAnService : INhomAnToaAnService
    {
        private INhomAnToaAnDataAccess _nhomAnToaAnDA;

        private INhomAnToaAnDataAccess NhomAnToaAnDA
        {
            get { return _nhomAnToaAnDA ?? (_nhomAnToaAnDA = new NhomAnToaAnDataAccess()); }
        }
        private ISettingDataService _settingDataService;
        private ISettingDataService SettingDataService => _settingDataService ?? (_settingDataService = new SettingDataService());

        public IEnumerable<NhomAnModel> NhomAnTheoToaAn(int ToaAnID)
        {
            try
            {
                var dbModel = NhomAnToaAnDA.NhomAnTheoToaAn(ToaAnID);
                return dbModel;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("NhomAnTheoToaAn with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public ResponseResult ChonNhomAn(int ToaAnID, string MaNhomAn, string TenNhomAn)
        {
            ResponseResult result = null;
            try
            {
                result = NhomAnToaAnDA.ChonNhomAn(ToaAnID, MaNhomAn, TenNhomAn, SettingDataService.GetNhanVienSessionInfo().MaNV);
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("ChonNhomAn with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
            return result;
        }
        public ResponseResult HuyChonNhomAn(int NhomAnID)
        {
            ResponseResult result = null;
            try
            {
                result = NhomAnToaAnDA.HuyChonNhomAn(NhomAnID);
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("HuyChonNhomAn with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
            return result;
        }
    }
}