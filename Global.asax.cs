using System;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace HX.Terminal
{
    /// <summary>
    /// Webアプリケーション初期化クラス
    /// </summary>
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // MVC設定
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            
            // ログ出力（log4netなどを使用）
            // Logger.Error("Application Error", exception);
            
            Server.ClearError();
            Response.Redirect("~/Error.aspx");
        }
    }

    /// <summary>
    /// ルート設定
    /// </summary>
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // 端末管理用ルート
            routes.MapRoute(
                name: "Terminal",
                url: "Terminal/{action}",
                defaults: new { controller = "Terminal", action = "Top" },
                namespaces: new[] { "HX.Terminal.Controllers" }
            );

            // デフォルトルート
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }

    /// <summary>
    /// バンドル設定
    /// </summary>
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            // jQuery
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // Bootstrap
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/Content/bootstrap.css",
                        "~/Content/site.css"));

            // 端末管理専用CSS/JS
            bundles.Add(new ScriptBundle("~/bundles/terminal").Include(
                        "~/Scripts/terminal.js"));

            bundles.Add(new StyleBundle("~/Content/terminal").Include(
                        "~/Content/terminal.css"));

            // 本番環境では最適化を有効にする
#if !DEBUG
            BundleTable.EnableOptimizations = true;
#endif
        }
    }
}
