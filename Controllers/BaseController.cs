using System;
using System.Web.Mvc;

namespace HX.Terminal.Controllers
{
    /// <summary>
    /// コントローラー基底クラス
    /// </summary>
    public class BaseController : Controller
    {
        #region 定数

        /// <summary>
        /// 事務センター責任者権限ID
        /// </summary>
        private const string ROLE_OFFICE_MANAGER = "21";

        /// <summary>
        /// 工業担当者権限ID
        /// </summary>
        private const string ROLE_INDUSTRIAL_STAFF = "60";

        /// <summary>
        /// 管理者権限ID
        /// </summary>
        private const string ROLE_ADMINISTRATOR = "99";

        #endregion

        #region プロテクトメソッド

        /// <summary>
        /// 権限チェック
        /// </summary>
        /// <returns>アクセス権限がある場合true</returns>
        protected bool CheckAuthorization()
        {
            try
            {
                // セッションからユーザー情報を取得（実装は環境に応じて調整）
                var userRole = Session["UserRole"]?.ToString();
                
                if (string.IsNullOrEmpty(userRole))
                {
                    return false;
                }

                // 許可されたロールかチェック
                return userRole == ROLE_OFFICE_MANAGER || 
                       userRole == ROLE_INDUSTRIAL_STAFF || 
                       userRole == ROLE_ADMINISTRATOR;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 現在のユーザーIDを取得
        /// </summary>
        /// <returns>ユーザーID</returns>
        protected string GetCurrentUserId()
        {
            try
            {
                return Session["UserId"]?.ToString() ?? "SYSTEM";
            }
            catch (Exception)
            {
                return "SYSTEM";
            }
        }

        /// <summary>
        /// 現在のユーザー名を取得
        /// </summary>
        /// <returns>ユーザー名</returns>
        protected string GetCurrentUserName()
        {
            try
            {
                return Session["UserName"]?.ToString() ?? "システム";
            }
            catch (Exception)
            {
                return "システム";
            }
        }

        #endregion
    }
}
