using System;
using System.Collections.Generic;
using System.Web.Mvc;
using HX.Terminal.BusinessLogic;
using HX.Terminal.Models;

namespace HX.Terminal.Controllers
{
    /// <summary>
    /// 端末登録コントローラー
    /// </summary>
    public class TerminalController : BaseController
    {
        private readonly TerminalRegistBusinessLogic terminalLogic;
        private readonly SimRegistBusinessLogic simLogic;
        private readonly TerminalSimHimodukeBusinessLogic himodukeLogic;

        public TerminalController()
        {
            terminalLogic = new TerminalRegistBusinessLogic();
            simLogic = new SimRegistBusinessLogic();
            himodukeLogic = new TerminalSimHimodukeBusinessLogic();
        }

        /// <summary>
        /// トップ画面
        /// </summary>
        [HttpGet]
        public ActionResult Top()
        {
            // 権限チェック
            if (!CheckAuthorization())
            {
                return RedirectToAction("Error", "Home");
            }

            return View();
        }

        /// <summary>
        /// KDDI受領データ登録画面
        /// </summary>
        [HttpGet]
        public ActionResult Regist()
        {
            if (!CheckAuthorization())
            {
                return RedirectToAction("Error", "Home");
            }

            return View();
        }

        /// <summary>
        /// 端末データCSVアップロード
        /// </summary>
        [HttpPost]
        public ActionResult UploadTerminalCsv()
        {
            try
            {
                var file = Request.Files["terminalFile"];
                
                // ファイルバリデーション
                var fileValidation = terminalLogic.ValidateCsvFile(file);
                if (!fileValidation.IsValid)
                {
                    return Json(new { success = false, message = string.Join("\n", fileValidation.Errors) });
                }

                // CSVファイル処理
                var result = terminalLogic.ProcessTerminalCsvFile(file, "Regist.aspx", GetCurrentUserId());
                
                if (result.IsValid)
                {
                    return Json(new { success = true, message = result.Message });
                }
                else
                {
                    return Json(new { success = false, message = string.Join("\n", result.Errors) });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"処理中にエラーが発生しました：{ex.Message}" });
            }
        }

        /// <summary>
        /// SIMデータCSVアップロード
        /// </summary>
        [HttpPost]
        public ActionResult UploadSimCsv()
        {
            try
            {
                var file = Request.Files["simFile"];
                
                // ファイルバリデーション
                var fileValidation = terminalLogic.ValidateCsvFile(file);
                if (!fileValidation.IsValid)
                {
                    return Json(new { success = false, message = string.Join("\n", fileValidation.Errors) });
                }

                // CSVファイル処理
                var result = simLogic.ProcessSimCsvFile(file, "Regist.aspx", GetCurrentUserId());
                
                if (result.IsValid)
                {
                    return Json(new { success = true, message = result.Message });
                }
                else
                {
                    return Json(new { success = false, message = string.Join("\n", result.Errors) });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"処理中にエラーが発生しました：{ex.Message}" });
            }
        }

        /// <summary>
        /// 端末-SIM情報紐付け登録画面
        /// </summary>
        [HttpGet]
        public ActionResult Himoduke()
        {
            if (!CheckAuthorization())
            {
                return RedirectToAction("Error", "Home");
            }

            return View();
        }

        /// <summary>
        /// 端末-SIM情報紐付け登録処理
        /// </summary>
        [HttpPost]
        public ActionResult RegisterHimoduke(string terminalNo, string icCardNo)
        {
            try
            {
                var result = himodukeLogic.ProcessHimoduke(terminalNo, icCardNo, "Himoduke.aspx", GetCurrentUserId());
                
                if (result.IsValid)
                {
                    ViewBag.SuccessMessage = result.Message;
                    // 入力フィールドクリア用
                    ModelState.Clear();
                    return View("Himoduke");
                }
                else
                {
                    ViewBag.ErrorMessage = string.Join("\n", result.Errors);
                    return View("Himoduke");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"処理中にエラーが発生しました：{ex.Message}";
                return View("Himoduke");
            }
        }

        /// <summary>
        /// SIM情報紐付け結果一覧画面
        /// </summary>
        [HttpGet]
        public ActionResult Result(int page = 1)
        {
            if (!CheckAuthorization())
            {
                return RedirectToAction("Error", "Home");
            }

            try
            {
                var himodukeList = himodukeLogic.GetTodayHimodukeList(page);
                
                ViewBag.CurrentPage = page;
                ViewBag.HasData = himodukeList.Count > 0;
                
                if (himodukeList.Count == 0)
                {
                    ViewBag.NoDataMessage = "本日登録されたデータはありません。";
                }

                return View(himodukeList);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"データ取得中にエラーが発生しました：{ex.Message}";
                return View(new List<TerminalSimHimodukeModel>());
            }
        }

        /// <summary>
        /// 紐付け情報削除処理
        /// </summary>
        [HttpPost]
        public ActionResult DeleteHimoduke(string terminalNo, string icCardNo)
        {
            try
            {
                var result = himodukeLogic.DeleteHimoduke(terminalNo, icCardNo);
                
                if (result.IsValid)
                {
                    return Json(new { success = true, message = result.Message });
                }
                else
                {
                    return Json(new { success = false, message = string.Join("\n", result.Errors) });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"削除処理中にエラーが発生しました：{ex.Message}" });
            }
        }

        /// <summary>
        /// 権限チェック
        /// </summary>
        private bool CheckAuthorization()
        {
            var userRole = GetCurrentUserRole();
            return userRole == "21" || userRole == "60" || userRole == "99"; // 事務センター責任者、工業担当者、管理者権限
        }
    }

    /// <summary>
    /// 基底コントローラー
    /// </summary>
    public abstract class BaseController : Controller
    {
        /// <summary>
        /// 現在のユーザーIDを取得
        /// </summary>
        protected string GetCurrentUserId()
        {
            // セッションまたは認証情報からユーザーIDを取得
            return Session["UserId"]?.ToString() ?? "system";
        }

        /// <summary>
        /// 現在のユーザーロールを取得
        /// </summary>
        protected string GetCurrentUserRole()
        {
            // セッションまたは認証情報からユーザーロールを取得
            return Session["UserRole"]?.ToString() ?? "";
        }
    }
}
