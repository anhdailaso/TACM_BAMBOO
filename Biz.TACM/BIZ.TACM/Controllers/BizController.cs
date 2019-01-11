using System;
using Biz.Lib.Helpers;
using Biz.TACM.Models.Model;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Biz.Lib.Authentication;
using Biz.Lib.SettingData.Model;
using Biz.TACM.IServices;
using Biz.TACM.Services;
using Biz.TACM.UtilityHelpers;
using ReportManagement;

namespace Biz.TACM.Controllers
{
    //[CustomAuthorize]
    public class BizController : PdfViewController
    {
        private ISettingDataService _settingDataService;
        private ISettingDataService SettingDataService => _settingDataService ?? (_settingDataService = new SettingDataService());

        public void UpdateAnSessionInfo(int? idToaAn = 0, string maNhomAn = null, int? idGiaiDoan = 0, int? idCongDoan = 0, int? idVuAn = 0, string maToaAn = null)
        {
            var an = Sessions.GetObject<AnSessionModel>("AnSessionObject");
            if (an == null)
                an = new AnSessionModel();

            an.ToaAnId = idToaAn ?? 0;
            an.MaNhomAn = maNhomAn ?? string.Empty;
            an.GiaiDoanId = idGiaiDoan ?? 0;
            an.CongDoanId = idCongDoan ?? 0;
            an.VuAnId = idVuAn ?? 0;
            an.MaToaAn = maToaAn ?? string.Empty;
            Sessions.AddObject<AnSessionModel>("AnSessionObject", an);
        }

        public AnSessionModel GetAnSessionInfo()
        {
            var an = Sessions.GetObject<AnSessionModel>("AnSessionObject");
            if (an == null)
                an = new AnSessionModel();

            return an;
        }
        
        public ActionResult KiemTraQuyenNhanVien(int hoSoVuAnId, string contrCheck, string actionCheck)
        {
            //kiem tra quyen nhan vien o trang chi tiet cong doan
            var roleEdit = AuthorizeNhanVien(hoSoVuAnId, contrCheck, actionCheck);
            var role = roleEdit ? Status.Active.GetHashCode() : Status.Inactive.GetHashCode();

            return Json(new { role = role }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult KiemTraQuyenNhanVienAction(string contrCheck, string actionCheck)
        {
            //kiem tra quyen nhan vien o trang chinh
            var roleEdit = IsInRoleAction(contrCheck, actionCheck);
            var role = roleEdit ? Status.Active.GetHashCode() : Status.Inactive.GetHashCode();

            return Json(new { role = role }, JsonRequestBehavior.AllowGet);
        }

        public bool AuthorizeNhanVien(int hoSoVuAnId, string controller, string action)
        {
            if (!AccountUtils.IsInRole(controller, action) || !IsInRoleHoSoVuAn(AccountUtils.CurrentUsername(), hoSoVuAnId))
            {
                return false;
            }

            return true;
        }

        public bool IsInRoleAction(string controller, string action)
        {
            if (!AccountUtils.IsInRole(controller, action))
            {
                return false;
            }

            return true;
        }

        public bool IsInRoleHoSoVuAn(string maNV, int hoSoVuAnId)
        {          
            var vuAnList = Sessions.GetObject<List<HoSoVuAnModel>>("AnRoleObject");
            if (vuAnList != null && vuAnList.Count > 0)
            {
                //good
            }
            else
            {
                //load from database
                vuAnList = SettingDataService.DanhSachHoSoVuAnIsInRole(maNV);

                //add back to session
                if (vuAnList != null && vuAnList.Count > 0)
                {
                    Sessions.AddObject<List<HoSoVuAnModel>>("AnRoleObject", vuAnList);
                }
                else
                {
                    vuAnList = new List<HoSoVuAnModel>();
                }
            }

            var inRole = vuAnList.FirstOrDefault(x => x.HoSoVuAnID == hoSoVuAnId);

            if (inRole != null)
                    return true;
                return false;
        }

        public void Success(string message)
        {
            if (TempData[Alerts.Success] != null)
            {
                TempData[Alerts.Success] = message;
            }
            else
            {
                TempData.Add(Alerts.Success, message);
            }
        }

        public void Warning(string message)
        {
            if (TempData[Alerts.Warning] != null)
            {
                TempData[Alerts.Warning] = message;
            }
            else
            {
                TempData.Add(Alerts.Warning, message);
            }
        }

        public void Error(string message)
        {
            if (TempData[Alerts.Error] != null)
            {
                TempData[Alerts.Error] = message;
            }
            else
            {
                TempData.Add(Alerts.Error, message);
            }
        }

        public static class Alerts
        {
            public const string Success = "success";
            public const string Error = "danger";
            public const string Warning = "warning";

            public static readonly IEnumerable<string> AllAlerts = new[] { Success, Error, Warning };
        }

        public ActionResult UploadDocument(DropzoneOptions model)
        {
            return PartialView("_UploadDocument", model);
        }
    }
}