using System.Web;
using System.Web.Optimization;

namespace CTInfo
{
    public class BundleConfig
    {
        // 有关捆绑的详细信息，请访问 https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bootstrap").Include(
                      "~/dist/css/AdminLTE.min.css",
                      "~/dist/css/skins/_all-skins.min.css"));
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery/jquery-1.12.0.min.js"));
        }
    }
}
