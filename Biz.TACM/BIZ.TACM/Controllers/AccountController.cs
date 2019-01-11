using System;
using Biz.Lib.Helpers;
using System.Web.Mvc;
using Biz.Lib.Security.Interfaces;
using Biz.Lib.Security.Services;
using Biz.Lib.Authentication;
using Biz.Lib.TACM.Resources.Resources;
using Biz.TACM.Models.ViewModel.Shared;
using Biz.TACM.IServices;
using Biz.TACM.Services;
using Biz.TACM.Enums;
using Biz.TACM.Infrastructure.Attributes;
using Org.BouncyCastle.Utilities.Collections;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Biz.TACM.Controllers
{
    public class AccountController : BizController
    {
        ISecUserService _userService;
        ISecUserService UserService => _userService ?? (_userService = new SecUserService());
        private ISettingDataService _settingDataService;
        private ISettingDataService SettingDataService => _settingDataService ?? (_settingDataService = new SettingDataService());

        int CountLogin = 0;
        //int MaxCountLogin = ConfigValue.MAX_COUNT_LOGIN;

        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Login(string returnUrl)
        {
            if (Session["CountLogin"] == null)
            {
                Session["CountLogin"] = 0;
            }

            CountLogin = (int)Session["CountLogin"];

            if (CountLogin == 0)
            {
                ViewBag.IsHidden = "none";
                ViewBag.TempCapcha = 123456;
            }
            else
            {
                ViewBag.IsHidden = "block";
            }

            //var model = new UserLoginViewModel();
            //model.ReturnUrl = returnUrl;

            var filesPath = Directory.EnumerateFiles(Server.MapPath(Setting.FOLDER_SLIDESHOW_TOAANCAMAU), "*.*").Where(s => s.EndsWith(".jpg") || s.EndsWith(".png"));
            List<string> list = new List<string>();
            foreach (var filePath in filesPath)
            {
                string temp = Setting.FOLDER_SLIDESHOW_TOAANCAMAU + Path.GetFileName(filePath);
                list.Add(temp);
            }

            ViewBag.SlideShowFiles = list;

            return View(new UserLoginViewModel());
        }

        [CaptchaValidation("captcha")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(bool captchaValid, UserLoginViewModel userViewModel)
        {
            bool showCaptcha = false;
            if (Session["CountLogin"] == null)
            {
                Session["CountLogin"] = 0;
            }

            CountLogin = (int)Session["CountLogin"];

            if (CountLogin >= Int32.Parse(Setting.SOLAN_DANGNHAP_SAI))
            {
                if (!captchaValid)
                {
                    return Json(new JsonResponseViewModel(false, new[] {AccountMessage.CAPTCHA_INVALID}, showCaptcha = true));
                }
               
            }

            bool isFirstLogin = false;
            var isValid = AuthenService.Login(userViewModel.UserName, Cryptography.EncryptMD5(userViewModel.Password), userViewModel.RememberMe, ref isFirstLogin);
            if (isValid)
                
            {
                //reset count Login
                Session["CountLogin"] = 0;

                //update session
                var nhanVienInfo = SettingDataService.GetSessionInfoNhanVienTheoMaNhanVien(AccountUtils.CurrentUsername());
                var toaAn = SettingDataService.GetToaAnTheoMaNhanVien(AccountUtils.CurrentUsername());
                UpdateAnSessionInfo(idToaAn: toaAn.ToaAnID, maToaAn: toaAn.MaToaAn, idGiaiDoan: GiaiDoan.SoTham.GetHashCode());
                SettingDataService.UpdateNhanVienSessionInfo(nhanVienID: nhanVienInfo.NhanVienID, maNV: nhanVienInfo.MaNV, maNVMoi: nhanVienInfo.MaNVMoi, hoNV: nhanVienInfo.HoNV, tenLotNV: nhanVienInfo.TenLotNV, tenNV: nhanVienInfo.TenNV, hoVaTen: nhanVienInfo.HoVaTenNV, pathImage: nhanVienInfo.DuongDanHinhDaiDien);

                //log activity
                SaveLog.SaveSecLog(ActionGroup.SECURITY_ACCESS, ActionName.sec_user, userViewModel.UserName, LogStatus.Success, Messages.LOGIN_SUCCESS, userViewModel.UserName);
                if (!string.IsNullOrEmpty(Sessions.GetMessage(SessionKeyList.IsFirstLogin)) || isFirstLogin)
                {
                    return Json(JsonResponseViewModel.CreateSuccess("/Account/ChangeForgotPassword"));
                }
                else if (string.IsNullOrEmpty(userViewModel.ReturnUrl))
                {
                    if (nhanVienInfo.TrangThai == 0)
                        return Json(JsonResponseViewModel.CreateFail(NotifyMessage.THONGBAO_TAIKHOANKHONGHOATDONG));
                    return Json(JsonResponseViewModel.CreateSuccess("/Home/Index"));
                }
                else
                {
                    if (nhanVienInfo.TrangThai == 0)
                        return Json(JsonResponseViewModel.CreateFail(NotifyMessage.THONGBAO_TAIKHOANKHONGHOATDONG));
                    return Json(JsonResponseViewModel.CreateSuccess(userViewModel.ReturnUrl));
                }
            }
            else
            {
                Session["CountLogin"] = ++CountLogin;
                if (CountLogin >= Int32.Parse(Setting.SOLAN_DANGNHAP_SAI))
                {
                    showCaptcha = true;
                }
                //Sessions.UpdateMessage(SessionKeyList.MessageError, Messages.LOGIN_USER_INVALID);
                //Error(Resources.AccountMessage.LOGIN_USER_INVALID);
                //log activity
                SaveLog.SaveSecLog(ActionGroup.SECURITY_ACCESS, ActionName.sec_user, userViewModel.UserName, LogStatus.Fail, AccountMessage.LOGIN_USER_INVALID, userViewModel.UserName);
                return Json(new JsonResponseViewModel(false, new[] { AccountMessage.LOGIN_USER_INVALID}, showCaptcha));
            }
        }

        public ActionResult ReloadCaptcha()
        {
            return PartialView("../Shared/_Captcha");
        }

        public ActionResult LogOff()
        {
            AuthenService.Logout();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult UnAuthorize()
        {
            return View();
        }

        public JsonResult DoEncrypt(string strInput)
        {
            string strOut = Cryptography.EncryptBase64(strInput);

            return Json(new { strOut }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel passwordModel)
        {
            if (AuthenService.ChangePassword(AccountUtils.CurrentUserId(), AccountUtils.CurrentUsername(), Cryptography.EncryptMD5(passwordModel.OldPassword), Cryptography.EncryptMD5(passwordModel.NewPassword)))
            {
                AuthenService.Logout();

                return Json(new JsonResponseViewModel(true, new[] { AccountMessage.CHANGE_PASSWORD_SUCCESS, "/Account/Login" }));
            }

            return Json(JsonResponseViewModel.CreateFail(AccountMessage.CHANGE_PASSWORD_FAIL));
        }

        public ActionResult ChangeForgotPassword()
        {
            var nv = SettingDataService.ChiTietNhanVienTheoMaNhanVien(AccountUtils.CurrentUsername());
            ViewBag.message = string.Format(AccountMessage.CHANGE_TEMP_PASSWORD, nv.HoVaTenNV);
            ChangePasswordModel model = new ChangePasswordModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult ChangeForgotPassword(ChangePasswordModel passwordModel)
        {
            if (AuthenService.ChangePassword(AccountUtils.CurrentUserId(), AccountUtils.CurrentUsername(), null, Cryptography.EncryptMD5(passwordModel.NewPassword)))
            {
                AuthenService.Logout();

                return Json(new JsonResponseViewModel(true, new[] { AccountMessage.CHANGE_PASSWORD_SUCCESS, "/Home/Index" }));
            }

            return Json(new JsonResponseViewModel(false, new[] { AccountMessage.CHANGE_PASSWORD_FAIL }, passwordModel));
            
            //return View(passwordModel);
        }

        public ActionResult ForgotPassword()
        {
            return View(new ForgotPasswordModel());
        }

        [CaptchaValidation("captcha")]
        [HttpPost]
        public ActionResult ForgotPassword(bool captchaValid ,ForgotPasswordModel passwordModel)
        {
            if (!captchaValid)
            {
                return Json(new JsonResponseViewModel(false, new[] { AccountMessage.CAPTCHA_INVALID }));
            }

            int responseCode = 0;
            var isSuccess = ForgotPassword(passwordModel.Email, Setting.TACM_SYSTEM, Setting.URL_DATLAIMATKHAU, ref responseCode);
            if (isSuccess && responseCode == 1)
            {
                //Success(AccountMessage.FORGOT_PASSWORD_SUCCESS);

                //return RedirectToAction("Login");
                return Json(new JsonResponseViewModel(true, new[] { AccountMessage.FORGOT_PASSWORD_SUCCESS, "/Account/Login" }));
            }
            else 
            {
                if (responseCode == -1) //email does not exist
                    //Error(AccountMessage.FORGOT_PASSWORD_EMAIL_NOT_EXIST);
                    return Json(new JsonResponseViewModel(false, new[] { AccountMessage.FORGOT_PASSWORD_EMAIL_NOT_EXIST }, passwordModel));
                else //exception
                    //Error(AccountMessage.FORGOT_PASSWORD_FAIL);
                    return Json(new JsonResponseViewModel(false, new[] { AccountMessage.FORGOT_PASSWORD_FAIL }, passwordModel));

                //return View(passwordModel);
            }
        }

        public static bool ForgotPassword(string email, string appName, string appUrl, ref int responseCode)
        {
            try
            {
                //string sessionkey = Cryptography.EncryptBase64(string.Format("{0}|{1}", email, DateTime.Today.AddDays(3).Ticks));
                //string resetLink = string.Format("{0}/Account/ResetPassword?sessionkey={1}", appUrl, sessionkey);

                string newPlainTextPassword = Commons.GenerateRandomString(8, 2);
                string newPassword = Cryptography.EncryptMD5(newPlainTextPassword);
                DateTime expiredDate = DateTime.Now.AddHours(24);

                var user = AuthenDataAccess.GetForgotPassword(email, newPassword, expiredDate, ref responseCode);
                if (user != null)
                {
                    try
                    {
                        bool isGmailSmtp = Protect.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["IS_GMAIL_SMTP"], true);
                        if (isGmailSmtp)
                        {
                            GMail m = new GMail();
                            m.Sender = appName;
                            m.To = email;
                            m.Subject = Messages.FORGOT_PASSWORD_EMAIL_SUBJECT;
                            string body = Messages.FORGOT_PASSWORD_EMAIL_BODY;
                            body = body.Replace("{_Name_}", user.Full_Name);
                            body = body.Replace("{_Email_}", email);
                            body = body.Replace("{_NewPassword_}", newPlainTextPassword);
                            body = body.Replace("{_ExpiredDate_}", expiredDate.ToString("dd/MM/yyyy HH:mm:ss"));
                            m.Body = body;

                            m.IsHTMLBody = true;

                            m.Send();
                        }
                        else
                        {
                            //Mail m = new Mail();
                            //m.Sender = appName;
                            //m.To = email;
                            //m.Subject = Messages.FORGOT_PASSWORD_EMAIL_SUBJECT;
                            string body = Messages.FORGOT_PASSWORD_EMAIL_BODY;
                            body = body.Replace("{_Name_}", user.Full_Name);
                            body = body.Replace("{_Email_}", email);
                            body = body.Replace("{_NewPassword_}", newPlainTextPassword);
                            body = body.Replace("{_ExpiredDate_}", expiredDate.ToString("dd/MM/yyyy HH:mm:ss"));
                            //m.Body = body;
                            //m.IsEnableSSL = false;

                            //m.Send();
                            Commons.SendMail(Messages.FORGOT_PASSWORD_EMAIL_SUBJECT, email, body);
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteLog.Exeption(ex);

                        return false;
                    }
                    //log activity
                    //WriteLog.Info(string.Format("Forgot password successfully for email [{0}]", email));

                    return true;
                }
                else
                {
                    //log activity
                    //WriteLog.Info("Forgot password fail, object USER is NULL");
                    return false;
                }
            }
            catch (Exception ex)
            {
                //log activity
                WriteLog.Exeption(ex);

                return false;
            }
        }
    }
}