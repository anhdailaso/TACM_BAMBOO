using System;
using Biz.Lib.TACM.PhanCongNhanVien.Model;
using Biz.Lib.TACM.PhanCongNhanVien.DataAccess;
using Biz.Lib.TACM.PhanCongNhanVien.IDataAccess;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biz.TACM.IServices;
using Biz.Lib.Helpers;
using Biz.Lib.Authentication;

namespace Biz.TACM.Services
{
    public class PhanCongNhanVienService : IPhanCongNhanVienService
    {
        private IPhanCongNhanVienDataAccess _phanCongNhanVienDA;

        private IPhanCongNhanVienDataAccess PhanCongNhanVienDA
        {
            get { return _phanCongNhanVienDA ?? (_phanCongNhanVienDA = new PhanCongNhanVienDataAccess()); }
        }
        private ISettingDataService _settingDataService;
        private ISettingDataService SettingDataService => _settingDataService ?? (_settingDataService = new SettingDataService());

        public IEnumerable<NhomAnNhanVienModel> DSNhomAn(int ToaAnID)
        {
            try
            {
                var dbModel = PhanCongNhanVienDA.DSNhomAn(ToaAnID);
                return dbModel;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("DSNhomAn with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public IEnumerable<NhanVienNhomAnModel> DSNhanVienNhomAn(int NhomAnID)
        {
            try
            {
                var dbModel = PhanCongNhanVienDA.DSNhanVienNhomAn(NhomAnID);
                return dbModel;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("DSNhanVienTheoNhomAn with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }
        public IEnumerable<NhanVienModel> DSNhanVienTheoChucDanh(int ToaAnID, int? SoDoToChucID)
        {
            try
            {
                var dbModel = PhanCongNhanVienDA.DSNhanVienTheoChucDanh(ToaAnID, SoDoToChucID);
                return dbModel;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("DSNhanVienTheoNhomAnVaChucDanh with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }
        public ResponseResult ChonNhomAnNV(int NhanVienID, int NhomAnID)
        {
            ResponseResult result = null;
            try
            {
                result = PhanCongNhanVienDA.ChonNhomAnNV(NhanVienID, NhomAnID, SettingDataService.GetNhanVienSessionInfo().MaNV);
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("ChonNhomAnNV with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
            return result;
        }

        public ResponseResult HuyChonNhomAnNV_log(int NhanVienNhomAnID, int NhanVienID, int NhomAnID)
        {
            ResponseResult result = null;
            try
            {
                result = PhanCongNhanVienDA.HuyChonNhomAnNV_log(NhanVienNhomAnID, NhanVienID, NhomAnID, SettingDataService.GetNhanVienSessionInfo().MaNV);
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("HuyChonNhomAnNV_log with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
            return result;
        }
    }
}
